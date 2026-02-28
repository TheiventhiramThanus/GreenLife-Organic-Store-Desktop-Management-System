using System;
using System.Drawing;
using System.Windows.Forms;
using GreenLifeWinForms.Services;
using GreenLifeWinForms.Models;

namespace GreenLifeWinForms.Forms.Admin
{
    public partial class AdminLoginForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private AuthService authService;

        public AdminLoginForm()
        {
            authService = new AuthService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Text = "Admin Login - GreenLife";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 253, 244);
            
            // Title Label
            Label lblTitle = new Label();
            lblTitle.Text = "Administrator Login";
            lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(22, 163, 74);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(110, 30);
            this.Controls.Add(lblTitle);
            
            // Username Label
            Label lblUsername = new Label();
            lblUsername.Text = "Username:";
            lblUsername.Font = new Font("Segoe UI", 11);
            lblUsername.ForeColor = Color.FromArgb(20, 83, 45);
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(80, 100);
            this.Controls.Add(lblUsername);
            
            // Username TextBox
            txtUsername = new TextBox();
            txtUsername.Font = new Font("Segoe UI", 11);
            txtUsername.Size = new Size(300, 30);
            txtUsername.Location = new Point(80, 130);
            this.Controls.Add(txtUsername);
            
            // Password Label
            Label lblPassword = new Label();
            lblPassword.Text = "Password:";
            lblPassword.Font = new Font("Segoe UI", 11);
            lblPassword.ForeColor = Color.FromArgb(20, 83, 45);
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(80, 180);
            this.Controls.Add(lblPassword);
            
            // Password TextBox
            txtPassword = new TextBox();
            txtPassword.Font = new Font("Segoe UI", 11);
            txtPassword.Size = new Size(300, 30);
            txtPassword.Location = new Point(80, 210);
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.KeyPress += TxtPassword_KeyPress;
            this.Controls.Add(txtPassword);
            
            // Login Button
            btnLogin = new Button();
            btnLogin.Text = "Login";
            btnLogin.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnLogin.Size = new Size(300, 45);
            btnLogin.Location = new Point(80, 270);
            btnLogin.BackColor = Color.FromArgb(22, 163, 74);
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.Click += BtnLogin_Click;
            this.Controls.Add(btnLogin);
            
            // Demo Credentials Label
            Label lblDemo = new Label();
            lblDemo.Text = "Demo: admin / admin123";
            lblDemo.Font = new Font("Segoe UI", 9);
            lblDemo.ForeColor = Color.Gray;
            lblDemo.AutoSize = true;
            lblDemo.Location = new Point(170, 330);
            this.Controls.Add(lblDemo);
            
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void TxtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnLogin_Click(sender, e);
            }
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text;

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Please enter both username and password.", 
                        "Validation Error", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Warning);
                    return;
                }

                // Authenticate admin
                Models.Admin admin = authService.AdminLogin(username, password);

                if (admin != null)
                {
                    MessageBox.Show($"Welcome, {admin.FullName}!", 
                        "Login Successful", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
                    
                    // Open Admin Dashboard
                    AdminDashboardForm dashboard = new AdminDashboardForm(admin);
                    this.Hide();
                    dashboard.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.", 
                        "Login Failed", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtUsername.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login error: {ex.Message}", 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }
    }
}
