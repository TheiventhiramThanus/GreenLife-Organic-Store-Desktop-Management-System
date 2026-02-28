using System;
using System.Windows.Forms;

namespace GreenLifeWinForms
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Initialize database on startup
            try
            {
                var db = Database.DatabaseManager.Instance;
                
                // Test connection
                if (db.TestConnection())
                {
                    MessageBox.Show("Database connected successfully!\n\nDatabase: GreenLifeDB\nServer: MSI\\SQLEXPRESS", 
                        "Connection Success", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to connect to database.\n\nPlease check your SQL Server connection.", 
                        "Connection Failed", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database initialization error:\n\n{ex.Message}\n\nPlease ensure SQL Server Express is running.", 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
                return;
            }
            
            // Start the application with Role Selection Form
            Application.Run(new Forms.Common.RoleSelectionForm());
        }
    }
}
