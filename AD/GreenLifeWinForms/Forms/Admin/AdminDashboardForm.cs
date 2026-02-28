using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using GreenLifeWinForms.Models;
using GreenLifeWinForms.Services;

namespace GreenLifeWinForms.Forms.Admin
{
    public partial class AdminDashboardForm : Form
    {
        private Models.Admin currentAdmin;
        private ProductService productService;
        private OrderService orderService;
        private CustomerService customerService;
        private Chart chartSales;

        public AdminDashboardForm(Models.Admin admin)
        {
            currentAdmin = admin;
            productService = new ProductService();
            orderService = new OrderService();
            customerService = new CustomerService();
            InitializeComponent();
            LoadDashboardData();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Text = "Admin Dashboard - GreenLife";
            this.Size = new Size(960, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 253, 244);
            
            // Title Label
            Label lblTitle = new Label();
            lblTitle.Text = "🌱 Admin Dashboard";
            lblTitle.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(22, 163, 74);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(30, 20);
            this.Controls.Add(lblTitle);
            
            // Welcome Label
            Label lblWelcome = new Label();
            lblWelcome.Text = $"Welcome, {currentAdmin.FullName}";
            lblWelcome.Font = new Font("Segoe UI", 12);
            lblWelcome.ForeColor = Color.FromArgb(20, 83, 45);
            lblWelcome.AutoSize = true;
            lblWelcome.Location = new Point(30, 65);
            this.Controls.Add(lblWelcome);
            
            // Metrics Panel
            Panel metricsPanel = new Panel();
            metricsPanel.Location = new Point(30, 110);
            metricsPanel.Size = new Size(830, 120);
            metricsPanel.BackColor = Color.White;
            this.Controls.Add(metricsPanel);
            
            // Total Products Card
            CreateMetricCard(metricsPanel, "Total Products", "0", 10, 10, Color.FromArgb(22, 163, 74));
            
            // Low Stock Card
            CreateMetricCard(metricsPanel, "Low Stock", "0", 220, 10, Color.FromArgb(239, 68, 68));
            
            // Total Customers Card
            CreateMetricCard(metricsPanel, "Total Customers", "0", 430, 10, Color.FromArgb(59, 130, 246));
            
            // Total Orders Card
            CreateMetricCard(metricsPanel, "Total Orders", "0", 640, 10, Color.FromArgb(251, 191, 36));
            
            // Navigation Buttons
            int btnY = 260;
            int btnSpacing = 70;
            
            CreateNavButton("📦 Product Management", 30, btnY, BtnProducts_Click);
            CreateNavButton("👥 Customer Management", 30, btnY + btnSpacing, BtnCustomers_Click);
            CreateNavButton("📋 Order Management", 30, btnY + btnSpacing * 2, BtnOrders_Click);
            CreateNavButton("📊 Reports", 30, btnY + btnSpacing * 3, BtnReports_Click);
            
            // ── Sales Overview Chart ──────────────────────────────────────────
            Panel chartPanel = new Panel();
            chartPanel.Location = new Point(470, 260);
            chartPanel.Size = new Size(460, 280);
            chartPanel.BackColor = Color.White;
            this.Controls.Add(chartPanel);

            Label lblChartTitle = new Label();
            lblChartTitle.Text = "📈 Sales Overview (Last 7 Days)";
            lblChartTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblChartTitle.ForeColor = Color.FromArgb(22, 163, 74);
            lblChartTitle.AutoSize = true;
            lblChartTitle.Location = new Point(10, 8);
            chartPanel.Controls.Add(lblChartTitle);

            chartSales = new Chart();
            chartSales.Location = new Point(5, 35);
            chartSales.Size = new Size(450, 235);
            chartSales.BackColor = Color.White;
            ChartArea area = new ChartArea("SalesArea");
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 8);
            area.AxisX.LabelStyle.Angle = -30;
            area.AxisY.MajorGrid.LineColor = Color.FromArgb(230, 230, 230);
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 8);
            area.AxisY.LabelStyle.Format = "N0";
            chartSales.ChartAreas.Add(area);

