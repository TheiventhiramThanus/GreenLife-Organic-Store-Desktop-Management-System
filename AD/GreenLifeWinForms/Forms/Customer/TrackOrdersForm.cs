using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GreenLifeWinForms.Models;
using GreenLifeWinForms.Services;

namespace GreenLifeWinForms.Forms.Customer
{
    public partial class TrackOrdersForm : Form
    {
        private OrderService orderService;
        private Models.Customer currentCustomer;
        private DataGridView dgvOrders;
        private List<Order> customerOrders;

        public TrackOrdersForm(Models.Customer customer)
        {
            currentCustomer = customer;
            orderService = new OrderService();
            customerOrders = new List<Order>();
            InitializeComponent();
            LoadOrders();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Text = "Track Orders - GreenLife";
            this.Size = new Size(1000, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 253, 244);
            
            // Title Label
            Label lblTitle = new Label();
            lblTitle.Text = "📦 Track Your Orders";
            lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(22, 163, 74);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(30, 20);
            this.Controls.Add(lblTitle);
            
            // Customer Info
            Label lblCustomer = new Label();
            lblCustomer.Text = $"Customer: {currentCustomer.FullName}";
            lblCustomer.Font = new Font("Segoe UI", 12);
            lblCustomer.ForeColor = Color.FromArgb(20, 83, 45);
            lblCustomer.AutoSize = true;
            lblCustomer.Location = new Point(30, 60);
            this.Controls.Add(lblCustomer);
            
            // DataGridView
            dgvOrders = new DataGridView();
            dgvOrders.Location = new Point(30, 110);
            dgvOrders.Size = new Size(930, 420);
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
            
            // Buttons Panel
            Panel buttonPanel = new Panel();
            buttonPanel.Location = new Point(30, 550);
            buttonPanel.Size = new Size(930, 50);
            this.Controls.Add(buttonPanel);
            
            // View Details Button
            Button btnViewDetails = new Button();
            btnViewDetails.Text = "👁️ View Details";
            btnViewDetails.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnViewDetails.Size = new Size(180, 45);
            btnViewDetails.Location = new Point(0, 0);
            btnViewDetails.BackColor = Color.FromArgb(59, 130, 246);
            btnViewDetails.ForeColor = Color.White;
            btnViewDetails.FlatStyle = FlatStyle.Flat;
            btnViewDetails.FlatAppearance.BorderSize = 0;
            btnViewDetails.Cursor = Cursors.Hand;
            btnViewDetails.Click += BtnViewDetails_Click;
            buttonPanel.Controls.Add(btnViewDetails);
            
            // Refresh Button
            Button btnRefresh = new Button();
            btnRefresh.Text = "🔄 Refresh";
            btnRefresh.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnRefresh.Size = new Size(150, 45);
            btnRefresh.Location = new Point(200, 0);
            btnRefresh.BackColor = Color.FromArgb(22, 163, 74);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.Click += BtnRefresh_Click;
            buttonPanel.Controls.Add(btnRefresh);
            
            // Close Button
            Button btnClose = new Button();
            btnClose.Text = "Close";
            btnClose.Font = new Font("Segoe UI", 11);
            btnClose.Size = new Size(120, 45);
            btnClose.Location = new Point(800, 0);
            btnClose.BackColor = Color.Gray;
            btnClose.ForeColor = Color.White;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Cursor = Cursors.Hand;
            btnClose.Click += (s, e) => this.Close();
            buttonPanel.Controls.Add(btnClose);
            
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void LoadOrders()
        {
            try
            {
                customerOrders = orderService.GetOrdersByCustomerId(currentCustomer.CustomerId);
                DisplayOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading orders: {ex.Message}", 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        private void DisplayOrders()
        {
            dgvOrders.DataSource = null;
            dgvOrders.Columns.Clear();
            
            if (customerOrders.Count == 0)
            {
                MessageBox.Show("You haven't placed any orders yet.\n\nStart shopping to see your orders here!", 
                    "No Orders", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
                return;
            }
            
            dgvOrders.DataSource = customerOrders;
            
            // Customize columns
            if (dgvOrders.Columns.Count > 0)
            {
                dgvOrders.Columns["OrderId"].HeaderText = "Order ID";
                dgvOrders.Columns["OrderId"].Width = 80;
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
                dgvOrders.Columns["GrandTotal"].HeaderText = "Total";
                dgvOrders.Columns["GrandTotal"].DefaultCellStyle.Format = "'LKR '#,##0.00";
                dgvOrders.Columns["GrandTotal"].Width = 100;
                
                // Hide unnecessary columns
                dgvOrders.Columns["CustomerId"].Visible = false;
                dgvOrders.Columns["CustomerName"].Visible = false;
            }
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
                
                // Create a detailed view form
                Form detailsForm = new Form();
                detailsForm.Text = $"Order #{selectedOrder.OrderId} Details";
                detailsForm.Size = new Size(700, 600);
                detailsForm.StartPosition = FormStartPosition.CenterParent;
                detailsForm.BackColor = Color.FromArgb(240, 253, 244);
                
                // Order Info Panel
                Panel infoPanel = new Panel();
                infoPanel.Location = new Point(20, 20);
                infoPanel.Size = new Size(640, 120);
                infoPanel.BackColor = Color.White;
                detailsForm.Controls.Add(infoPanel);
                
                Label lblOrderInfo = new Label();
                lblOrderInfo.Text = $"Order ID: {selectedOrder.OrderId}\n" +
                                   $"Order Date: {selectedOrder.OrderDate:yyyy-MM-dd HH:mm}\n" +
                                   $"Status: {selectedOrder.Status}\n" +
                                   $"Customer: {selectedOrder.CustomerName}";
                lblOrderInfo.Font = new Font("Segoe UI", 11);
                lblOrderInfo.Location = new Point(15, 15);
                lblOrderInfo.AutoSize = true;
                infoPanel.Controls.Add(lblOrderInfo);
                
                // Order Items DataGridView
                DataGridView dgvItems = new DataGridView();
                dgvItems.Location = new Point(20, 160);
                dgvItems.Size = new Size(640, 280);
                dgvItems.BackgroundColor = Color.White;
                dgvItems.BorderStyle = BorderStyle.None;
                dgvItems.AllowUserToAddRows = false;
                dgvItems.AllowUserToDeleteRows = false;
                dgvItems.ReadOnly = true;
                dgvItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvItems.RowHeadersVisible = false;
                dgvItems.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(22, 163, 74);
                dgvItems.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvItems.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                dgvItems.EnableHeadersVisualStyles = false;
                detailsForm.Controls.Add(dgvItems);
                
                dgvItems.DataSource = orderItems;
                
                if (dgvItems.Columns.Count > 0)
                {
                    dgvItems.Columns["OrderItemId"].Visible = false;
                    dgvItems.Columns["OrderId"].Visible = false;
                    dgvItems.Columns["ProductId"].Visible = false;
                    dgvItems.Columns["ProductName"].HeaderText = "Product";
                    dgvItems.Columns["Quantity"].HeaderText = "Qty";
                    dgvItems.Columns["Quantity"].Width = 60;
                    dgvItems.Columns["UnitPrice"].HeaderText = "Unit Price";
                    dgvItems.Columns["UnitPrice"].DefaultCellStyle.Format = "'LKR '#,##0.00";
                    dgvItems.Columns["Discount"].HeaderText = "Discount %";
                    dgvItems.Columns["Discount"].Width = 80;
                    dgvItems.Columns["LineTotal"].HeaderText = "Total";
                    dgvItems.Columns["LineTotal"].DefaultCellStyle.Format = "'LKR '#,##0.00";
                }
                
                // Totals Panel
                Panel totalsPanel = new Panel();
                totalsPanel.Location = new Point(20, 460);
                totalsPanel.Size = new Size(640, 80);
                totalsPanel.BackColor = Color.White;
                detailsForm.Controls.Add(totalsPanel);
                
                Label lblTotals = new Label();
                lblTotals.Text = $"Subtotal: LKR {selectedOrder.Subtotal:F2}\n" +
                               $"Discount: -LKR {selectedOrder.DiscountTotal:F2}\n" +
                               $"Grand Total: LKR {selectedOrder.GrandTotal:F2}";
                lblTotals.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                lblTotals.ForeColor = Color.FromArgb(22, 163, 74);
                lblTotals.Location = new Point(400, 10);
                lblTotals.AutoSize = true;
                totalsPanel.Controls.Add(lblTotals);
                
                // Close Button
                Button btnCloseDetails = new Button();
                btnCloseDetails.Text = "Close";
                btnCloseDetails.Font = new Font("Segoe UI", 11);
                btnCloseDetails.Size = new Size(120, 40);
                btnCloseDetails.Location = new Point(540, 550);
                btnCloseDetails.BackColor = Color.Gray;
                btnCloseDetails.ForeColor = Color.White;
                btnCloseDetails.FlatStyle = FlatStyle.Flat;
                btnCloseDetails.FlatAppearance.BorderSize = 0;
                btnCloseDetails.Cursor = Cursors.Hand;
                btnCloseDetails.Click += (s, ev) => detailsForm.Close();
                detailsForm.Controls.Add(btnCloseDetails);
                
                detailsForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading order details: {ex.Message}", 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadOrders();
        }
    }
}
