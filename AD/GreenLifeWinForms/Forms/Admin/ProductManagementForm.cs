using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GreenLifeWinForms.Models;
using GreenLifeWinForms.Services;

namespace GreenLifeWinForms.Forms.Admin
{
    public partial class ProductManagementForm : Form
    {
        private ProductService productService;
        private DataGridView dgvProducts;
        private TextBox txtSearch;
        private ComboBox cmbCategory;
        private Button btnAdd, btnEdit, btnDelete, btnRefresh;
        private List<Product> currentProducts;

        public ProductManagementForm()
        {
            productService = new ProductService();
            currentProducts = new List<Product>();
            InitializeComponent();
            LoadProducts();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Text = "Product Management - GreenLife";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 253, 244);
            
            // Title Label
            Label lblTitle = new Label();
            lblTitle.Text = "📦 Product Management";
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
            
            // Category Label
            Label lblCategory = new Label();
            lblCategory.Text = "Category:";
            lblCategory.Font = new Font("Segoe UI", 10);
            lblCategory.Location = new Point(340, 20);
            lblCategory.AutoSize = true;
            searchPanel.Controls.Add(lblCategory);
            
            // Category ComboBox
            cmbCategory = new ComboBox();
            cmbCategory.Font = new Font("Segoe UI", 10);
            cmbCategory.Size = new Size(200, 25);
            cmbCategory.Location = new Point(420, 17);
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategory.SelectedIndexChanged += CmbCategory_SelectedIndexChanged;
            searchPanel.Controls.Add(cmbCategory);
            
            // Refresh Button
            btnRefresh = new Button();
            btnRefresh.Text = "🔄 Refresh";
            btnRefresh.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnRefresh.Size = new Size(120, 35);
            btnRefresh.Location = new Point(640, 12);
            btnRefresh.BackColor = Color.FromArgb(59, 130, 246);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.Click += BtnRefresh_Click;
            searchPanel.Controls.Add(btnRefresh);
            
            // DataGridView
            dgvProducts = new DataGridView();
            dgvProducts.Location = new Point(20, 150);
            dgvProducts.Size = new Size(1150, 420);
            dgvProducts.BackgroundColor = Color.White;
            dgvProducts.BorderStyle = BorderStyle.None;
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.AllowUserToDeleteRows = false;
            dgvProducts.ReadOnly = true;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = false;
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProducts.RowHeadersVisible = false;
            dgvProducts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 253, 244);
            dgvProducts.DefaultCellStyle.SelectionBackColor = Color.FromArgb(22, 163, 74);
            dgvProducts.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(22, 163, 74);
            dgvProducts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProducts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvProducts.EnableHeadersVisualStyles = false;
            this.Controls.Add(dgvProducts);
            
            // Action Buttons Panel
            Panel actionPanel = new Panel();
            actionPanel.Location = new Point(20, 590);
            actionPanel.Size = new Size(1150, 60);
            this.Controls.Add(actionPanel);
            
            // Add Button
            btnAdd = new Button();
            btnAdd.Text = "➕ Add Product";
            btnAdd.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnAdd.Size = new Size(160, 45);
            btnAdd.Location = new Point(0, 5);
            btnAdd.BackColor = Color.FromArgb(22, 163, 74);
            btnAdd.ForeColor = Color.White;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Cursor = Cursors.Hand;
            btnAdd.Click += BtnAdd_Click;
            actionPanel.Controls.Add(btnAdd);
            
            // Edit Button
            btnEdit = new Button();
            btnEdit.Text = "✏️ Edit Product";
            btnEdit.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnEdit.Size = new Size(160, 45);
            btnEdit.Location = new Point(180, 5);
            btnEdit.BackColor = Color.FromArgb(251, 191, 36);
            btnEdit.ForeColor = Color.FromArgb(20, 83, 45);
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Cursor = Cursors.Hand;
            btnEdit.Click += BtnEdit_Click;
            actionPanel.Controls.Add(btnEdit);
            
