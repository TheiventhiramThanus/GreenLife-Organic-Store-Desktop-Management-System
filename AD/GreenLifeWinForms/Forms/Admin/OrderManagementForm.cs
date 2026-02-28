using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GreenLifeWinForms.Models;
using GreenLifeWinForms.Services;

namespace GreenLifeWinForms.Forms.Admin
{
    public partial class OrderManagementForm : Form
    {
        private OrderService orderService;
        private DataGridView dgvOrders;
        private TextBox txtSearch;
        private ComboBox cmbStatus;
        private List<Order> currentOrders;

        public OrderManagementForm()
        {
            orderService = new OrderService();
            currentOrders = new List<Order>();
            InitializeComponent();
            LoadOrders();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Text = "Order Management - GreenLife";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 253, 244);
            
            // Title Label
            Label lblTitle = new Label();
            lblTitle.Text = "📋 Order Management";
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
            txtSearch.Size = new Size(250, 25);
            txtSearch.Location = new Point(70, 17);
            txtSearch.TextChanged += TxtSearch_TextChanged;
            searchPanel.Controls.Add(txtSearch);
            
            // Status Label
            Label lblStatus = new Label();
            lblStatus.Text = "Status:";
            lblStatus.Font = new Font("Segoe UI", 10);
            lblStatus.Location = new Point(340, 20);
            lblStatus.AutoSize = true;
            searchPanel.Controls.Add(lblStatus);
            
            // Status ComboBox
            cmbStatus = new ComboBox();
            cmbStatus.Font = new Font("Segoe UI", 10);
            cmbStatus.Size = new Size(200, 25);
            cmbStatus.Location = new Point(400, 17);
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.Items.AddRange(new string[] { "All", "Pending", "Processing", "Shipped", "Delivered", "Cancelled" });
            cmbStatus.SelectedIndex = 0;
            cmbStatus.SelectedIndexChanged += CmbStatus_SelectedIndexChanged;
            searchPanel.Controls.Add(cmbStatus);
            
            // Refresh Button
            Button btnRefresh = new Button();
            btnRefresh.Text = "🔄 Refresh";
            btnRefresh.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnRefresh.Size = new Size(120, 35);
            btnRefresh.Location = new Point(620, 12);
            btnRefresh.BackColor = Color.FromArgb(59, 130, 246);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.Click += BtnRefresh_Click;
            searchPanel.Controls.Add(btnRefresh);
            
            // DataGridView
            dgvOrders = new DataGridView();
            dgvOrders.Location = new Point(20, 150);
            dgvOrders.Size = new Size(1150, 420);
            dgvOrders.BackgroundColor = Color.White;
            dgvOrders.BorderStyle = BorderStyle.None;
            dgvOrders.AllowUserToAddRows = false;
            dgvOrders.AllowUserToDeleteRows = false;
            dgvOrders.ReadOnly = true;
            dgvOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrders.MultiSelect = false;
            dgvOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrders.RowHeadersVisible = false;
            dgvOrders.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 253, 244);
            dgvOrders.DefaultCellStyle.SelectionBackColor = Color.FromArgb(22, 163, 74);
            dgvOrders.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvOrders.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(22, 163, 74);
            dgvOrders.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvOrders.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvOrders.EnableHeadersVisualStyles = false;
            this.Controls.Add(dgvOrders);
            
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
            
            // Update Status Button
            Button btnUpdateStatus = new Button();
            btnUpdateStatus.Text = "✏️ Update Status";
            btnUpdateStatus.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnUpdateStatus.Size = new Size(160, 45);
            btnUpdateStatus.Location = new Point(180, 5);
            btnUpdateStatus.BackColor = Color.FromArgb(251, 191, 36);
            btnUpdateStatus.ForeColor = Color.FromArgb(20, 83, 45);
            btnUpdateStatus.FlatStyle = FlatStyle.Flat;
            btnUpdateStatus.FlatAppearance.BorderSize = 0;
            btnUpdateStatus.Cursor = Cursors.Hand;
            btnUpdateStatus.Click += BtnUpdateStatus_Click;
            actionPanel.Controls.Add(btnUpdateStatus);
            
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

