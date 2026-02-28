using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using GreenLifeWinForms.Database;
using GreenLifeWinForms.Models;

namespace GreenLifeWinForms.Services
{
    public class ReviewService
    {
        private DatabaseManager db = DatabaseManager.Instance;

        /// <summary>
        /// Add a new review
        /// </summary>
        public bool AddReview(Review review)
        {
            try
            {
                string query = @"INSERT INTO Reviews (ProductId, CustomerId, CustomerName, Rating, Comment, ReviewDate) 
                               VALUES (@productId, @customerId, @customerName, @rating, @comment, @reviewDate)";

                SqlParameter[] parameters = {
                    new SqlParameter("@productId", review.ProductId),
                    new SqlParameter("@customerId", review.CustomerId),
                    new SqlParameter("@customerName", review.CustomerName),
                    new SqlParameter("@rating", review.Rating),
                    new SqlParameter("@comment", review.Comment ?? (object)DBNull.Value),
                    new SqlParameter("@reviewDate", review.ReviewDate)
                };

                int result = db.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to add review: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get reviews by product ID
        /// </summary>
        public List<Review> GetReviewsByProductId(int productId)
        {
            try
            {
                string query = "SELECT * FROM Reviews WHERE ProductId = @productId ORDER BY ReviewDate DESC";
                SqlParameter[] parameters = {
                    new SqlParameter("@productId", productId)
                };
                DataTable dt = db.ExecuteQuery(query, parameters);
                return DataTableToReviewList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve product reviews: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get reviews by customer ID
        /// </summary>
        public List<Review> GetReviewsByCustomerId(int customerId)
        {
            try
            {
                string query = "SELECT * FROM Reviews WHERE CustomerId = @customerId ORDER BY ReviewDate DESC";
                SqlParameter[] parameters = {
                    new SqlParameter("@customerId", customerId)
                };
                DataTable dt = db.ExecuteQuery(query, parameters);
                return DataTableToReviewList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve customer reviews: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get average rating for a product
        /// </summary>
        public decimal GetAverageRating(int productId)
        {
            try
            {
                string query = "SELECT ISNULL(AVG(CAST(Rating AS DECIMAL(10,2))), 0) FROM Reviews WHERE ProductId = @productId";
                SqlParameter[] parameters = {
                    new SqlParameter("@productId", productId)
                };
                object result = db.ExecuteScalar(query, parameters);
                return Convert.ToDecimal(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to calculate average rating: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get review count for a product
        /// </summary>
        public int GetReviewCount(int productId)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Reviews WHERE ProductId = @productId";
                SqlParameter[] parameters = {
                    new SqlParameter("@productId", productId)
                };
                object result = db.ExecuteScalar(query, parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get review count: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Update a review
        /// </summary>
        public bool UpdateReview(Review review)
        {
            try
            {
                string query = @"UPDATE Reviews 
                               SET Rating = @rating, 
                                   Comment = @comment
                               WHERE ReviewId = @reviewId";

                SqlParameter[] parameters = {
                    new SqlParameter("@rating", review.Rating),
                    new SqlParameter("@comment", review.Comment ?? (object)DBNull.Value),
                    new SqlParameter("@reviewId", review.ReviewId)
                };

                int result = db.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update review: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Delete a review
        /// </summary>
        public bool DeleteReview(int reviewId)
        {
            try
            {
                string query = "DELETE FROM Reviews WHERE ReviewId = @reviewId";
                SqlParameter[] parameters = {
                    new SqlParameter("@reviewId", reviewId)
                };
                int result = db.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete review: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Check if customer has already reviewed a product
        /// </summary>
        public bool HasCustomerReviewedProduct(int customerId, int productId)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Reviews WHERE CustomerId = @customerId AND ProductId = @productId";
                SqlParameter[] parameters = {
                    new SqlParameter("@customerId", customerId),
                    new SqlParameter("@productId", productId)
                };
                object result = db.ExecuteScalar(query, parameters);
                return Convert.ToInt32(result) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to check review status: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Multi-field search across CustomerName, Comment, Rating and ReviewId
        /// </summary>
        public List<Review> SearchReviews(string searchTerm)
        {
            try
            {
                string query = @"SELECT * FROM Reviews 
                               WHERE CustomerName LIKE @searchTerm 
                               OR Comment LIKE @searchTerm
                               OR CAST(Rating AS NVARCHAR) LIKE @searchTerm
                               OR CAST(ReviewId AS NVARCHAR) LIKE @searchTerm
                               OR CAST(ProductId AS NVARCHAR) LIKE @searchTerm
                               ORDER BY ReviewDate DESC";
                SqlParameter[] parameters = {
                    new SqlParameter("@searchTerm", $"%{searchTerm}%")
                };
                DataTable dt = db.ExecuteQuery(query, parameters);
                return DataTableToReviewList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception($"Review search failed: {ex.Message}", ex);
            }
        }

        // Helper methods
        private List<Review> DataTableToReviewList(DataTable dt)
        {
            List<Review> reviews = new List<Review>();
            foreach (DataRow row in dt.Rows)
            {
                reviews.Add(new Review
                {
                    ReviewId = Convert.ToInt32(row["ReviewId"]),
                    ProductId = Convert.ToInt32(row["ProductId"]),
                    CustomerId = Convert.ToInt32(row["CustomerId"]),
                    CustomerName = row["CustomerName"].ToString(),
                    Rating = Convert.ToInt32(row["Rating"]),
                    Comment = row["Comment"] != DBNull.Value ? row["Comment"].ToString() : "",
                    ReviewDate = Convert.ToDateTime(row["ReviewDate"])
                });
            }
            return reviews;
        }
    }
}
