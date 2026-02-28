using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using GreenLifeWinForms.Models;
using GreenLifeWinForms.Services;

namespace GreenLifeWinForms.Forms.Admin
{
    public partial class AddEditProductForm : Form
    {
        private ProductService productService;
        private Product currentProduct;
        private bool isEditMode;
        
        private TextBox txtName, txtSupplier, txtPrice, txtStock, txtDiscount, txtImage, txtDescription;
        private ComboBox cmbCategory;
        private PictureBox picPreview;
        private Button btnSave, btnCancel;

        public AddEditProductForm(Product product = null)
        {
            productService = new ProductService();
            currentProduct = product;
            isEditMode = product != null;
            InitializeComponent();
            
            if (isEditMode)
            {
                LoadProductData();
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Text = isEditMode ? "Edit Product - GreenLife" : "Add New Product - GreenLife";
            this.Size = new Size(650, 720);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(240, 253, 244);
            
            // Title Label
            Label lblTitle = new Label();
            lblTitle.Text = isEditMode ? "✏️ Edit Product" : "➕ Add New Product";
            lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(22, 163, 74);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(30, 15);
            this.Controls.Add(lblTitle);
            
            int yPos = 60;
            int labelX = 30;
            int controlX = 175;
            int spacing = 45;
            
            // Product Name
            CreateLabel("Product Name:", labelX, yPos);
            txtName = CreateTextBox(controlX, yPos, 420);
            yPos += spacing;
            
            // Category
            CreateLabel("Category:", labelX, yPos);
            cmbCategory = new ComboBox();
            cmbCategory.Font = new Font("Segoe UI", 10);
            cmbCategory.Size = new Size(420, 28);
            cmbCategory.Location = new Point(controlX, yPos);
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategory.Items.AddRange(new string[] { 
                "Fruits", "Vegetables", "Dairy", "Grains", 
                "Beverages", "Snacks", "Meat & Poultry" 
            });
            this.Controls.Add(cmbCategory);
            yPos += spacing;
            
            // Supplier
            CreateLabel("Supplier:", labelX, yPos);
            txtSupplier = CreateTextBox(controlX, yPos, 420);
            yPos += spacing;
            
            // Price + Stock on same row
            CreateLabel("Price (LKR):", labelX, yPos);
            txtPrice = CreateTextBox(controlX, yPos, 150);
            txtPrice.KeyPress += NumericTextBox_KeyPress;
            
            CreateLabel("Stock:", 360, yPos);
            txtStock = CreateTextBox(420, yPos, 175);
            txtStock.KeyPress += IntegerTextBox_KeyPress;
            yPos += spacing;
            
            // Discount
            CreateLabel("Discount (%):", labelX, yPos);
            txtDiscount = CreateTextBox(controlX, yPos, 150);
            txtDiscount.Text = "0";
            txtDiscount.KeyPress += NumericTextBox_KeyPress;
            yPos += spacing;
            
            // Image — Browse button + path + preview
            CreateLabel("Image:", labelX, yPos);
            txtImage = CreateTextBox(controlX, yPos, 300);
            txtImage.Text = "product.jpg";
            txtImage.ReadOnly = true;
            txtImage.BackColor = Color.FromArgb(245, 245, 245);
            
            Button btnBrowse = new Button();
            btnBrowse.Text = "📁 Browse...";
            btnBrowse.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnBrowse.Size = new Size(110, 28);
            btnBrowse.Location = new Point(485, yPos);
            btnBrowse.BackColor = Color.FromArgb(59, 130, 246);
            btnBrowse.ForeColor = Color.White;
            btnBrowse.FlatStyle = FlatStyle.Flat;
            btnBrowse.FlatAppearance.BorderSize = 0;
            btnBrowse.Cursor = Cursors.Hand;
            btnBrowse.Click += BtnBrowse_Click;
            this.Controls.Add(btnBrowse);
            yPos += 35;

            // Image Preview
            picPreview = new PictureBox();
            picPreview.Location = new Point(controlX, yPos);
            picPreview.Size = new Size(120, 80);
            picPreview.BorderStyle = BorderStyle.FixedSingle;
            picPreview.SizeMode = PictureBoxSizeMode.Zoom;
            picPreview.BackColor = Color.White;
            this.Controls.Add(picPreview);

            Label lblPreviewHint = new Label();
            lblPreviewHint.Text = "Supported: JPG, PNG, BMP, GIF";
            lblPreviewHint.Font = new Font("Segoe UI", 8);
            lblPreviewHint.ForeColor = Color.Gray;
            lblPreviewHint.AutoSize = true;
            lblPreviewHint.Location = new Point(305, yPos + 30);
            this.Controls.Add(lblPreviewHint);

            yPos += 95;
            
            // Description
            CreateLabel("Description:", labelX, yPos);
            txtDescription = new TextBox();
            txtDescription.Font = new Font("Segoe UI", 10);
            txtDescription.Size = new Size(420, 70);
            txtDescription.Location = new Point(controlX, yPos);
            txtDescription.Multiline = true;
            txtDescription.ScrollBars = ScrollBars.Vertical;
            this.Controls.Add(txtDescription);
            yPos += 85;
            
            // Buttons Panel
            Panel buttonPanel = new Panel();
            buttonPanel.Location = new Point(30, yPos);
            buttonPanel.Size = new Size(580, 50);
            this.Controls.Add(buttonPanel);
            
            // Save Button
            btnSave = new Button();
            btnSave.Text = isEditMode ? "💾 Update Product" : "💾 Save Product";
            btnSave.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnSave.Size = new Size(270, 45);
            btnSave.Location = new Point(0, 0);
            btnSave.BackColor = Color.FromArgb(22, 163, 74);
            btnSave.ForeColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Cursor = Cursors.Hand;
            btnSave.Click += BtnSave_Click;
            buttonPanel.Controls.Add(btnSave);
            
            // Cancel Button
            btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Font = new Font("Segoe UI", 12);
            btnCancel.Size = new Size(270, 45);
            btnCancel.Location = new Point(290, 0);
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

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select Product Image";
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|JPEG Files|*.jpg;*.jpeg|PNG Files|*.png|All Files|*.*";
                openFileDialog.FilterIndex = 1;
                
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFile = openFileDialog.FileName;
                    string fileName = Path.GetFileName(selectedFile);
                    
                    // Store just the filename in the textbox
                    txtImage.Text = fileName;
                    
                    // Copy image to app's images folder
                    try
                    {
                        string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                        if (!Directory.Exists(imagesFolder))
                            Directory.CreateDirectory(imagesFolder);
                        
                        string destPath = Path.Combine(imagesFolder, fileName);
                        File.Copy(selectedFile, destPath, true);
                    }
                    catch
                    {
                        // If copy fails, store the full path instead
                        txtImage.Text = selectedFile;
                    }
                    
                    // Show preview
                    try
                    {
                        using (var stream = new FileStream(selectedFile, FileMode.Open, FileAccess.Read))
                        {
                            picPreview.Image = Image.FromStream(stream);
                        }
                    }
                    catch
                    {
                        picPreview.Image = null;
                    }
                }
            }
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