            // Delete Button
            btnDelete = new Button();
            btnDelete.Text = "🗑️ Delete Product";
            btnDelete.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnDelete.Size = new Size(160, 45);
            btnDelete.Location = new Point(360, 5);
            btnDelete.BackColor = Color.FromArgb(239, 68, 68);
            btnDelete.ForeColor = Color.White;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Cursor = Cursors.Hand;
            btnDelete.Click += BtnDelete_Click;
            actionPanel.Controls.Add(btnDelete);
            
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

        private void LoadProducts()
        {
            try
            {
                currentProducts = productService.GetAllProducts();
                DisplayProducts(currentProducts);
                LoadCategories();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}", 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        private void LoadCategories()
        {
            try
            {
                List<string> categories = productService.GetProductCategories();
                cmbCategory.Items.Clear();
                cmbCategory.Items.Add("All");
                foreach (string category in categories)
                {
                    cmbCategory.Items.Add(category);
                }
                cmbCategory.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        private void DisplayProducts(List<Product> products)
        {
            dgvProducts.DataSource = null;
            dgvProducts.Columns.Clear();
            
            dgvProducts.DataSource = products;
            
            // Customize columns
            if (dgvProducts.Columns.Count > 0)
            {
                dgvProducts.Columns["ProductId"].HeaderText = "ID";
                dgvProducts.Columns["ProductId"].Width = 50;
                dgvProducts.Columns["Name"].HeaderText = "Product Name";
                dgvProducts.Columns["Name"].Width = 200;
                dgvProducts.Columns["Category"].HeaderText = "Category";
                dgvProducts.Columns["Category"].Width = 120;
                dgvProducts.Columns["Supplier"].HeaderText = "Supplier";
                dgvProducts.Columns["Supplier"].Width = 150;
                dgvProducts.Columns["Price"].HeaderText = "Price";
                dgvProducts.Columns["Price"].DefaultCellStyle.Format = "'LKR '#,##0.00";
                dgvProducts.Columns["Price"].Width = 100;
                dgvProducts.Columns["Stock"].HeaderText = "Stock";
                dgvProducts.Columns["Stock"].Width = 70;
                dgvProducts.Columns["Discount"].HeaderText = "Discount %";
                dgvProducts.Columns["Discount"].Width = 90;
                dgvProducts.Columns["StockStatus"].HeaderText = "Status";
                dgvProducts.Columns["StockStatus"].Width = 100;
                
                // Hide unnecessary columns
                dgvProducts.Columns["Image"].Visible = false;
                dgvProducts.Columns["Description"].Visible = false;
                dgvProducts.Columns["CreatedDate"].Visible = false;
                dgvProducts.Columns["IsActive"].Visible = false;
                dgvProducts.Columns["DiscountedPrice"].Visible = false;
                dgvProducts.Columns["IsLowStock"].Visible = false;
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            SearchProducts();
        }

        private void CmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchProducts();
        }

        private void SearchProducts()
        {
            try
            {
                string searchTerm = txtSearch.Text.Trim();
                string category = cmbCategory.SelectedItem?.ToString() ?? "All";
                
                List<Product> filteredProducts = productService.SearchProducts(searchTerm, category);
                DisplayProducts(filteredProducts);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching products: {ex.Message}", 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            cmbCategory.SelectedIndex = 0;
            LoadProducts();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            AddEditProductForm addForm = new AddEditProductForm();
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                LoadProducts();
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product to edit.", 
                    "No Selection", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                return;
            }

            Product selectedProduct = (Product)dgvProducts.SelectedRows[0].DataBoundItem;
            AddEditProductForm editForm = new AddEditProductForm(selectedProduct);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadProducts();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product to delete.", 
                    "No Selection", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                return;
            }

            Product selectedProduct = (Product)dgvProducts.SelectedRows[0].DataBoundItem;
            
            DialogResult result = MessageBox.Show(
                $"Are you sure you want to delete '{selectedProduct.Name}'?\n\nThis action cannot be undone.", 
                "Confirm Delete", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    bool success = productService.DeleteProduct(selectedProduct.ProductId);
                    if (success)
                    {
                        MessageBox.Show("Product deleted successfully!", 
                            "Success", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Information);
                        LoadProducts();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete product.", 
                            "Error", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting product: {ex.Message}", 
                        "Error", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
                }
            }
        }
    }
}
