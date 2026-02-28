using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using GreenLifeWinForms.Database;
using GreenLifeWinForms.Models;

namespace GreenLifeWinForms.Services
{
    public class ProductService
    {
        private DatabaseManager db = DatabaseManager.Instance;

        /// <summary>
        /// Get all active products
        /// </summary>
        public List<Product> GetAllProducts()
        {
            try
            {
                string query = "SELECT * FROM Products WHERE IsActive = 1 ORDER BY Name";
                DataTable dt = db.ExecuteQuery(query);
                return DataTableToProductList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve products: {ex.Message}");
            }
        }

        /// <summary>
        /// Get product by ID
        /// </summary>
        public Product GetProductById(int productId)
        {
            try
            {
                string query = "SELECT * FROM Products WHERE ProductId = @productId";
                SqlParameter[] parameters = {
                    new SqlParameter("@productId", productId)
                };

                DataTable dt = db.ExecuteQuery(query, parameters);
                if (dt.Rows.Count > 0)
                {
                    return DataRowToProduct(dt.Rows[0]);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve product: {ex.Message}");
            }
        }

        /// <summary>
        /// Add a new product
        /// </summary>
        public bool AddProduct(Product product)
        {
            try
            {
                string query = @"INSERT INTO Products (Name, Category, Supplier, Price, Stock, Discount, IsActive, Image, Description, CreatedDate) 
                                VALUES (@name, @category, @supplier, @price, @stock, @discount, @isActive, @image, @description, @createdDate)";

                SqlParameter[] parameters = {
                    new SqlParameter("@name", product.Name),
                    new SqlParameter("@category", product.Category),
                    new SqlParameter("@supplier", product.Supplier),
                    new SqlParameter("@price", product.Price),
                    new SqlParameter("@stock", product.Stock),
                    new SqlParameter("@discount", product.Discount),
                    new SqlParameter("@isActive", product.IsActive ? 1 : 0),
                    new SqlParameter("@image", product.Image ?? ""),
                    new SqlParameter("@description", product.Description ?? ""),
                    new SqlParameter("@createdDate", DateTime.Now)
                };

                int result = db.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to add product: {ex.Message}");
            }
        }

        /// <summary>
        /// Update an existing product
        /// </summary>
        public bool UpdateProduct(Product product)
        {
            try
            {
                string query = @"UPDATE Products SET 
                                Name = @name, 
                                Category = @category, 
                                Supplier = @supplier, 
                                Price = @price, 
                                Stock = @stock, 
                                Discount = @discount, 
                                IsActive = @isActive, 
                                Image = @image, 
                                Description = @description 
                                WHERE ProductId = @productId";

                SqlParameter[] parameters = {
                    new SqlParameter("@name", product.Name),
                    new SqlParameter("@category", product.Category),
                    new SqlParameter("@supplier", product.Supplier),
                    new SqlParameter("@price", product.Price),
                    new SqlParameter("@stock", product.Stock),
                    new SqlParameter("@discount", product.Discount),
                    new SqlParameter("@isActive", product.IsActive ? 1 : 0),
                    new SqlParameter("@image", product.Image ?? ""),
                    new SqlParameter("@description", product.Description ?? ""),
                    new SqlParameter("@productId", product.ProductId)
                };

                int result = db.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update product: {ex.Message}");
            }
        }

        /// <summary>
        /// Delete a product (soft delete)
        /// </summary>
        public bool DeleteProduct(int productId)
        {
            try
            {
                string query = "UPDATE Products SET IsActive = 0 WHERE ProductId = @productId";
                SqlParameter[] parameters = {
                    new SqlParameter("@productId", productId)
                };

                int result = db.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete product: {ex.Message}");
            }
        }

        /// <summary>
        /// Multi-field search across name, category, supplier and description
        /// </summary>
        public List<Product> SearchProducts(string searchTerm, string category = "All")
        {
            try
            {
                string query = "SELECT * FROM Products WHERE IsActive = 1";

                List<SqlParameter> parameters = new List<SqlParameter>();

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    query += " AND (Name LIKE @searchTerm OR Supplier LIKE @searchTerm OR Description LIKE @searchTerm OR CAST(ProductId AS NVARCHAR) LIKE @searchTerm)";
                    parameters.Add(new SqlParameter("@searchTerm", $"%{searchTerm}%"));
                }

                if (!string.IsNullOrWhiteSpace(category) && category != "All")
                {
                    query += " AND Category = @category";
                    parameters.Add(new SqlParameter("@category", category));
                }

                query += " ORDER BY Name";

                DataTable dt = db.ExecuteQuery(query, parameters.ToArray());
                return DataTableToProductList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to search products: {ex.Message}");
            }
        }

