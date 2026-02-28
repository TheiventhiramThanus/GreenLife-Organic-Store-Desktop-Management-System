using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GreenLifeWinForms.Models;
using GreenLifeWinForms.Services;

namespace GreenLifeWinForms.Forms.Admin
{
    public partial class CustomerManagementForm : Form
    {
        private CustomerService customerService;
        private OrderService orderService;
        private DataGridView dgvCustomers;
        private TextBox txtSearch;
        private List<GreenLifeWinForms.Models.Customer> currentCustomers;

        public CustomerManagementForm()
        {
            customerService = new CustomerService();
            orderService = new OrderService();
            currentCustomers = new List<GreenLifeWinForms.Models.Customer>();
            InitializeComponent();
            LoadCustomers();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Text = "Customer Management - GreenLife";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 253, 244);
            
            // Title Label
            Label lblTitle = new Label();
            lblTitle.Text = "👥 Customer Management";
            lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(22, 163, 74);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(20, 20);
            this.Controls.Add(lblTitle);
            
            // Search Panel
            Panel searchPanel = new Panel();
            searchPanel.Location = new Point(20, 70);
            searchPanel.Size = new Size(1150, 60);
            searchPanel.BackColor = Color.White;
            this.Controls.Add(searchPanel);
            
            // Search Label
            Label lblSearch = new Label();
            lblSearch.Text = "Search:";
            lblSearch.Font = new Font("Segoe UI", 10);
            lblSearch.Location = new Point(10, 20);
            lblSearch.AutoSize = true;
            searchPanel.Controls.Add(lblSearch);
            
            // Search TextBox
            txtSearch = new TextBox();
            txtSearch.Font = new Font("Segoe UI", 10);
            txtSearch.Size = new Size(300, 25);
            txtSearch.Location = new Point(70, 17);
            txtSearch.TextChanged += TxtSearch_TextChanged;
            searchPanel.Controls.Add(txtSearch);
            
            // Refresh Button
            Button btnRefresh = new Button();
            btnRefresh.Text = "🔄 Refresh";
            btnRefresh.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnRefresh.Size = new Size(120, 35);
            btnRefresh.Location = new Point(390, 12);
            btnRefresh.BackColor = Color.FromArgb(59, 130, 246);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.Click += BtnRefresh_Click;
            searchPanel.Controls.Add(btnRefresh);
            
            // DataGridView
            dgvCustomers = new DataGridView();
            dgvCustomers.Location = new Point(20, 150);
            dgvCustomers.Size = new Size(1150, 420);
            dgvCustomers.BackgroundColor = Color.White;
            dgvCustomers.BorderStyle = BorderStyle.None;
            dgvCustomers.AllowUserToAddRows = false;
            dgvCustomers.AllowUserToDeleteRows = false;
            dgvCustomers.ReadOnly = true;
            dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCustomers.MultiSelect = false;
            dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCustomers.RowHeadersVisible = false;
            dgvCustomers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 253, 244);
            dgvCustomers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(22, 163, 74);
            dgvCustomers.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvCustomers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(22, 163, 74);
            dgvCustomers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCustomers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvCustomers.EnableHeadersVisualStyles = false;
            this.Controls.Add(dgvCustomers);
            
            // Action Buttons Panel
            Panel actionPanel = new Panel();
            actionPanel.Location = new Point(20, 590);
            actionPanel.Size = new Size(1150, 60);
            this.Controls.Add(actionPanel);
            
            // View Details Button
            Button btnViewDetails = new Button();
            btnViewDetails.Text = "👁️ View Details";
            btnViewDetails.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnViewDetails.Size = new Size(160, 45);
            btnViewDetails.Location = new Point(0, 5);
            btnViewDetails.BackColor = Color.FromArgb(59, 130, 246);
            btnViewDetails.ForeColor = Color.White;
            btnViewDetails.FlatStyle = FlatStyle.Flat;
            btnViewDetails.FlatAppearance.BorderSize = 0;
            btnViewDetails.Cursor = Cursors.Hand;
            btnViewDetails.Click += BtnViewDetails_Click;
            actionPanel.Controls.Add(btnViewDetails);
            
