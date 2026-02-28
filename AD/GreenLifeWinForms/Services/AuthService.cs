using System;
using System.Data;
using System.Data.SqlClient;
using GreenLifeWinForms.Database;
using GreenLifeWinForms.Models;
using GreenLifeWinForms.Utils;

namespace GreenLifeWinForms.Services
{
    public class AuthService
    {
        private DatabaseManager db = DatabaseManager.Instance;

        /// <summary>
        /// Authenticate admin user
        /// </summary>
        public Admin AdminLogin(string username, string password)
        {
            try
            {
                string query = "SELECT * FROM Admins WHERE Username = @username";
                SqlParameter[] parameters = {
                    new SqlParameter("@username", username)
                };

                DataTable dt = db.ExecuteQuery(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    string storedPassword = row["Password"].ToString();

                    // Verify hashed password; fall back to plain-text for legacy data
                    bool match = PasswordHelper.VerifyPassword(password, storedPassword)
                              || storedPassword == password;

                    if (match)
                    {
                        return new Admin
                        {
                            AdminId = Convert.ToInt32(row["AdminId"]),
                            Username = row["Username"].ToString(),
                            Password = storedPassword,
                            FullName = row["FullName"].ToString(),
                            CreatedDate = Convert.ToDateTime(row["CreatedDate"])
                        };
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Admin login failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Authenticate customer user
        /// </summary>
        public Customer CustomerLogin(string email, string password)
        {
            try
            {
                string query = "SELECT * FROM Customers WHERE Email = @email AND IsActive = 1";
                SqlParameter[] parameters = {
                    new SqlParameter("@email", email)
                };

                DataTable dt = db.ExecuteQuery(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    string storedPassword = row["Password"].ToString();

                    // Verify hashed password; fall back to plain-text for legacy data
                    bool match = PasswordHelper.VerifyPassword(password, storedPassword)
                              || storedPassword == password;

                    if (match)
                    {
                        return new Customer
                        {
                            CustomerId = Convert.ToInt32(row["CustomerId"]),
                            FullName = row["FullName"].ToString(),
                            Email = row["Email"].ToString(),
                            Phone = row["Phone"].ToString(),
                            Address = row["Address"].ToString(),
                            Password = storedPassword,
                            IsActive = Convert.ToBoolean(row["IsActive"]),
                            RegisteredDate = Convert.ToDateTime(row["RegisteredDate"])
                        };
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Customer login failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Register a new customer
        /// </summary>
        public bool RegisterCustomer(Customer customer)
        {
            try
            {
                // Check if email already exists
                if (EmailExists(customer.Email))
                {
                    throw new Exception("Email already registered");
                }

                // Hash the password before storing
                string hashedPassword = PasswordHelper.HashPassword(customer.Password);

                string query = @"INSERT INTO Customers (FullName, Email, Phone, Address, Password, IsActive, RegisteredDate) 
                                VALUES (@fullName, @email, @phone, @address, @password, @isActive, @registeredDate)";

                SqlParameter[] parameters = {
                    new SqlParameter("@fullName", customer.FullName),
                    new SqlParameter("@email", customer.Email),
                    new SqlParameter("@phone", customer.Phone),
                    new SqlParameter("@address", customer.Address),
                    new SqlParameter("@password", hashedPassword),
                    new SqlParameter("@isActive", customer.IsActive ? 1 : 0),
                    new SqlParameter("@registeredDate", customer.RegisteredDate)
                };

                int result = db.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Customer registration failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Check if email already exists
        /// </summary>
        public bool EmailExists(string email)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Customers WHERE Email = @email";
                SqlParameter[] parameters = {
                    new SqlParameter("@email", email)
                };

                object result = db.ExecuteScalar(query, parameters);
                return Convert.ToInt32(result) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Email check failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Update customer password
        /// </summary>
        public bool UpdatePassword(int customerId, string newPassword)
        {
            try
            {
                string hashedPassword = PasswordHelper.HashPassword(newPassword);
                string query = "UPDATE Customers SET Password = @password WHERE CustomerId = @customerId";
                SqlParameter[] parameters = {
                    new SqlParameter("@password", hashedPassword),
                    new SqlParameter("@customerId", customerId)
                };

                int result = db.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Password update failed: {ex.Message}");
            }
        }
    }
}