        /// <summary>
        /// Get products with low stock (below threshold)
        /// </summary>
        public List<Product> GetLowStockProducts(int threshold = 50)
        {
            try
            {
                string query = "SELECT * FROM Products WHERE IsActive = 1 AND Stock < @threshold ORDER BY Stock";
                SqlParameter[] parameters = {
                    new SqlParameter("@threshold", threshold)
                };

                DataTable dt = db.ExecuteQuery(query, parameters);
                return DataTableToProductList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve low stock products: {ex.Message}");
            }
        }

        /// <summary>
        /// Get all product categories
        /// </summary>
        public List<string> GetCategories()
        {
            try
            {
                string query = "SELECT DISTINCT Category FROM Products WHERE IsActive = 1 ORDER BY Category";
                DataTable dt = db.ExecuteQuery(query);

                List<string> categories = new List<string> { "All" };
                foreach (DataRow row in dt.Rows)
                {
                    categories.Add(row["Category"].ToString());
                }

                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve categories: {ex.Message}");
            }
        }

        /// <summary>
        /// Get all product categories (alias for GetCategories)
        /// </summary>
        public List<string> GetProductCategories()
        {
            return GetCategories();
        }

        /// <summary>
        /// Update product stock
        /// </summary>
        public bool UpdateStock(int productId, int quantity)
        {
            try
            {
                string query = "UPDATE Products SET Stock = Stock + @quantity WHERE ProductId = @productId";
                SqlParameter[] parameters = {
                    new SqlParameter("@quantity", quantity),
                    new SqlParameter("@productId", productId)
                };

                int result = db.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update stock: {ex.Message}");
            }
        }

        /// <summary>
        /// Get product with rating information
        /// </summary>
        public Product GetProductWithRating(int productId)
        {
            try
            {
                Product product = GetProductById(productId);
                if (product != null)
                {
                    // Get average rating and review count
                    string query = @"SELECT AVG(Rating) as AvgRating, COUNT(*) as ReviewCount 
                                    FROM Reviews WHERE ProductId = @productId";
                    SqlParameter[] parameters = {
                        new SqlParameter("@productId", productId)
                    };

                    DataTable dt = db.ExecuteQuery(query, parameters);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];
                        product.AverageRating = row["AvgRating"] != DBNull.Value ? Convert.ToDouble(row["AvgRating"]) : 0;
                        product.ReviewCount = row["ReviewCount"] != DBNull.Value ? Convert.ToInt32(row["ReviewCount"]) : 0;
                    }
                }
                return product;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve product with rating: {ex.Message}");
            }
        }

        // Helper methods
        private List<Product> DataTableToProductList(DataTable dt)
        {
            List<Product> products = new List<Product>();
            foreach (DataRow row in dt.Rows)
            {
                products.Add(DataRowToProduct(row));
            }
            return products;
        }

        private Product DataRowToProduct(DataRow row)
        {
            return new Product
            {
                ProductId = Convert.ToInt32(row["ProductId"]),
                Name = row["Name"].ToString(),
                Category = row["Category"].ToString(),
                Supplier = row["Supplier"].ToString(),
                Price = Convert.ToDecimal(row["Price"]),
                Stock = Convert.ToInt32(row["Stock"]),
                Discount = Convert.ToDecimal(row["Discount"]),
                IsActive = Convert.ToBoolean(row["IsActive"]),
                Image = row["Image"].ToString(),
                Description = row["Description"].ToString(),
                CreatedDate = Convert.ToDateTime(row["CreatedDate"])
            };
        }
    }
}