            // View Orders Button
            Button btnViewOrders = new Button();
            btnViewOrders.Text = "📦 View Orders";
            btnViewOrders.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnViewOrders.Size = new Size(160, 45);
            btnViewOrders.Location = new Point(180, 5);
            btnViewOrders.BackColor = Color.FromArgb(22, 163, 74);
            btnViewOrders.ForeColor = Color.White;
            btnViewOrders.FlatStyle = FlatStyle.Flat;
            btnViewOrders.FlatAppearance.BorderSize = 0;
            btnViewOrders.Cursor = Cursors.Hand;
            btnViewOrders.Click += BtnViewOrders_Click;
            actionPanel.Controls.Add(btnViewOrders);
            
            // Deactivate Button
            Button btnDeactivate = new Button();
            btnDeactivate.Text = "🚫 Deactivate";
            btnDeactivate.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnDeactivate.Size = new Size(160, 45);
            btnDeactivate.Location = new Point(360, 5);
            btnDeactivate.BackColor = Color.FromArgb(239, 68, 68);
            btnDeactivate.ForeColor = Color.White;
            btnDeactivate.FlatStyle = FlatStyle.Flat;
            btnDeactivate.FlatAppearance.BorderSize = 0;
            btnDeactivate.Cursor = Cursors.Hand;
            btnDeactivate.Click += BtnDeactivate_Click;
            actionPanel.Controls.Add(btnDeactivate);
            
            // Activate Button
            Button btnActivate = new Button();
            btnActivate.Text = "✅ Activate";
            btnActivate.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnActivate.Size = new Size(160, 45);
            btnActivate.Location = new Point(540, 5);
            btnActivate.BackColor = Color.FromArgb(34, 197, 94);
            btnActivate.ForeColor = Color.White;
            btnActivate.FlatStyle = FlatStyle.Flat;
            btnActivate.FlatAppearance.BorderSize = 0;
            btnActivate.Cursor = Cursors.Hand;
            btnActivate.Click += BtnActivate_Click;
            actionPanel.Controls.Add(btnActivate);
            
