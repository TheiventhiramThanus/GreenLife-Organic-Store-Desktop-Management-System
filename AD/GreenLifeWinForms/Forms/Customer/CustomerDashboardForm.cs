using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using GreenLifeWinForms.Models;
using GreenLifeWinForms.Services;

namespace GreenLifeWinForms.Forms.Customer
{
    public partial class CustomerDashboardForm : Form
    {
        private Models.Customer currentCustomer;
        private OrderService orderService;
        private Chart chartSpending;

        public CustomerDashboardForm(Models.Customer customer)
        {
            currentCustomer = customer;
            orderService = new OrderService();
            InitializeComponent();
            LoadDashboardChart();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Text = "Customer Dashboard - GreenLife";
            this.Size = new Size(960, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 253, 244);
            
            // Title Label
            Label lblTitle = new Label();
            lblTitle.Text = "🌱 GreenLife Organic Store";
            lblTitle.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(22, 163, 74);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(30, 20);
            this.Controls.Add(lblTitle);
            
            // Welcome Label
            Label lblWelcome = new Label();
            lblWelcome.Text = $"Welcome, {currentCustomer.FullName}!";
            lblWelcome.Font = new Font("Segoe UI", 14);
            lblWelcome.ForeColor = Color.FromArgb(20, 83, 45);
            lblWelcome.AutoSize = true;
            lblWelcome.Location = new Point(30, 65);
            this.Controls.Add(lblWelcome);
            
            // Quick Actions Label
            Label lblActions = new Label();
            lblActions.Text = "What would you like to do today?";
            lblActions.Font = new Font("Segoe UI", 12);
            lblActions.ForeColor = Color.FromArgb(20, 83, 45);
            lblActions.AutoSize = true;
            lblActions.Location = new Point(30, 110);
            this.Controls.Add(lblActions);
            
            // Navigation Buttons
            int btnY = 160;
            int btnSpacing = 70;
            
            CreateNavButton("🛒 Browse Products", 30, btnY, BtnBrowse_Click);
            CreateNavButton("🛍️ View Cart", 30, btnY + btnSpacing, BtnCart_Click);
            CreateNavButton("📦 Track Orders", 30, btnY + btnSpacing * 2, BtnOrders_Click);
            CreateNavButton("⭐ My Reviews", 30, btnY + btnSpacing * 3, BtnReviews_Click);
            CreateNavButton("👤 My Profile", 30, btnY + btnSpacing * 4, BtnProfile_Click);
            
            // ── Spending Overview Chart ─────────────────────────────────────
            Panel chartPanel = new Panel();
            chartPanel.Location = new Point(470, 160);
            chartPanel.Size = new Size(460, 350);
            chartPanel.BackColor = Color.White;
            this.Controls.Add(chartPanel);

            Label lblChartTitle = new Label();
            lblChartTitle.Text = "📈 My Spending Overview";
            lblChartTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblChartTitle.ForeColor = Color.FromArgb(22, 163, 74);
            lblChartTitle.AutoSize = true;
            lblChartTitle.Location = new Point(10, 8);
            chartPanel.Controls.Add(lblChartTitle);

            chartSpending = new Chart();
            chartSpending.Location = new Point(5, 35);
            chartSpending.Size = new Size(450, 305);
            chartSpending.BackColor = Color.White;

            ChartArea area = new ChartArea("SpendingArea");
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 8);
            area.AxisX.LabelStyle.Angle = -30;
            area.AxisY.MajorGrid.LineColor = Color.FromArgb(230, 230, 230);
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 8);
            area.AxisY.LabelStyle.Format = "N0";
            chartSpending.ChartAreas.Add(area);

            Series spendSeries = new Series("Spending")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.FromArgb(22, 163, 74),
                XValueType = ChartValueType.String,
                YValueType = ChartValueType.Double
            };
            Series ordersSeries = new Series("Orders")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.FromArgb(234, 179, 8),
                BorderWidth = 3,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 7,
                XValueType = ChartValueType.String,
                YValueType = ChartValueType.Int32
            };
            chartSpending.Series.Add(spendSeries);
            chartSpending.Series.Add(ordersSeries);
            chartPanel.Controls.Add(chartSpending);
            
            // Chatbot Button
            Button btnChatbot = new Button();
            btnChatbot.Text = "🤖 Assistant";
            btnChatbot.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnChatbot.Size = new Size(150, 40);
            btnChatbot.Location = new Point(600, 670);
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
            btnLogout.Location = new Point(770, 670);
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

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            BrowseProductsForm browseForm = new BrowseProductsForm(currentCustomer);
            browseForm.ShowDialog();
        }

        private void BtnCart_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Shopping Cart form will be implemented next.", 
                "Coming Soon", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);
        }

        private void BtnOrders_Click(object sender, EventArgs e)
        {
            TrackOrdersForm ordersForm = new TrackOrdersForm(currentCustomer);
            ordersForm.ShowDialog();
        }

        private void BtnReviews_Click(object sender, EventArgs e)
        {
            MyReviewsForm reviewsForm = new MyReviewsForm(currentCustomer);
            reviewsForm.ShowDialog();
        }

        private void BtnProfile_Click(object sender, EventArgs e)
        {
            MessageBox.Show("My Profile form will be implemented next.", 
                "Coming Soon", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);
        }

        private void LoadDashboardChart()
        {
            try
            {
                chartSpending.Series["Spending"].Points.Clear();
                chartSpending.Series["Orders"].Points.Clear();

                List<Order> myOrders = orderService.GetOrdersByCustomerId(currentCustomer.CustomerId);

                // Group by last 6 months
                for (int i = 5; i >= 0; i--)
                {
                    DateTime month = DateTime.Today.AddMonths(-i);
                    string label = month.ToString("MMM yyyy");
                    var monthOrders = myOrders.Where(o =>
                        o.OrderDate.Year == month.Year && o.OrderDate.Month == month.Month).ToList();
                    double spent = (double)monthOrders.Sum(o => o.GrandTotal);
                    int count = monthOrders.Count;

                    chartSpending.Series["Spending"].Points.AddXY(label, spent);
                    chartSpending.Series["Orders"].Points.AddXY(label, count);
                }
            }
            catch { }
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
