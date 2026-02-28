using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GreenLifeWinForms.Models;
using GreenLifeWinForms.Services;

namespace GreenLifeWinForms.Forms.Customer
{
    public partial class ShoppingCartForm : Form
    {
        private ProductService productService;
        private OrderService orderService;
        private EmailNotificationService emailService;
        private Models.Customer currentCustomer;
        private Dictionary<int, int> cart; // ProductId -> Quantity
        private List<Product> cartProducts;
        
        private DataGridView dgvCart;
        private Label lblSubtotal, lblDiscount, lblGrandTotal;
        private Button btnCheckout, btnClearCart;

        public ShoppingCartForm(Models.Customer customer, Dictionary<int, int> cartItems)
        {
            currentCustomer = customer;
            cart = cartItems;
            productService = new ProductService();
            orderService = new OrderService();
            emailService = new EmailNotificationService();
            cartProducts = new List<Product>();
            InitializeComponent();
            LoadCartItems();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Text = "Shopping Cart - GreenLife";
            this.Size = new Size(900, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 253, 244);
            
            // Title Label
            Label lblTitle = new Label();
            lblTitle.Text = "🛍️ Your Shopping Cart";
            lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(22, 163, 74);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(30, 20);
            this.Controls.Add(lblTitle);
            
            // Customer Info
            Label lblCustomer = new Label();
            lblCustomer.Text = $"Customer: {currentCustomer.FullName}";
            lblCustomer.Font = new Font("Segoe UI", 11);
            lblCustomer.ForeColor = Color.FromArgb(20, 83, 45);
            lblCustomer.AutoSize = true;
            lblCustomer.Location = new Point(30, 60);
            this.Controls.Add(lblCustomer);
            
            // DataGridView
            dgvCart = new DataGridView();
            dgvCart.Location = new Point(30, 100);
            dgvCart.Size = new Size(830, 350);
            dgvCart.BackgroundColor = Color.White;
            dgvCart.BorderStyle = BorderStyle.None;
            dgvCart.AllowUserToAddRows = false;
            dgvCart.AllowUserToDeleteRows = false;
            dgvCart.ReadOnly = false;
            dgvCart.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCart.MultiSelect = false;
            dgvCart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCart.RowHeadersVisible = false;
            dgvCart.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 253, 244);
            dgvCart.DefaultCellStyle.SelectionBackColor = Color.FromArgb(22, 163, 74);
            dgvCart.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvCart.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(22, 163, 74);
            dgvCart.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCart.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvCart.EnableHeadersVisualStyles = false;
            dgvCart.CellValueChanged += DgvCart_CellValueChanged;
            this.Controls.Add(dgvCart);
            
            // Summary Panel
            Panel summaryPanel = new Panel();
            summaryPanel.Location = new Point(30, 470);
            summaryPanel.Size = new Size(830, 120);
            summaryPanel.BackColor = Color.White;
            this.Controls.Add(summaryPanel);
            
            // Subtotal
            Label lblSubtotalLabel = new Label();
            lblSubtotalLabel.Text = "Subtotal:";
            lblSubtotalLabel.Font = new Font("Segoe UI", 12);
            lblSubtotalLabel.Location = new Point(550, 20);
            lblSubtotalLabel.AutoSize = true;
            summaryPanel.Controls.Add(lblSubtotalLabel);
            
            lblSubtotal = new Label();
            lblSubtotal.Text = "LKR 0.00";
            lblSubtotal.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblSubtotal.Location = new Point(700, 20);
            lblSubtotal.AutoSize = true;
            summaryPanel.Controls.Add(lblSubtotal);
            
            // Discount
            Label lblDiscountLabel = new Label();
            lblDiscountLabel.Text = "Discount:";
            lblDiscountLabel.Font = new Font("Segoe UI", 12);
            lblDiscountLabel.ForeColor = Color.FromArgb(239, 68, 68);
            lblDiscountLabel.Location = new Point(550, 50);
            lblDiscountLabel.AutoSize = true;
            summaryPanel.Controls.Add(lblDiscountLabel);
            
            lblDiscount = new Label();
            lblDiscount.Text = "LKR 0.00";
            lblDiscount.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblDiscount.ForeColor = Color.FromArgb(239, 68, 68);
            lblDiscount.Location = new Point(700, 50);
            lblDiscount.AutoSize = true;
            summaryPanel.Controls.Add(lblDiscount);
            
            // Grand Total
            Label lblGrandTotalLabel = new Label();
            lblGrandTotalLabel.Text = "Grand Total:";
            lblGrandTotalLabel.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblGrandTotalLabel.ForeColor = Color.FromArgb(22, 163, 74);
            lblGrandTotalLabel.Location = new Point(550, 80);
            lblGrandTotalLabel.AutoSize = true;
            summaryPanel.Controls.Add(lblGrandTotalLabel);
            
