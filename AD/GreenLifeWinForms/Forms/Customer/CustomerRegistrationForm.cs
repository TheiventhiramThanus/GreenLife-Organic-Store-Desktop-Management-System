using System;
using System.Drawing;
using System.Windows.Forms;
using GreenLifeWinForms.Models;
using GreenLifeWinForms.Services;

namespace GreenLifeWinForms.Forms.Customer
{
    public partial class CustomerRegistrationForm : Form
    {
        private AuthService authService;
        private TextBox txtFullName, txtEmail, txtPhone, txtAddress, txtPassword, txtConfirmPassword;
        private Button btnRegister, btnCancel;

        public CustomerRegistrationForm()
        {
            authService = new AuthService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Text = "Customer Registration - GreenLife";
            this.Size = new Size(600, 650);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(240, 253, 244);
            
            // Title Label
            Label lblTitle = new Label();
            lblTitle.Text = "📝 Create New Account";
            lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(22, 163, 74);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(150, 20);
            this.Controls.Add(lblTitle);
            
            // Subtitle
            Label lblSubtitle = new Label();
            lblSubtitle.Text = "Join GreenLife Organic Store";
            lblSubtitle.Font = new Font("Segoe UI", 12);
            lblSubtitle.ForeColor = Color.FromArgb(20, 83, 45);
            lblSubtitle.AutoSize = true;
            lblSubtitle.Location = new Point(180, 60);
            this.Controls.Add(lblSubtitle);
            
            int yPos = 110;
            int labelX = 50;
            int controlX = 200;
            int spacing = 70;
            
            // Full Name
            CreateLabel("Full Name:", labelX, yPos);
            txtFullName = CreateTextBox(controlX, yPos, 320);
            yPos += spacing;
            
            // Email
            CreateLabel("Email:", labelX, yPos);
            txtEmail = CreateTextBox(controlX, yPos, 320);
            yPos += spacing;
            
            // Phone
            CreateLabel("Phone:", labelX, yPos);
            txtPhone = CreateTextBox(controlX, yPos, 320);
            yPos += spacing;
            
            // Address
            CreateLabel("Address:", labelX, yPos);
            txtAddress = new TextBox();
            txtAddress.Font = new Font("Segoe UI", 11);
            txtAddress.Size = new Size(320, 60);
            txtAddress.Location = new Point(controlX, yPos);
            txtAddress.Multiline = true;
            txtAddress.ScrollBars = ScrollBars.Vertical;
            this.Controls.Add(txtAddress);
            yPos += 80;
            
            // Password
            CreateLabel("Password:", labelX, yPos);
            txtPassword = CreateTextBox(controlX, yPos, 320);
            txtPassword.UseSystemPasswordChar = true;
            yPos += spacing;
            
            // Confirm Password
            CreateLabel("Confirm Password:", labelX, yPos);
            txtConfirmPassword = CreateTextBox(controlX, yPos, 320);
            txtConfirmPassword.UseSystemPasswordChar = true;
            yPos += spacing + 10;
            
            // Buttons Panel
            Panel buttonPanel = new Panel();
            buttonPanel.Location = new Point(50, yPos);
            buttonPanel.Size = new Size(500, 50);
            this.Controls.Add(buttonPanel);
            
            // Register Button
            btnRegister = new Button();
            btnRegister.Text = "✅ Register";
            btnRegister.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnRegister.Size = new Size(230, 45);
            btnRegister.Location = new Point(0, 0);
            btnRegister.BackColor = Color.FromArgb(22, 163, 74);
            btnRegister.ForeColor = Color.White;
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Cursor = Cursors.Hand;
            btnRegister.Click += BtnRegister_Click;
            buttonPanel.Controls.Add(btnRegister);
            
            // Cancel Button
            btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Font = new Font("Segoe UI", 12);
            btnCancel.Size = new Size(230, 45);
            btnCancel.Location = new Point(250, 0);
            btnCancel.BackColor = Color.Gray;
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
            buttonPanel.Controls.Add(btnCancel);
            
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Label CreateLabel(string text, int x, int y)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lbl.ForeColor = Color.FromArgb(20, 83, 45);
            lbl.AutoSize = true;
            lbl.Location = new Point(x, y + 5);
            this.Controls.Add(lbl);
            return lbl;
        }

        private TextBox CreateTextBox(int x, int y, int width)
        {
            TextBox txt = new TextBox();
            txt.Font = new Font("Segoe UI", 11);
            txt.Size = new Size(width, 30);
            txt.Location = new Point(x, y);
            this.Controls.Add(txt);
            return txt;
        }

        private bool ValidateInput()
        {
            // Full Name
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Please enter your full name.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return false;
            }

            // Email
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Please enter your email address.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            if (!txtEmail.Text.Contains("@") || !txtEmail.Text.Contains("."))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            // Phone
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Please enter your phone number.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return false;
            }

            // Address
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Please enter your address.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAddress.Focus();
                return false;
            }

            // Password
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter a password.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }

            if (txtPassword.Text.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters long.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }

            // Confirm Password
            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmPassword.Focus();
                return false;
            }

            return true;
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                // Check if email already exists
                if (authService.EmailExists(txtEmail.Text.Trim()))
                {
                    MessageBox.Show("This email is already registered. Please use a different email or login.", 
                        "Email Already Exists", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return;
                }

                // Create new customer
                Models.Customer newCustomer = new Models.Customer
                {
                    FullName = txtFullName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                    Password = txtPassword.Text,
                    IsActive = true,
                    RegisteredDate = DateTime.Now
                };

                bool success = authService.RegisterCustomer(newCustomer);

                if (success)
                {
                    // Send welcome email (non-blocking)
                    try
                    {
                        var emailService = new EmailNotificationService();
                        emailService.SendWelcomeEmail(newCustomer.Email, newCustomer.FullName);
                    }
                    catch { }

                    MessageBox.Show("Registration successful!\n\nYou can now login with your email and password.", 
                        "Success", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Registration failed. Please try again.", 
                        "Error", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Registration error: {ex.Message}", 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }
    }
}
