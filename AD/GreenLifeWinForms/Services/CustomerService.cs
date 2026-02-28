using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using GreenLifeWinForms.Database;
using GreenLifeWinForms.Models;

namespace GreenLifeWinForms.Services
{
    public class CustomerService
    {
        private DatabaseManager db = DatabaseManager.Instance;

        /// <summary>
        /// Get all customers
        /// </summary>
        public List<Customer> GetAllCustomers()
        {
            try
            {
                string query = "SELECT * FROM Customers ORDER BY RegisteredDate DESC";
                DataTable dt = db.ExecuteQuery(query);
                return DataTableToCustomerList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve customers: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get customer by ID
        /// </summary>
        public Customer GetCustomerById(int customerId)
        {
            try
            {
                string query = "SELECT * FROM Customers WHERE CustomerId = @customerId";
                SqlParameter[] parameters = {
                    new SqlParameter("@customerId", customerId)
                };
                DataTable dt = db.ExecuteQuery(query, parameters);
                
                if (dt.Rows.Count > 0)
                {
                    return DataRowToCustomer(dt.Rows[0]);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve customer: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Update customer information
        /// </summary>
        public bool UpdateCustomer(Customer customer)
        {
            try
            {
                string query = @"UPDATE Customers 
                               SET FullName = @fullName, 
                                   Email = @email, 
                                   Phone = @phone, 
                                   Address = @address,
                                   IsActive = @isActive
                               WHERE CustomerId = @customerId";

                SqlParameter[] parameters = {
                    new SqlParameter("@fullName", customer.FullName),
                    new SqlParameter("@email", customer.Email),
                    new SqlParameter("@phone", customer.Phone),
                    new SqlParameter("@address", customer.Address),
                    new SqlParameter("@isActive", customer.IsActive ? 1 : 0),
                    new SqlParameter("@customerId", customer.CustomerId)
                };

                int result = db.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update customer: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deactivate customer account
        /// </summary>
        public bool DeactivateCustomer(int customerId)
        {
            try
            {
                string query = "UPDATE Customers SET IsActive = 0 WHERE CustomerId = @customerId";
                SqlParameter[] parameters = {
                    new SqlParameter("@customerId", customerId)
                };
                int result = db.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to deactivate customer: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Activate customer account
        /// </summary>
        public bool ActivateCustomer(int customerId)
        {
            try
            {
                string query = "UPDATE Customers SET IsActive = 1 WHERE CustomerId = @customerId";
                SqlParameter[] parameters = {
                    new SqlParameter("@customerId", customerId)
                };
                int result = db.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to activate customer: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Multi-field search across name, email, phone, address and customer ID
        /// </summary>
        public List<Customer> SearchCustomers(string searchTerm)
        {
            try
            {
                string query = @"SELECT * FROM Customers 
                               WHERE FullName LIKE @searchTerm 
                               OR Email LIKE @searchTerm
                               OR Phone LIKE @searchTerm
                               OR Address LIKE @searchTerm
                               OR CAST(CustomerId AS NVARCHAR) LIKE @searchTerm
                               ORDER BY RegisteredDate DESC";
                SqlParameter[] parameters = {
                    new SqlParameter("@searchTerm", $"%{searchTerm}%")
                };
                DataTable dt = db.ExecuteQuery(query, parameters);
                return DataTableToCustomerList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception($"Customer search failed: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get customer count
        /// </summary>
        public int GetCustomerCount()
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Customers WHERE IsActive = 1";
                object result = db.ExecuteScalar(query);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get customer count: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get active customers
        /// </summary>
        public List<Customer> GetActiveCustomers()
        {
            try
            {
                string query = "SELECT * FROM Customers WHERE IsActive = 1 ORDER BY RegisteredDate DESC";
                DataTable dt = db.ExecuteQuery(query);
                return DataTableToCustomerList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve active customers: {ex.Message}", ex);
            }
        }

        // Helper methods
        private List<Customer> DataTableToCustomerList(DataTable dt)
        {
            List<Customer> customers = new List<Customer>();
            foreach (DataRow row in dt.Rows)
            {
                customers.Add(DataRowToCustomer(row));
            }
            return customers;
        }

        private Customer DataRowToCustomer(DataRow row)
        {
            return new Customer
            {
                CustomerId = Convert.ToInt32(row["CustomerId"]),
                FullName = row["FullName"].ToString(),
                Email = row["Email"].ToString(),
                Phone = row["Phone"].ToString(),
                Address = row["Address"].ToString(),
                Password = row["Password"].ToString(),
                IsActive = Convert.ToBoolean(row["IsActive"]),
                RegisteredDate = Convert.ToDateTime(row["RegisteredDate"])
            };
        }
    }
}