            lblGrandTotal = new Label();
            lblGrandTotal.Text = "LKR 0.00";
            lblGrandTotal.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblGrandTotal.ForeColor = Color.FromArgb(22, 163, 74);
            lblGrandTotal.Location = new Point(700, 78);
            lblGrandTotal.AutoSize = true;
            summaryPanel.Controls.Add(lblGrandTotal);
            
            // Buttons Panel
            Panel buttonPanel = new Panel();
            buttonPanel.Location = new Point(30, 610);
            buttonPanel.Size = new Size(830, 50);
            this.Controls.Add(buttonPanel);
            
            // Clear Cart Button
            btnClearCart = new Button();
            btnClearCart.Text = "🗑️ Clear Cart";
            btnClearCart.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnClearCart.Size = new Size(150, 45);
            btnClearCart.Location = new Point(0, 0);
            btnClearCart.BackColor = Color.FromArgb(239, 68, 68);
            btnClearCart.ForeColor = Color.White;
            btnClearCart.FlatStyle = FlatStyle.Flat;
            btnClearCart.FlatAppearance.BorderSize = 0;
            btnClearCart.Cursor = Cursors.Hand;
            btnClearCart.Click += BtnClearCart_Click;
            buttonPanel.Controls.Add(btnClearCart);
            
            // Remove Item Button
            Button btnRemove = new Button();
            btnRemove.Text = "Remove Item";
            btnRemove.Font = new Font("Segoe UI", 11);
            btnRemove.Size = new Size(150, 45);
            btnRemove.Location = new Point(170, 0);
            btnRemove.BackColor = Color.FromArgb(251, 191, 36);
            btnRemove.ForeColor = Color.FromArgb(20, 83, 45);
            btnRemove.FlatStyle = FlatStyle.Flat;
            btnRemove.FlatAppearance.BorderSize = 0;
            btnRemove.Cursor = Cursors.Hand;
            btnRemove.Click += BtnRemove_Click;
            buttonPanel.Controls.Add(btnRemove);
            
            // Checkout Button
            btnCheckout = new Button();
            btnCheckout.Text = "✅ Proceed to Checkout";
            btnCheckout.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnCheckout.Size = new Size(250, 45);
            btnCheckout.Location = new Point(580, 0);
            btnCheckout.BackColor = Color.FromArgb(22, 163, 74);
            btnCheckout.ForeColor = Color.White;
            btnCheckout.FlatStyle = FlatStyle.Flat;
            btnCheckout.FlatAppearance.BorderSize = 0;
            btnCheckout.Cursor = Cursors.Hand;
            btnCheckout.Click += BtnCheckout_Click;
            buttonPanel.Controls.Add(btnCheckout);
            
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void LoadCartItems()
        {
            try
            {
                cartProducts.Clear();
                foreach (var item in cart)
                {
                    Product product = productService.GetProductById(item.Key);
                    if (product != null)
                    {
                        cartProducts.Add(product);
                    }
                }
                
                DisplayCart();
                CalculateTotals();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading cart: {ex.Message}", 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        private void DisplayCart()
        {
            dgvCart.DataSource = null;
            dgvCart.Columns.Clear();
            
            // Create columns
            dgvCart.Columns.Add("ProductId", "ID");
            dgvCart.Columns.Add("Name", "Product Name");
            dgvCart.Columns.Add("Price", "Unit Price");
            dgvCart.Columns.Add("Discount", "Discount %");
            dgvCart.Columns.Add("DiscountedPrice", "Discounted Price");
            
            DataGridViewTextBoxColumn qtyColumn = new DataGridViewTextBoxColumn();
            qtyColumn.Name = "Quantity";
            qtyColumn.HeaderText = "Quantity";
            qtyColumn.ReadOnly = false;
            dgvCart.Columns.Add(qtyColumn);
            
            dgvCart.Columns.Add("LineTotal", "Line Total");
            
            // Set column widths
            dgvCart.Columns["ProductId"].Width = 50;
            dgvCart.Columns["Name"].Width = 250;
            dgvCart.Columns["Price"].Width = 100;
            dgvCart.Columns["Discount"].Width = 80;
            dgvCart.Columns["DiscountedPrice"].Width = 120;
            dgvCart.Columns["Quantity"].Width = 80;
            dgvCart.Columns["LineTotal"].Width = 120;
            
            // Add rows
            foreach (Product product in cartProducts)
            {
                int quantity = cart[product.ProductId];
                decimal lineTotal = product.DiscountedPrice * quantity;
                
                dgvCart.Rows.Add(
                    product.ProductId,
                    product.Name,
                    $"LKR {product.Price:F2}",
                    product.Discount.ToString("F2"),
                    $"LKR {product.DiscountedPrice:F2}",
                    quantity,
                    $"LKR {lineTotal:F2}"
                );
            }
        }

        private void CalculateTotals()
        {
            decimal subtotal = 0;
            decimal discountTotal = 0;
            
            foreach (Product product in cartProducts)
            {
                int quantity = cart[product.ProductId];
                subtotal += product.Price * quantity;
                discountTotal += (product.Price - product.DiscountedPrice) * quantity;
            }
            
            decimal grandTotal = subtotal - discountTotal;
            
            lblSubtotal.Text = $"LKR {subtotal:F2}";
            lblDiscount.Text = $"LKR {discountTotal:F2}";
            lblGrandTotal.Text = $"LKR {grandTotal:F2}";
        }

        private void DgvCart_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvCart.Columns["Quantity"].Index)
            {
                try
                {
                    int productId = Convert.ToInt32(dgvCart.Rows[e.RowIndex].Cells["ProductId"].Value);
                    int newQuantity = Convert.ToInt32(dgvCart.Rows[e.RowIndex].Cells["Quantity"].Value);
                    
                    if (newQuantity <= 0)
                    {
                        MessageBox.Show("Quantity must be greater than 0.", 
                            "Invalid Quantity", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Warning);
                        dgvCart.Rows[e.RowIndex].Cells["Quantity"].Value = cart[productId];
                        return;
                    }
                    
                    Product product = cartProducts.Find(p => p.ProductId == productId);
                    if (newQuantity > product.Stock)
                    {
                        MessageBox.Show($"Only {product.Stock} units available in stock.", 
                            "Insufficient Stock", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Warning);
                        dgvCart.Rows[e.RowIndex].Cells["Quantity"].Value = cart[productId];
                        return;
                    }
                    
                    cart[productId] = newQuantity;
                    decimal lineTotal = product.DiscountedPrice * newQuantity;
                    dgvCart.Rows[e.RowIndex].Cells["LineTotal"].Value = $"LKR {lineTotal:F2}";
                    
                    CalculateTotals();
                }
                catch
                {
                    // Invalid input, revert
                }
            }
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            if (dgvCart.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an item to remove.", 
                    "No Selection", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                return;
            }
            
            int productId = Convert.ToInt32(dgvCart.SelectedRows[0].Cells["ProductId"].Value);
            string productName = dgvCart.SelectedRows[0].Cells["Name"].Value.ToString();
            
            DialogResult result = MessageBox.Show($"Remove '{productName}' from cart?", 
                "Confirm Remove", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                cart.Remove(productId);
                LoadCartItems();
            }
        }