        private void LoadOrders()
        {
            try
            {
                currentOrders = orderService.GetAllOrders();
                DisplayOrders(currentOrders);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading orders: {ex.Message}", 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        private void DisplayOrders(List<Order> orders)
        {
            dgvOrders.DataSource = null;
            dgvOrders.Columns.Clear();
            
            dgvOrders.DataSource = orders;
            
            // Customize columns
            if (dgvOrders.Columns.Count > 0)
            {
                dgvOrders.Columns["OrderId"].HeaderText = "Order ID";
                dgvOrders.Columns["OrderId"].Width = 80;
                dgvOrders.Columns["CustomerId"].HeaderText = "Customer ID";
                dgvOrders.Columns["CustomerId"].Width = 100;
                dgvOrders.Columns["CustomerName"].HeaderText = "Customer Name";
                dgvOrders.Columns["CustomerName"].Width = 200;
                dgvOrders.Columns["OrderDate"].HeaderText = "Order Date";
                dgvOrders.Columns["OrderDate"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
                dgvOrders.Columns["OrderDate"].Width = 150;
                dgvOrders.Columns["Status"].HeaderText = "Status";
                dgvOrders.Columns["Status"].Width = 100;
                dgvOrders.Columns["Subtotal"].HeaderText = "Subtotal";
                dgvOrders.Columns["Subtotal"].DefaultCellStyle.Format = "'LKR '#,##0.00";
                dgvOrders.Columns["Subtotal"].Width = 100;
                dgvOrders.Columns["DiscountTotal"].HeaderText = "Discount";
                dgvOrders.Columns["DiscountTotal"].DefaultCellStyle.Format = "'LKR '#,##0.00";
                dgvOrders.Columns["DiscountTotal"].Width = 100;
                dgvOrders.Columns["GrandTotal"].HeaderText = "Grand Total";
                dgvOrders.Columns["GrandTotal"].DefaultCellStyle.Format = "'LKR '#,##0.00";
                dgvOrders.Columns["GrandTotal"].Width = 120;
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            SearchOrders();
        }

        private void CmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterByStatus();
        }

        private void SearchOrders()
        {
            try
            {
                string searchTerm = txtSearch.Text.Trim();
                if (string.IsNullOrEmpty(searchTerm))
                {
                    FilterByStatus();
                    return;
                }
                
                List<Order> filteredOrders = orderService.SearchOrders(searchTerm);
                DisplayOrders(filteredOrders);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching orders: {ex.Message}", 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        private void FilterByStatus()
        {
            try
            {
                string status = cmbStatus.SelectedItem?.ToString() ?? "All";
                
                if (status == "All")
                {
                    DisplayOrders(currentOrders);
                }
                else
                {
                    List<Order> filteredOrders = orderService.GetOrdersByStatus(status);
                    DisplayOrders(filteredOrders);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering orders: {ex.Message}", 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            cmbStatus.SelectedIndex = 0;
            LoadOrders();
        }

        private void BtnViewDetails_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an order to view details.", 
                    "No Selection", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                return;
            }

            Order selectedOrder = (Order)dgvOrders.SelectedRows[0].DataBoundItem;
            
            try
            {
                List<OrderItem> orderItems = orderService.GetOrderItems(selectedOrder.OrderId);
                
                string details = $"Order ID: {selectedOrder.OrderId}\n" +
                               $"Customer: {selectedOrder.CustomerName}\n" +
                               $"Order Date: {selectedOrder.OrderDate:yyyy-MM-dd HH:mm}\n" +
                               $"Status: {selectedOrder.Status}\n\n" +
                               $"Order Items:\n" +
                               $"{'=',-50}\n";
                
                foreach (OrderItem item in orderItems)
                {
                    details += $"{item.ProductName}\n" +
                             $"  Qty: {item.Quantity} x LKR {item.UnitPrice:F2} = LKR {item.LineTotal:F2}\n";
                }
                
                details += $"{'=',-50}\n" +
                         $"Subtotal: LKR {selectedOrder.Subtotal:F2}\n" +
                         $"Discount: -LKR {selectedOrder.DiscountTotal:F2}\n" +
                         $"Grand Total: LKR {selectedOrder.GrandTotal:F2}";
                
                MessageBox.Show(details, "Order Details", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading order details: {ex.Message}", 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        private void BtnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an order to update status.", 
                    "No Selection", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                return;
            }

            Order selectedOrder = (Order)dgvOrders.SelectedRows[0].DataBoundItem;
            
            // Show status selection dialog
            Form statusForm = new Form();
            statusForm.Text = "Update Order Status";
            statusForm.Size = new Size(400, 200);
            statusForm.StartPosition = FormStartPosition.CenterParent;
            statusForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            statusForm.MaximizeBox = false;
            statusForm.MinimizeBox = false;
            statusForm.BackColor = Color.FromArgb(240, 253, 244);
            
            Label lblInfo = new Label();
            lblInfo.Text = $"Order ID: {selectedOrder.OrderId}\nCurrent Status: {selectedOrder.Status}";
            lblInfo.Font = new Font("Segoe UI", 11);
            lblInfo.Location = new Point(20, 20);
            lblInfo.AutoSize = true;
            statusForm.Controls.Add(lblInfo);
            
            Label lblNewStatus = new Label();
            lblNewStatus.Text = "New Status:";
            lblNewStatus.Font = new Font("Segoe UI", 11);
            lblNewStatus.Location = new Point(20, 70);
            lblNewStatus.AutoSize = true;
            statusForm.Controls.Add(lblNewStatus);
            
            ComboBox cmbNewStatus = new ComboBox();
            cmbNewStatus.Font = new Font("Segoe UI", 11);
            cmbNewStatus.Size = new Size(250, 30);
            cmbNewStatus.Location = new Point(120, 67);
            cmbNewStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbNewStatus.Items.AddRange(new string[] { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" });
            cmbNewStatus.SelectedItem = selectedOrder.Status;
            statusForm.Controls.Add(cmbNewStatus);
            
            Button btnUpdate = new Button();
            btnUpdate.Text = "Update";
            btnUpdate.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnUpdate.Size = new Size(120, 40);
            btnUpdate.Location = new Point(120, 110);
            btnUpdate.BackColor = Color.FromArgb(22, 163, 74);
            btnUpdate.ForeColor = Color.White;
            btnUpdate.FlatStyle = FlatStyle.Flat;
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.Cursor = Cursors.Hand;
            btnUpdate.Click += (s, ev) => {
                try
                {
                    string newStatus = cmbNewStatus.SelectedItem?.ToString();
                    if (string.IsNullOrEmpty(newStatus))
                    {
                        MessageBox.Show("Please select a status.", "Validation Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    
                    bool success = orderService.UpdateOrderStatus(selectedOrder.OrderId, newStatus);
                    if (success)
                    {
                        MessageBox.Show("Order status updated successfully!", "Success", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        statusForm.DialogResult = DialogResult.OK;
                        statusForm.Close();
                        LoadOrders();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update order status.", "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating status: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            statusForm.Controls.Add(btnUpdate);
            
            Button btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Font = new Font("Segoe UI", 11);
            btnCancel.Size = new Size(120, 40);
            btnCancel.Location = new Point(250, 110);
            btnCancel.BackColor = Color.Gray;
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.Click += (s, ev) => { statusForm.DialogResult = DialogResult.Cancel; statusForm.Close(); };
            statusForm.Controls.Add(btnCancel);
            
            statusForm.ShowDialog();
        }
    }
}
