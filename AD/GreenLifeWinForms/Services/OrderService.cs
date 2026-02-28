using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using GreenLifeWinForms.Database;
using GreenLifeWinForms.Models;

namespace GreenLifeWinForms.Services
{
    public class OrderService
    {
        private DatabaseManager db = DatabaseManager.Instance;
        private ProductService productService = new ProductService();

        /// <summary>
        /// Create a new order with order items
        /// </summary>
        public int CreateOrder(Order order, List<OrderItem> orderItems)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Insert order
                    string orderQuery = @"INSERT INTO Orders (CustomerId, CustomerName, OrderDate, Status, Subtotal, DiscountTotal, GrandTotal) 
                                        OUTPUT INSERTED.OrderId
                                        VALUES (@customerId, @customerName, @orderDate, @status, @subtotal, @discountTotal, @grandTotal)";

                    SqlCommand orderCmd = new SqlCommand(orderQuery, conn, transaction);
                    orderCmd.Parameters.AddWithValue("@customerId", order.CustomerId);
                    orderCmd.Parameters.AddWithValue("@customerName", order.CustomerName);
                    orderCmd.Parameters.AddWithValue("@orderDate", order.OrderDate);
                    orderCmd.Parameters.AddWithValue("@status", order.Status);
                    orderCmd.Parameters.AddWithValue("@subtotal", order.Subtotal);
                    orderCmd.Parameters.AddWithValue("@discountTotal", order.DiscountTotal);
                    orderCmd.Parameters.AddWithValue("@grandTotal", order.GrandTotal);

                    int orderId = (int)orderCmd.ExecuteScalar();