        private void BtnClearCart_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Clear all items from cart?", 
                "Confirm Clear Cart", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Warning);
            
            if (result == DialogResult.Yes)
            {
                cart.Clear();
                LoadCartItems();
            }
        }

        private void BtnCheckout_Click(object sender, EventArgs e)
        {
            if (cart.Count == 0)
            {
                MessageBox.Show("Your cart is empty!", 
                    "Empty Cart", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                return;
            }
            
            // Calculate totals
            decimal subtotal = 0;
            decimal discountTotal = 0;
            
            foreach (Product product in cartProducts)
            {
                int quantity = cart[product.ProductId];
                subtotal += product.Price * quantity;
                discountTotal += (product.Price - product.DiscountedPrice) * quantity;
            }
            
            decimal grandTotal = subtotal - discountTotal;
            
            string summary = $"Order Summary:\n\n" +
                           $"Subtotal: LKR {subtotal:F2}\n" +
                           $"Discount: -LKR {discountTotal:F2}\n" +
                           $"Grand Total: LKR {grandTotal:F2}\n\n" +
                           $"Proceed with checkout?";
            
            DialogResult result = MessageBox.Show(summary, 
                "Confirm Order", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                try
                {
                    // Create order
                    Order order = new Order
                    {
                        CustomerId = currentCustomer.CustomerId,
                        CustomerName = currentCustomer.FullName,
                        OrderDate = DateTime.Now,
                        Status = "Pending",
                        Subtotal = subtotal,
                        DiscountTotal = discountTotal,
                        GrandTotal = grandTotal
                    };

                    // Create order items
                    List<OrderItem> orderItems = new List<OrderItem>();
                    foreach (Product product in cartProducts)
                    {
                        int quantity = cart[product.ProductId];
                        OrderItem item = new OrderItem
                        {
                            ProductId = product.ProductId,
                            ProductName = product.Name,
                            Quantity = quantity,
                            UnitPrice = product.Price,
                            Discount = product.Discount,
                            LineTotal = product.DiscountedPrice * quantity
                        };
                        orderItems.Add(item);
                    }

                    // Process order (this will update stock automatically)
                    int orderId = orderService.CreateOrder(order, orderItems);

                    // Send order confirmation email (non-blocking)
                    try
                    {
                        emailService.SendOrderConfirmation(
                            currentCustomer.Email,
                            currentCustomer.FullName,
                            orderId, grandTotal, orderItems.Count);
                    }
                    catch { }

                    MessageBox.Show($"Order placed successfully!\n\nOrder ID: {orderId}\nTotal: LKR {grandTotal:F2}\n\nThank you for shopping with GreenLife!", 
                        "Order Placed", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
                    
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Order processing failed: {ex.Message}\n\nPlease try again or contact support.", 
                        "Order Error", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
                }
            }
        }
    }
}
