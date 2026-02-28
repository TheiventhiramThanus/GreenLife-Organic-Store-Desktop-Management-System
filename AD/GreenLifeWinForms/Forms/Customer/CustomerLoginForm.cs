using System;
using System.Drawing;
using System.Windows.Forms;
using GreenLifeWinForms.Services;

namespace GreenLifeWinForms.Forms.Customer
{
    public partial class CustomerLoginForm : Form
    {
        private TextBox txtEmail;
        private TextBox txtPassword;
        private Button btnLogin;
        private AuthService authService;

        public CustomerLoginForm()
        {
            authService = new AuthService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Text = "Customer Login - GreenLife";
            this.Size = new Size(500, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 253, 244);
            
            // Title Label
            Label lblTitle = new Label();
            lblTitle.Text = "Customer Login";
            lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(22, 163, 74);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(140, 30);
            this.Controls.Add(lblTitle);
            
            // Email Label
            Label lblEmail = new Label();
            lblEmail.Text = "Email:";
            lblEmail.Font = new Font("Segoe UI", 11);
            lblEmail.ForeColor = Color.FromArgb(20, 83, 45);
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(80, 100);
            this.Controls.Add(lblEmail);
            
            // Email TextBox
            txtEmail = new TextBox();
            txtEmail.Font = new Font("Segoe UI", 11);
            txtEmail.Size = new Size(300, 30);
            txtEmail.Location = new Point(80, 130);
            this.Controls.Add(txtEmail);
            
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
            
            // Register Link
            LinkLabel linkRegister = new LinkLabel();
            linkRegister.Text = "Don't have an account? Register here";
            linkRegister.Font = new Font("Segoe UI", 10);
            linkRegister.LinkColor = Color.FromArgb(22, 163, 74);
            linkRegister.AutoSize = true;
            linkRegister.Location = new Point(120, 330);
            linkRegister.LinkClicked += LinkRegister_LinkClicked;
            this.Controls.Add(linkRegister);
            
            // Demo Credentials Label
            Label lblDemo = new Label();
            lblDemo.Text = "Demo: sarah.johnson@email.com / password123";
            lblDemo.Font = new Font("Segoe UI", 9);
            lblDemo.ForeColor = Color.Gray;
            lblDemo.AutoSize = true;
            lblDemo.Location = new Point(110, 370);
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
                string email = txtEmail.Text.Trim();
                string password = txtPassword.Text;

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Please enter both email and password.", 
                        "Validation Error", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Warning);
                    return;
                }

                // Authenticate customer
                Models.Customer customer = authService.CustomerLogin(email, password);

                if (customer != null)
                {
                    MessageBox.Show($"Welcome, {customer.FullName}!", 
                        "Login Successful", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
                    
                    // Open Customer Dashboard
                    CustomerDashboardForm dashboard = new CustomerDashboardForm(customer);
                    this.Hide();
                    dashboard.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid email or password.", 
                        "Login Failed", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtEmail.Focus();
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

        private void LinkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CustomerRegistrationForm registrationForm = new CustomerRegistrationForm();
            if (registrationForm.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Registration successful! You can now login.", 
                    "Success", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
            }
        }
    }
}