                    // Insert order items and update stock
                    foreach (OrderItem item in orderItems)
                    {
                        // Insert order item
                        string itemQuery = @"INSERT INTO OrderItems (OrderId, ProductId, ProductName, Quantity, UnitPrice, Discount, LineTotal) 
                                           VALUES (@orderId, @productId, @productName, @quantity, @unitPrice, @discount, @lineTotal)";

                        SqlCommand itemCmd = new SqlCommand(itemQuery, conn, transaction);
                        itemCmd.Parameters.AddWithValue("@orderId", orderId);
                        itemCmd.Parameters.AddWithValue("@productId", item.ProductId);
                        itemCmd.Parameters.AddWithValue("@productName", item.ProductName);
                        itemCmd.Parameters.AddWithValue("@quantity", item.Quantity);
                        itemCmd.Parameters.AddWithValue("@unitPrice", item.UnitPrice);
                        itemCmd.Parameters.AddWithValue("@discount", item.Discount);
                        itemCmd.Parameters.AddWithValue("@lineTotal", item.LineTotal);
                        itemCmd.ExecuteNonQuery();

                        // Update product stock
                        string stockQuery = "UPDATE Products SET Stock = Stock - @quantity WHERE ProductId = @productId";
                        SqlCommand stockCmd = new SqlCommand(stockQuery, conn, transaction);
                        stockCmd.Parameters.AddWithValue("@quantity", item.Quantity);
                        stockCmd.Parameters.AddWithValue("@productId", item.ProductId);
                        stockCmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return orderId;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"Order creation failed: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Get all orders
        /// </summary>
        public List<Order> GetAllOrders()
        {
            try
            {
                string query = "SELECT * FROM Orders ORDER BY OrderDate DESC";
                DataTable dt = db.ExecuteQuery(query);
                return DataTableToOrderList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve orders: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get orders by customer ID
        /// </summary>
        public List<Order> GetOrdersByCustomerId(int customerId)
        {
            try
            {
                string query = "SELECT * FROM Orders WHERE CustomerId = @customerId ORDER BY OrderDate DESC";
                SqlParameter[] parameters = {
                    new SqlParameter("@customerId", customerId)
                };
                DataTable dt = db.ExecuteQuery(query, parameters);
                return DataTableToOrderList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve customer orders: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get order by ID
        /// </summary>
        public Order GetOrderById(int orderId)
        {
            try
            {
                string query = "SELECT * FROM Orders WHERE OrderId = @orderId";
                SqlParameter[] parameters = {
                    new SqlParameter("@orderId", orderId)
                };
                DataTable dt = db.ExecuteQuery(query, parameters);
                
                if (dt.Rows.Count > 0)
                {
                    return DataRowToOrder(dt.Rows[0]);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve order: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get order items by order ID
        /// </summary>
        public List<OrderItem> GetOrderItems(int orderId)
        {
            try
            {
                string query = "SELECT * FROM OrderItems WHERE OrderId = @orderId";
                SqlParameter[] parameters = {
                    new SqlParameter("@orderId", orderId)
                };
                DataTable dt = db.ExecuteQuery(query, parameters);
                return DataTableToOrderItemList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve order items: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Update order status
        /// </summary>
        public bool UpdateOrderStatus(int orderId, string status)
        {
            try
            {
                string query = "UPDATE Orders SET Status = @status WHERE OrderId = @orderId";
                SqlParameter[] parameters = {
                    new SqlParameter("@status", status),
                    new SqlParameter("@orderId", orderId)
                };
                int result = db.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update order status: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get orders by status
        /// </summary>
        public List<Order> GetOrdersByStatus(string status)
        {
            try
            {
                string query = "SELECT * FROM Orders WHERE Status = @status ORDER BY OrderDate DESC";
                SqlParameter[] parameters = {
                    new SqlParameter("@status", status)
                };
                DataTable dt = db.ExecuteQuery(query, parameters);
                return DataTableToOrderList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve orders by status: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get total sales amount
        /// </summary>
        public decimal GetTotalSales()
        {
            try
            {
                string query = "SELECT ISNULL(SUM(GrandTotal), 0) FROM Orders";
                object result = db.ExecuteScalar(query);
                return Convert.ToDecimal(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to calculate total sales: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get order count
        /// </summary>
        public int GetOrderCount()
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Orders";
                object result = db.ExecuteScalar(query);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get order count: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Multi-field search across OrderId, CustomerName, Status and CustomerId
        /// </summary>
        public List<Order> SearchOrders(string searchTerm)
        {
            try
            {
                string query = @"SELECT * FROM Orders 
                               WHERE CustomerName LIKE @searchTerm 
                               OR CAST(OrderId AS NVARCHAR) LIKE @searchTerm
                               OR Status LIKE @searchTerm
                               OR CAST(CustomerId AS NVARCHAR) LIKE @searchTerm
                               ORDER BY OrderDate DESC";
                SqlParameter[] parameters = {
                    new SqlParameter("@searchTerm", $"%{searchTerm}%")
                };
                DataTable dt = db.ExecuteQuery(query, parameters);
                return DataTableToOrderList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception($"Order search failed: {ex.Message}", ex);
            }
        }

        // Helper methods
        private List<Order> DataTableToOrderList(DataTable dt)
        {
            List<Order> orders = new List<Order>();
            foreach (DataRow row in dt.Rows)
            {
                orders.Add(DataRowToOrder(row));
            }
            return orders;
        }

        private Order DataRowToOrder(DataRow row)
        {
            return new Order
            {
                OrderId = Convert.ToInt32(row["OrderId"]),
                CustomerId = Convert.ToInt32(row["CustomerId"]),
                CustomerName = row["CustomerName"].ToString(),
                OrderDate = Convert.ToDateTime(row["OrderDate"]),
                Status = row["Status"].ToString(),
                Subtotal = Convert.ToDecimal(row["Subtotal"]),
                DiscountTotal = Convert.ToDecimal(row["DiscountTotal"]),
                GrandTotal = Convert.ToDecimal(row["GrandTotal"])
            };
        }

        private List<OrderItem> DataTableToOrderItemList(DataTable dt)
        {
            List<OrderItem> items = new List<OrderItem>();
            foreach (DataRow row in dt.Rows)
            {
                items.Add(new OrderItem
                {
                    OrderItemId = Convert.ToInt32(row["OrderItemId"]),
                    OrderId = Convert.ToInt32(row["OrderId"]),
                    ProductId = Convert.ToInt32(row["ProductId"]),
                    ProductName = row["ProductName"].ToString(),
                    Quantity = Convert.ToInt32(row["Quantity"]),
                    UnitPrice = Convert.ToDecimal(row["UnitPrice"]),
                    Discount = Convert.ToDecimal(row["Discount"]),
                    LineTotal = Convert.ToDecimal(row["LineTotal"])
                });
            }
            return items;
        }
    }
}