            // Close Button
            Button btnClose = new Button();
            btnClose.Text = "Close";
            btnClose.Font = new Font("Segoe UI", 11);
            btnClose.Size = new Size(120, 45);
            btnClose.Location = new Point(1020, 5);
            btnClose.BackColor = Color.Gray;
            btnClose.ForeColor = Color.White;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Cursor = Cursors.Hand;
            btnClose.Click += (s, e) => this.Close();
            actionPanel.Controls.Add(btnClose);
            
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void LoadCustomers()
        {
            try
            {
                currentCustomers = customerService.GetAllCustomers();
                DisplayCustomers(currentCustomers);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customers: {ex.Message}", 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        private void DisplayCustomers(List<GreenLifeWinForms.Models.Customer> customers)
        {
            dgvCustomers.DataSource = null;
            dgvCustomers.Columns.Clear();
            
            dgvCustomers.DataSource = customers;
            
            // Customize columns
            if (dgvCustomers.Columns.Count > 0)
            {
                dgvCustomers.Columns["CustomerId"].HeaderText = "ID";
                dgvCustomers.Columns["CustomerId"].Width = 60;
                dgvCustomers.Columns["FullName"].HeaderText = "Full Name";
                dgvCustomers.Columns["FullName"].Width = 200;
                dgvCustomers.Columns["Email"].HeaderText = "Email";
                dgvCustomers.Columns["Email"].Width = 250;
                dgvCustomers.Columns["Phone"].HeaderText = "Phone";
                dgvCustomers.Columns["Phone"].Width = 130;
                dgvCustomers.Columns["Address"].HeaderText = "Address";
                dgvCustomers.Columns["Address"].Width = 250;
                dgvCustomers.Columns["IsActive"].HeaderText = "Active";
                dgvCustomers.Columns["IsActive"].Width = 70;
                dgvCustomers.Columns["RegisteredDate"].HeaderText = "Registered";
                dgvCustomers.Columns["RegisteredDate"].DefaultCellStyle.Format = "yyyy-MM-dd";
                dgvCustomers.Columns["RegisteredDate"].Width = 120;
                
                // Hide password column
                dgvCustomers.Columns["Password"].Visible = false;
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            SearchCustomers();
        }

        private void SearchCustomers()
        {
            try
            {
                string searchTerm = txtSearch.Text.Trim();
                if (string.IsNullOrEmpty(searchTerm))
                {
                    DisplayCustomers(currentCustomers);
                    return;
                }
                
                List<GreenLifeWinForms.Models.Customer> filteredCustomers = customerService.SearchCustomers(searchTerm);
                DisplayCustomers(filteredCustomers);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching customers: {ex.Message}", 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadCustomers();
        }

        private void BtnViewDetails_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a customer to view details.", 
                    "No Selection", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                return;
            }

            GreenLifeWinForms.Models.Customer selectedCustomer = (GreenLifeWinForms.Models.Customer)dgvCustomers.SelectedRows[0].DataBoundItem;
            
            string details = $"Customer ID: {selectedCustomer.CustomerId}\n" +
                           $"Full Name: {selectedCustomer.FullName}\n" +
                           $"Email: {selectedCustomer.Email}\n" +
                           $"Phone: {selectedCustomer.Phone}\n" +
                           $"Address: {selectedCustomer.Address}\n" +
                           $"Status: {(selectedCustomer.IsActive ? "Active" : "Inactive")}\n" +
                           $"Registered: {selectedCustomer.RegisteredDate:yyyy-MM-dd}";
            
            MessageBox.Show(details, "Customer Details", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);
        }

        private void BtnViewOrders_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a customer to view orders.", 
                    "No Selection", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                return;
            }

            GreenLifeWinForms.Models.Customer selectedCustomer = (GreenLifeWinForms.Models.Customer)dgvCustomers.SelectedRows[0].DataBoundItem;
            
            try
            {
                List<Order> customerOrders = orderService.GetOrdersByCustomerId(selectedCustomer.CustomerId);
                
                if (customerOrders.Count == 0)
                {
                    MessageBox.Show($"{selectedCustomer.FullName} has no orders yet.", 
                        "No Orders", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
                    return;
                }
                
                string orderList = $"Orders for {selectedCustomer.FullName}:\n\n";
                foreach (Order order in customerOrders)
                {
                    orderList += $"Order #{order.OrderId} - {order.OrderDate:yyyy-MM-dd} - {order.Status} - LKR {order.GrandTotal:F2}\n";
                }
                
                MessageBox.Show(orderList, "Customer Orders", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customer orders: {ex.Message}", 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        private void BtnDeactivate_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a customer to deactivate.", 
                    "No Selection", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                return;
            }

            GreenLifeWinForms.Models.Customer selectedCustomer = (GreenLifeWinForms.Models.Customer)dgvCustomers.SelectedRows[0].DataBoundItem;
            
            if (!selectedCustomer.IsActive)
            {
                MessageBox.Show("This customer is already inactive.", 
                    "Already Inactive", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
                return;
            }
            
            DialogResult result = MessageBox.Show(
                $"Are you sure you want to deactivate '{selectedCustomer.FullName}'?\n\nThey will not be able to login.", 
                "Confirm Deactivate", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    bool success = customerService.DeactivateCustomer(selectedCustomer.CustomerId);
                    if (success)
                    {
                        MessageBox.Show("Customer deactivated successfully!", 
                            "Success", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Information);
                        LoadCustomers();
                    }
                    else
                    {
                        MessageBox.Show("Failed to deactivate customer.", 
                            "Error", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deactivating customer: {ex.Message}", 
                        "Error", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
                }
            }
        }

        private void BtnActivate_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a customer to activate.", 
                    "No Selection", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                return;
            }

            GreenLifeWinForms.Models.Customer selectedCustomer = (GreenLifeWinForms.Models.Customer)dgvCustomers.SelectedRows[0].DataBoundItem;
            
            if (selectedCustomer.IsActive)
            {
                MessageBox.Show("This customer is already active.", 
                    "Already Active", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
                return;
            }
            
            DialogResult result = MessageBox.Show(
                $"Are you sure you want to activate '{selectedCustomer.FullName}'?", 
                "Confirm Activate", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    bool success = customerService.ActivateCustomer(selectedCustomer.CustomerId);
                    if (success)
                    {
                        MessageBox.Show("Customer activated successfully!", 
                            "Success", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Information);
                        LoadCustomers();
                    }
                    else
                    {
                        MessageBox.Show("Failed to activate customer.", 
                            "Error", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error activating customer: {ex.Message}", 
                        "Error", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
                }
            }
        }
    }
}
