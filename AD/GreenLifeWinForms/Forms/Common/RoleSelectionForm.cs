using System;
using System.Drawing;
using System.Windows.Forms;

namespace GreenLifeWinForms.Forms.Common
{
    public partial class RoleSelectionForm : Form
    {
        public RoleSelectionForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Text = "GreenLife Organic Store - Welcome";
            this.Size = new Size(600, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 253, 244); // Light green background
            
            // Title Label
            Label lblTitle = new Label();
            lblTitle.Text = "🌱 GreenLife Organic Store";
            lblTitle.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(22, 163, 74); // Green
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(80, 40);
            this.Controls.Add(lblTitle);
            
            // Subtitle Label
            Label lblSubtitle = new Label();
            lblSubtitle.Text = "Management System";
            lblSubtitle.Font = new Font("Segoe UI", 14);
            lblSubtitle.ForeColor = Color.FromArgb(20, 83, 45); // Dark green
            lblSubtitle.AutoSize = true;
            lblSubtitle.Location = new Point(200, 85);
            this.Controls.Add(lblSubtitle);
            
            // Select Role Label
            Label lblSelectRole = new Label();
            lblSelectRole.Text = "Please select your role:";
            lblSelectRole.Font = new Font("Segoe UI", 12);
            lblSelectRole.ForeColor = Color.FromArgb(20, 83, 45);
            lblSelectRole.AutoSize = true;
            lblSelectRole.Location = new Point(200, 140);
            this.Controls.Add(lblSelectRole);
            
            // Admin Button
            Button btnAdmin = new Button();
            btnAdmin.Text = "👤 Administrator";
            btnAdmin.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            btnAdmin.Size = new Size(300, 60);
            btnAdmin.Location = new Point(150, 190);
            btnAdmin.BackColor = Color.FromArgb(22, 163, 74); // Green
            btnAdmin.ForeColor = Color.White;
            btnAdmin.FlatStyle = FlatStyle.Flat;
            btnAdmin.FlatAppearance.BorderSize = 0;
            btnAdmin.Cursor = Cursors.Hand;
            btnAdmin.Click += BtnAdmin_Click;
            this.Controls.Add(btnAdmin);
            
            // Customer Button
            Button btnCustomer = new Button();
            btnCustomer.Text = "🛒 Customer";
            btnCustomer.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            btnCustomer.Size = new Size(300, 60);
            btnCustomer.Location = new Point(150, 270);
            btnCustomer.BackColor = Color.FromArgb(251, 191, 36); // Yellow
            btnCustomer.ForeColor = Color.FromArgb(20, 83, 45);
            btnCustomer.FlatStyle = FlatStyle.Flat;
            btnCustomer.FlatAppearance.BorderSize = 0;
            btnCustomer.Cursor = Cursors.Hand;
            btnCustomer.Click += BtnCustomer_Click;
            this.Controls.Add(btnCustomer);
            
            // Footer Label
            Label lblFooter = new Label();
            lblFooter.Text = "Built with ❤️ for organic food enthusiasts";
            lblFooter.Font = new Font("Segoe UI", 9);
            lblFooter.ForeColor = Color.Gray;
            lblFooter.AutoSize = true;
            lblFooter.Location = new Point(180, 370);
            this.Controls.Add(lblFooter);
            
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void BtnAdmin_Click(object sender, EventArgs e)
        {
            // Open Admin Login Form
            Admin.AdminLoginForm adminLogin = new Admin.AdminLoginForm();
            this.Hide();
            adminLogin.ShowDialog();
            this.Show();
        }

        private void BtnCustomer_Click(object sender, EventArgs e)
        {
            // Open Customer Login Form
            Customer.CustomerLoginForm customerLogin = new Customer.CustomerLoginForm();
            this.Hide();
            customerLogin.ShowDialog();
            this.Show();
        }
    }
}