        private void NumericTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Only allow one decimal point
            if (e.KeyChar == '.' && ((TextBox)sender).Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void IntegerTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void LoadProductData()
        {
            if (currentProduct != null)
            {
                txtName.Text = currentProduct.Name;
                cmbCategory.SelectedItem = currentProduct.Category;
                txtSupplier.Text = currentProduct.Supplier;
                txtPrice.Text = currentProduct.Price.ToString("F2");
                txtStock.Text = currentProduct.Stock.ToString();
                txtDiscount.Text = currentProduct.Discount.ToString("F2");
                txtImage.Text = currentProduct.Image;
                txtDescription.Text = currentProduct.Description;
                
                // Try to load image preview
                try
                {
                    string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                    string imagePath = Path.Combine(imagesFolder, currentProduct.Image);
                    if (File.Exists(imagePath))
                    {
                        using (var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                        {
                            picPreview.Image = Image.FromStream(stream);
                        }
                    }
                    else if (File.Exists(currentProduct.Image))
                    {
                        using (var stream = new FileStream(currentProduct.Image, FileMode.Open, FileAccess.Read))
                        {
                            picPreview.Image = Image.FromStream(stream);
                        }
                    }
                }
                catch
                {
                    // No preview available
                }
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter product name.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            if (cmbCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a category.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategory.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSupplier.Text))
            {
                MessageBox.Show("Please enter supplier name.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSupplier.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Please enter a valid price greater than 0.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrice.Focus();
                return false;
            }

            if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
            {
                MessageBox.Show("Please enter a valid stock quantity (0 or greater).", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStock.Focus();
                return false;
            }

            if (!decimal.TryParse(txtDiscount.Text, out decimal discount) || discount < 0 || discount > 100)
            {
                MessageBox.Show("Please enter a valid discount (0-100).", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiscount.Focus();
                return false;
            }

            return true;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                Product product = new Product
                {
                    Name = txtName.Text.Trim(),
                    Category = cmbCategory.SelectedItem.ToString(),
                    Supplier = txtSupplier.Text.Trim(),
                    Price = decimal.Parse(txtPrice.Text),
                    Stock = int.Parse(txtStock.Text),
                    Discount = decimal.Parse(txtDiscount.Text),
                    Image = txtImage.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    IsActive = true,
                    CreatedDate = DateTime.Now
                };

                bool success;
                if (isEditMode)
                {
                    product.ProductId = currentProduct.ProductId;
                    success = productService.UpdateProduct(product);
                }
                else
                {
                    success = productService.AddProduct(product);
                }

                if (success)
                {
                    MessageBox.Show(
                        isEditMode ? "Product updated successfully!" : "Product added successfully!", 
                        "Success", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                        isEditMode ? "Failed to update product." : "Failed to add product.", 
                        "Error", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving product: {ex.Message}", 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }
    }
}