            Series revenueSeries = new Series("Revenue")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.FromArgb(22, 163, 74),
                XValueType = ChartValueType.String,
                YValueType = ChartValueType.Double
            };
            Series orderCountSeries = new Series("Orders")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.FromArgb(234, 179, 8),
                BorderWidth = 3,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 7,
                XValueType = ChartValueType.String,
                YValueType = ChartValueType.Int32
            };
            chartSales.Series.Add(revenueSeries);
            chartSales.Series.Add(orderCountSeries);
            chartPanel.Controls.Add(chartSales);
            
            // Chatbot Button
            Button btnChatbot = new Button();
            btnChatbot.Text = "🤖 Assistant";
            btnChatbot.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnChatbot.Size = new Size(150, 40);
            btnChatbot.Location = new Point(600, 720);
            btnChatbot.BackColor = Color.FromArgb(59, 130, 246);
            btnChatbot.ForeColor = Color.White;
            btnChatbot.FlatStyle = FlatStyle.Flat;
            btnChatbot.FlatAppearance.BorderSize = 0;
            btnChatbot.Cursor = Cursors.Hand;
            btnChatbot.Click += (s, ev) => { new Common.ChatbotForm().ShowDialog(); };
            this.Controls.Add(btnChatbot);

            // Logout Button
            Button btnLogout = new Button();
            btnLogout.Text = "Logout";
            btnLogout.Font = new Font("Segoe UI", 11);
            btnLogout.Size = new Size(150, 40);
            btnLogout.Location = new Point(770, 720);
            btnLogout.BackColor = Color.FromArgb(239, 68, 68);
            btnLogout.ForeColor = Color.White;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.Click += BtnLogout_Click;
            this.Controls.Add(btnLogout);
            
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void CreateMetricCard(Panel parent, string title, string value, int x, int y, Color color)
        {
            Panel card = new Panel();
            card.Location = new Point(x, y);
            card.Size = new Size(180, 100);
            card.BackColor = color;
            parent.Controls.Add(card);
            
            Label lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new Font("Segoe UI", 10);
            lblTitle.ForeColor = Color.White;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(10, 10);
            lblTitle.Tag = title; // Store title for later update
            card.Controls.Add(lblTitle);
            
            Label lblValue = new Label();
            lblValue.Text = value;
            lblValue.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            lblValue.ForeColor = Color.White;
            lblValue.AutoSize = true;
            lblValue.Location = new Point(10, 40);
            lblValue.Name = "lblValue_" + title.Replace(" ", "");
            card.Controls.Add(lblValue);
        }

        private void CreateNavButton(string text, int x, int y, EventHandler clickHandler)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btn.Size = new Size(400, 50);
            btn.Location = new Point(x, y);
            btn.BackColor = Color.FromArgb(22, 163, 74);
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            btn.Click += clickHandler;
            this.Controls.Add(btn);
        }

        private void LoadDashboardData()
        {
            try
            {
                // Get total products
                var products = productService.GetAllProducts();
                UpdateMetricValue("TotalProducts", products.Count.ToString());
                
                // Get low stock products
                var lowStock = productService.GetLowStockProducts();
                UpdateMetricValue("LowStock", lowStock.Count.ToString());
                
                // Get real customer count
                int customerCount = customerService.GetCustomerCount();
                UpdateMetricValue("TotalCustomers", customerCount.ToString());
                
                // Get real order count
                int orderCount = orderService.GetOrderCount();
                UpdateMetricValue("TotalOrders", orderCount.ToString());

                // Load chart data
                LoadSalesChart();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dashboard data: {ex.Message}", 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        private void LoadSalesChart()
        {
            try
            {
                chartSales.Series["Revenue"].Points.Clear();
                chartSales.Series["Orders"].Points.Clear();

                List<Order> allOrders = orderService.GetAllOrders();

                // Group by last 7 days
                DateTime startDate = DateTime.Today.AddDays(-6);
                for (int i = 0; i < 7; i++)
                {
                    DateTime day = startDate.AddDays(i);
                    string label = day.ToString("MM/dd");
                    var dayOrders = allOrders.Where(o => o.OrderDate.Date == day.Date).ToList();
                    double revenue = (double)dayOrders.Sum(o => o.GrandTotal);
                    int count = dayOrders.Count;

                    chartSales.Series["Revenue"].Points.AddXY(label, revenue);
                    chartSales.Series["Orders"].Points.AddXY(label, count);
                }
            }
            catch { }
        }

        private void UpdateMetricValue(string metricName, string value)
        {
            string controlName = "lblValue_" + metricName;
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Panel)
                {
                    foreach (Control panelCtrl in ctrl.Controls)
                    {
                        if (panelCtrl is Panel)
                        {
                            foreach (Control cardCtrl in panelCtrl.Controls)
                            {
                                if (cardCtrl.Name == controlName && cardCtrl is Label)
                                {
                                    ((Label)cardCtrl).Text = value;
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void BtnProducts_Click(object sender, EventArgs e)
        {
            ProductManagementForm productForm = new ProductManagementForm();
            productForm.ShowDialog();
            LoadDashboardData(); // Refresh metrics after closing
        }

        private void BtnCustomers_Click(object sender, EventArgs e)
        {
            CustomerManagementForm customerForm = new CustomerManagementForm();
            customerForm.ShowDialog();
            LoadDashboardData(); // Refresh metrics after closing
        }

        private void BtnOrders_Click(object sender, EventArgs e)
        {
            OrderManagementForm orderForm = new OrderManagementForm();
            orderForm.ShowDialog();
            LoadDashboardData(); // Refresh metrics after closing
        }

        private void BtnReports_Click(object sender, EventArgs e)
        {
            ReportsForm reportsForm = new ReportsForm();
            reportsForm.ShowDialog();
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?", 
                "Confirm Logout", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
