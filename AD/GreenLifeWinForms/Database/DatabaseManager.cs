using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace GreenLifeWinForms.Database
{
    public class DatabaseManager
    {
        private static DatabaseManager _instance;
        private static readonly object _lock = new object();
        private string _connectionString;

        private DatabaseManager()
        {
            // SQL Server connection string
            _connectionString = @"Data Source=MSI\SQLEXPRESS;Initial Catalog=GreenLifeDB;Integrated Security=True;Persist Security Info=False;Pooling=True;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False;";
            
            // Initialize database on first run
            InitializeDatabase();
        }

        public static DatabaseManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DatabaseManager();
                        }
                    }
                }
                return _instance;
            }
        }

        private void InitializeDatabase()
        {
            try
            {
                // Check if database exists, if not create it
                if (!DatabaseExists())
                {
                    CreateDatabase();
                    CreateTables();
                    SeedData();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Database initialization failed: {ex.Message}", ex);
            }
        }

        private bool DatabaseExists()
        {
            try
            {
                string masterConnectionString = @"Data Source=MSI\SQLEXPRESS;Integrated Security=True;Persist Security Info=False;Pooling=False;";
                
                using (SqlConnection conn = new SqlConnection(masterConnectionString))
                {
                    conn.Open();
                    string query = "SELECT database_id FROM sys.databases WHERE name = 'GreenLifeDB'";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        return result != null;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        private void CreateDatabase()
        {
            try
            {
                string masterConnectionString = @"Data Source=MSI\SQLEXPRESS;Integrated Security=True;Persist Security Info=False;Pooling=False;";
                
                using (SqlConnection conn = new SqlConnection(masterConnectionString))
                {
                    conn.Open();
                    string query = "CREATE DATABASE GreenLifeDB";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create database: {ex.Message}", ex);
            }
        }

        private void CreateTables()
        {
            string schemaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "DatabaseSchema.sql");
            
            if (File.Exists(schemaPath))
            {
                string schema = File.ReadAllText(schemaPath);
                ExecuteScript(schema);
            }
            else
            {
                // Fallback: Create tables programmatically
                CreateTablesManually();
            }
        }

        private void CreateTablesManually()
        {
            string[] createTableQueries = new string[]
            {
                @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Products')
                BEGIN
                    CREATE TABLE Products (
                        ProductId INT PRIMARY KEY IDENTITY(1,1),
                        Name NVARCHAR(200) NOT NULL,
                        Category NVARCHAR(100) NOT NULL,
                        Supplier NVARCHAR(200) NOT NULL,
                        Price DECIMAL(10,2) NOT NULL,
                        Stock INT NOT NULL,
                        Discount DECIMAL(5,2) DEFAULT 0,
                        IsActive BIT DEFAULT 1,
                        Image NVARCHAR(500),
                        Description NVARCHAR(MAX),
                        CreatedDate DATETIME DEFAULT GETDATE()
                    );
                END",
                
                @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Customers')
                BEGIN
                    CREATE TABLE Customers (
                        CustomerId INT PRIMARY KEY IDENTITY(1,1),
                        FullName NVARCHAR(200) NOT NULL,
                        Email NVARCHAR(200) UNIQUE NOT NULL,
                        Phone NVARCHAR(20) NOT NULL,
                        Address NVARCHAR(500) NOT NULL,
                        Password NVARCHAR(200) NOT NULL,
                        IsActive BIT DEFAULT 1,
                        RegisteredDate DATETIME DEFAULT GETDATE()
                    );
                END",
                
                @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Orders')
                BEGIN
                    CREATE TABLE Orders (
                        OrderId INT PRIMARY KEY IDENTITY(1,1),
                        CustomerId INT NOT NULL,
                        CustomerName NVARCHAR(200) NOT NULL,
                        OrderDate DATETIME DEFAULT GETDATE(),
                        Status NVARCHAR(20) CHECK(Status IN ('Pending', 'Shipped', 'Delivered')) DEFAULT 'Pending',
                        Subtotal DECIMAL(10,2) NOT NULL,
                        DiscountTotal DECIMAL(10,2) DEFAULT 0,
                        GrandTotal DECIMAL(10,2) NOT NULL,
                        FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId)
                    );
                END",
                
                @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'OrderItems')
                BEGIN
                    CREATE TABLE OrderItems (
                        OrderItemId INT PRIMARY KEY IDENTITY(1,1),
                        OrderId INT NOT NULL,
                        ProductId INT NOT NULL,
                        ProductName NVARCHAR(200) NOT NULL,
                        Quantity INT NOT NULL,
                        UnitPrice DECIMAL(10,2) NOT NULL,
                        Discount DECIMAL(5,2) DEFAULT 0,
                        LineTotal DECIMAL(10,2) NOT NULL,
                        FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
                        FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
                    );
                END",
                
                @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Reviews')
                BEGIN
                    CREATE TABLE Reviews (
                        ReviewId INT PRIMARY KEY IDENTITY(1,1),
                        ProductId INT NOT NULL,
                        CustomerId INT NOT NULL,
                        CustomerName NVARCHAR(200) NOT NULL,
                        Rating INT CHECK(Rating >= 1 AND Rating <= 5) NOT NULL,
                        Comment NVARCHAR(MAX),
                        ReviewDate DATETIME DEFAULT GETDATE(),
                        FOREIGN KEY (ProductId) REFERENCES Products(ProductId),
                        FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId)
                    );
                END",
                
                @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Admins')
                BEGIN
                    CREATE TABLE Admins (
                        AdminId INT PRIMARY KEY IDENTITY(1,1),
                        Username NVARCHAR(100) UNIQUE NOT NULL,
                        Password NVARCHAR(200) NOT NULL,
                        FullName NVARCHAR(200) NOT NULL,
                        CreatedDate DATETIME DEFAULT GETDATE()
                    );
                END"
            };

            foreach (string query in createTableQueries)
            {
                ExecuteNonQuery(query);
            }
        }

        private void SeedData()
        {
            string seedPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "SeedData.sql");
            
            if (File.Exists(seedPath))
            {
                string seedSql = File.ReadAllText(seedPath);
                ExecuteScript(seedSql);
            }
        }

        private void ExecuteScript(string script)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    
                    // Split script by GO statements
                    string[] batches = script.Split(new[] { "\r\nGO\r\n", "\nGO\n", "\r\nGO", "GO" }, StringSplitOptions.RemoveEmptyEntries);
                    
                    foreach (string batch in batches)
                    {
                        if (!string.IsNullOrWhiteSpace(batch))
                        {
                            using (SqlCommand cmd = new SqlCommand(batch, conn))
                            {
                                cmd.CommandTimeout = 60;
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Script execution failed: {ex.Message}", ex);
            }
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.CommandTimeout = 30;
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Query execution failed: {ex.Message}", ex);
            }
        }

        public object ExecuteScalar(string query, params SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.CommandTimeout = 30;
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        return cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Scalar query failed: {ex.Message}", ex);
            }
        }

        public DataTable ExecuteQuery(string query, params SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.CommandTimeout = 30;
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Query execution failed: {ex.Message}", ex);
            }
        }

        public SqlDataReader ExecuteReader(string query, SqlConnection conn, params SqlParameter[] parameters)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandTimeout = 30;
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                return cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception($"Reader execution failed: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Test database connection
        /// </summary>
        public bool TestConnection()
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    return conn.State == ConnectionState.Open;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get connection string (for debugging)
        /// </summary>
        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
