using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using GreenLifeWinForms.Models;
using GreenLifeWinForms.Services;

namespace GreenLifeWinForms.Forms.Customer
{
    public partial class BrowseProductsForm : Form
    {
        // ── Fields ────────────────────────────────────────────────────────────
        private readonly ProductService productService;
        private readonly ReviewService reviewService;
        private readonly Models.Customer currentCustomer;
        private readonly Dictionary<int, int> cart;

        private List<Product> currentProducts;
        private FlowLayoutPanel flowProducts;
        private TextBox txtSearch;
        private ComboBox cmbCategory;
        private Label lblCartCount;

        private static readonly string ImagesFolder =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

        // ── Category → (emoji, pastel bg) ────────────────────────────────────
        private static readonly Dictionary<string, (string Emoji, Color Bg)> CategoryThemes
            = new Dictionary<string, (string, Color)>(StringComparer.OrdinalIgnoreCase)
            {
                { "Fruits",         ("🍎", Color.FromArgb(254, 202, 202)) },
                { "Vegetables",     ("🥦", Color.FromArgb(187, 247, 208)) },
                { "Dairy",          ("🧀", Color.FromArgb(254, 249, 195)) },
                { "Grains",         ("🌾", Color.FromArgb(253, 230, 138)) },
                { "Beverages",      ("🥤", Color.FromArgb(191, 219, 254)) },
                { "Snacks",         ("🍿", Color.FromArgb(233, 213, 255)) },
                { "Meat & Poultry", ("🥩", Color.FromArgb(254, 215, 170)) },
                { "Spreads",        ("🍯", Color.FromArgb(252, 231,  77)) },
                { "Bakery",         ("🍞", Color.FromArgb(254, 243, 199)) },
                { "Seafood",        ("🐟", Color.FromArgb(186, 230, 253)) },
            };

        private static readonly (string Emoji, Color Bg) DefaultTheme
            = ("🌿", Color.FromArgb(209, 250, 229));

        // ── Constructor ───────────────────────────────────────────────────────
        public BrowseProductsForm(Models.Customer customer)
        {
            currentCustomer = customer;
            productService  = new ProductService();
            reviewService   = new ReviewService();
            cart            = new Dictionary<int, int>();
            currentProducts = new List<Product>();

            InitializeComponent();
            LoadProducts();
        }

        // ── InitializeComponent ───────────────────────────────────────────────
        private void InitializeComponent()
        {
            SuspendLayout();

            Text           = "Browse Products - GreenLife";
            Size           = new Size(1200, 800);
            StartPosition  = FormStartPosition.CenterScreen;
            BackColor      = Color.FromArgb(240, 253, 244);
            DoubleBuffered = true;

            Controls.Add(new Label
            {
                Text      = "🛒 Browse Our Organic Products",
                Font      = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 163, 74),
                AutoSize  = true,
                Location  = new Point(20, 20)
            });

            // ── Search panel ──────────────────────────────────────────────────
            Panel searchPanel = new Panel
            {
                Location  = new Point(20, 70),
                Size      = new Size(1150, 60),
                BackColor = Color.White
            };
            Controls.Add(searchPanel);

            searchPanel.Controls.Add(new Label
            {
                Text     = "Search:",
                Font     = new Font("Segoe UI", 10F),
                Location = new Point(10, 20),
                AutoSize = true
            });

            txtSearch = new TextBox
            {
                Font     = new Font("Segoe UI", 10F),
                Size     = new Size(300, 25),
                Location = new Point(70, 17)
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;
            searchPanel.Controls.Add(txtSearch);

            searchPanel.Controls.Add(new Label
            {
                Text     = "Category:",
                Font     = new Font("Segoe UI", 10F),
                Location = new Point(390, 20),
                AutoSize = true
            });

            cmbCategory = new ComboBox
            {
                Font          = new Font("Segoe UI", 10F),
                Size          = new Size(200, 25),
                Location      = new Point(470, 17),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCategory.SelectedIndexChanged += CmbCategory_SelectedIndexChanged;
            searchPanel.Controls.Add(cmbCategory);

            Button btnViewCart = new Button
            {
                Text      = "🛍️ View Cart",
                Font      = new Font("Segoe UI", 11F, FontStyle.Bold),
                Size      = new Size(150, 40),
                Location  = new Point(880, 10),
                BackColor = Color.FromArgb(251, 191, 36),
                ForeColor = Color.FromArgb(20, 83, 45),
                FlatStyle = FlatStyle.Flat,
                Cursor    = Cursors.Hand
            };
            btnViewCart.FlatAppearance.BorderSize = 0;
            btnViewCart.Click += BtnViewCart_Click;
            searchPanel.Controls.Add(btnViewCart);

            lblCartCount = new Label
            {
                Text      = "0",
                Font      = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(239, 68, 68),
                Size      = new Size(25, 25),
                Location  = new Point(1015, 8),
                TextAlign = ContentAlignment.MiddleCenter
            };
            searchPanel.Controls.Add(lblCartCount);

            // ── Products flow ─────────────────────────────────────────────────
            flowProducts = new FlowLayoutPanel
            {
                Location     = new Point(20, 150),
                Size         = new Size(1150, 570),
                BackColor    = Color.White,
                AutoScroll   = true,
                WrapContents = true,
                Padding      = new Padding(10)
            };
            Controls.Add(flowProducts);

            Button btnClose = new Button
            {
                Text      = "Close",
                Font      = new Font("Segoe UI", 11F),
                Size      = new Size(120, 40),
                Location  = new Point(1050, 730),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor    = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => Close();
            Controls.Add(btnClose);

            ResumeLayout(false);
            PerformLayout();
        }

        // ── Data ──────────────────────────────────────────────────────────────
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
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCategories()
        {
            try
            {
                cmbCategory.Items.Clear();
                cmbCategory.Items.Add("All");
                foreach (string cat in productService.GetProductCategories())
                    cmbCategory.Items.Add(cat);
                if (cmbCategory.Items.Count > 0)
                    cmbCategory.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayProducts(List<Product> products)
        {
            flowProducts.SuspendLayout();
            flowProducts.Controls.Clear();

            if (products == null || products.Count == 0)
            {
                flowProducts.Controls.Add(new Label
                {
                    Text      = "No products found.",
                    Font      = new Font("Segoe UI", 12F, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    AutoSize  = true,
                    Margin    = new Padding(20)
                });
            }
            else
            {
                foreach (Product product in products)
                    flowProducts.Controls.Add(CreateProductCard(product));
            }

            flowProducts.ResumeLayout();
        }

        // ── Card builder ──────────────────────────────────────────────────────
        private Panel CreateProductCard(Product product)
        {
            (string emoji, Color bg) = GetTheme(product.Category);
            bool noStock  = product.Stock <= 0;
            bool lowStock = !noStock && product.IsLowStock;

            Panel card = new Panel
            {
                Size        = new Size(260, 460),
                BackColor   = Color.FromArgb(240, 253, 244),
                BorderStyle = BorderStyle.FixedSingle,
                Margin      = new Padding(10)
            };

            // ── Image: real file if present, painted mock otherwise ───────────
            Image productImage = TryLoadProductImage(product.Image);

            string capturedEmoji = emoji;
            Color  capturedBg    = bg;
            string capturedName  = product.Name;
            string capturedCat   = product.Category;

            if (productImage != null)
            {
                PictureBox pic = new PictureBox
                {
                    Location  = new Point(0, 0),
                    Size      = new Size(258, 140),
                    SizeMode  = PictureBoxSizeMode.Zoom,
                    BackColor = bg,
                    Image     = productImage
                };
                card.Controls.Add(pic);
            }
            else
            {
                Panel imgPanel = new Panel
                {
                    Location  = new Point(0, 0),
                    Size      = new Size(258, 140),
                    BackColor = capturedBg
                };
                imgPanel.Paint += (s, e) =>
                    DrawMockImage(e.Graphics, imgPanel.Size,
                                  capturedEmoji, capturedBg,
                                  capturedName,  capturedCat);
                card.Controls.Add(imgPanel);
            }

            // ── Discount badge ────────────────────────────────────────────────
            if (product.Discount > 0)
            {
                Label badge = new Label
                {
                    Text      = $"{product.Discount:0}% OFF",
                    Font      = new Font("Segoe UI", 8F, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(239, 68, 68),
                    Size      = new Size(72, 22),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location  = new Point(258 - 76, 8)
                };
                card.Controls.Add(badge);
                badge.BringToFront();
            }

            int top = 148;

            // Name
            card.Controls.Add(new Label
            {
                Text      = product.Name,
                Font      = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(20, 83, 45),
                Size      = new Size(240, 44),
                Location  = new Point(10, top),
                TextAlign = ContentAlignment.TopCenter
            });
            top += 48;

            // Category
            card.Controls.Add(new Label
            {
                Text      = product.Category,
                Font      = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                AutoSize  = true,
                Location  = new Point(10, top)
            });
            top += 22;

            // ── Star rating row ───────────────────────────────────────────────
            decimal avgRating = 0;
            int reviewCount = 0;
            try
            {
                avgRating   = reviewService.GetAverageRating(product.ProductId);
                reviewCount = reviewService.GetReviewCount(product.ProductId);
            }
            catch { }

            string stars = BuildStarString(avgRating);
            Color  goldColor = Color.FromArgb(234, 179, 8);
            Color  grayColor = Color.FromArgb(209, 213, 219);

            // Star label
            Label lblStars = new Label
            {
                Text      = stars,
                Font      = new Font("Segoe UI", 14F),
                ForeColor = goldColor,
                AutoSize  = true,
                Location  = new Point(10, top)
            };
            card.Controls.Add(lblStars);

            // Rating text beside stars
            string ratingText = reviewCount > 0
                ? $"{avgRating:F1} ({reviewCount})"
                : "No reviews";
            Color ratingColor = reviewCount > 0 ? goldColor : grayColor;

            card.Controls.Add(new Label
            {
                Text      = ratingText,
                Font      = new Font("Segoe UI", 9F),
                ForeColor = ratingColor,
                AutoSize  = true,
                Location  = new Point(lblStars.PreferredWidth + 14, top + 4)
            });
            top += 28;

            // Price
            decimal displayPrice = product.Discount > 0 ? product.DiscountedPrice : product.Price;
            Color   priceColor   = product.Discount > 0
                                   ? Color.FromArgb(239, 68, 68)
                                   : Color.FromArgb(22, 163, 74);

            card.Controls.Add(new Label
            {
                Text      = $"LKR {displayPrice:F2}",
                Font      = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = priceColor,
                AutoSize  = true,
                Location  = new Point(10, top)
            });
            top += 28;

            if (product.Discount > 0)
            {
                card.Controls.Add(new Label
                {
                    Text      = $"LKR {product.Price:F2}",
                    Font      = new Font("Segoe UI", 10F, FontStyle.Strikeout),
                    ForeColor = Color.Gray,
                    AutoSize  = true,
                    Location  = new Point(10, top)
                });
                top += 22;
            }

            // Stock
            string stockTxt = noStock  ? "Out of Stock"
                            : lowStock ? $"Low Stock ({product.Stock})"
                                       : product.StockStatus;
            Color  stockClr = noStock  ? Color.FromArgb(239, 68, 68)
                            : lowStock ? Color.FromArgb(202, 138, 4)
                                       : Color.FromArgb(22, 163, 74);

            card.Controls.Add(new Label
            {
                Text      = stockTxt,
                Font      = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = stockClr,
                AutoSize  = true,
                Location  = new Point(10, top)
            });
            top += 26;

            // Qty
            Panel qtyPanel = new Panel { Location = new Point(10, top), Size = new Size(240, 32) };
            card.Controls.Add(qtyPanel);

            qtyPanel.Controls.Add(new Label
            {
                Text     = "Qty:",
                Font     = new Font("Segoe UI", 10F),
                AutoSize = true,
                Location = new Point(0, 6)
            });

            NumericUpDown numQty = new NumericUpDown
            {
                Font     = new Font("Segoe UI", 10F),
                Size     = new Size(80, 25),
                Location = new Point(40, 3),
                Minimum  = noStock ? 0 : 1,
                Maximum  = noStock ? 0 : product.Stock,
                Value    = noStock ? 0 : 1,
                Enabled  = !noStock
            };
            qtyPanel.Controls.Add(numQty);
            top += 40;

            // Add to Cart
            Button btnAddToCart = new Button
            {
                Text      = noStock ? "Out of Stock" : "🛒 Add to Cart",
                Font      = new Font("Segoe UI", 11F, FontStyle.Bold),
                Size      = new Size(240, 42),
                Location  = new Point(10, top),
                BackColor = noStock ? Color.FromArgb(209, 213, 219) : Color.FromArgb(22, 163, 74),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Enabled   = !noStock,
                Cursor    = noStock ? Cursors.No : Cursors.Hand,
                Tag       = new AddToCartContext(product, numQty)
            };
            btnAddToCart.FlatAppearance.BorderSize = 0;
            btnAddToCart.Click += BtnAddToCart_Click;
            card.Controls.Add(btnAddToCart);
            top += 48;

            // View Details
            Button btnDetails = new Button
            {
                Text      = "View Details",
                Font      = new Font("Segoe UI", 9F),
                Size      = new Size(240, 28),
                Location  = new Point(10, top),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor    = Cursors.Hand,
                Tag       = product
            };
            btnDetails.FlatAppearance.BorderSize = 0;
            btnDetails.Click += BtnDetails_Click;
            card.Controls.Add(btnDetails);

            return card;
        }

        // ── Image loading ─────────────────────────────────────────────────────
        private static Image TryLoadProductImage(string imageValue)
        {
            if (string.IsNullOrWhiteSpace(imageValue))
                return null;

            string[] candidates =
            {
                Path.Combine(ImagesFolder, imageValue),
                imageValue
            };

            foreach (string path in candidates)
            {
                if (!File.Exists(path))
                    continue;
                try
                {
                    byte[] bytes = File.ReadAllBytes(path);
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes))
                        return new Bitmap(Image.FromStream(ms));
                }
                catch { }
            }

            return null;
        }

        // ── Mock image painter ────────────────────────────────────────────────
        private static void DrawMockImage(Graphics g, Size size,
                                          string emoji, Color bg,
                                          string name,  string category)
        {
            g.SmoothingMode     = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            using (SolidBrush bgBrush = new SolidBrush(bg))
                g.FillRectangle(bgBrush, 0, 0, size.Width, size.Height);

            using (Pen stripe = new Pen(Color.FromArgb(20, 0, 0, 0), 10))
                for (int x = -size.Height; x < size.Width + size.Height; x += 26)
                    g.DrawLine(stripe, x, 0, x + size.Height, size.Height);

            int cr = 50;
            int cx = (size.Width  - cr * 2) / 2;
            int cy = (size.Height - cr * 2) / 2 - 6;

            using (SolidBrush glow = new SolidBrush(Color.FromArgb(40, 255, 255, 255)))
                g.FillEllipse(glow, cx - 8, cy - 8, cr * 2 + 16, cr * 2 + 16);

            using (SolidBrush circle = new SolidBrush(Color.FromArgb(100, 255, 255, 255)))
                g.FillEllipse(circle, cx, cy, cr * 2, cr * 2);

            using (Font ef = new Font("Segoe UI Emoji", 32F, FontStyle.Regular, GraphicsUnit.Pixel))
            {
                SizeF es = g.MeasureString(emoji, ef);
                g.DrawString(emoji, ef, Brushes.White,
                    cx + (cr * 2 - es.Width)  / 2,
                    cy + (cr * 2 - es.Height) / 2);
            }

            int bannerH = 34;
            Rectangle bannerRect = new Rectangle(0, size.Height - bannerH, size.Width, bannerH);

            using (LinearGradientBrush bannerBrush = new LinearGradientBrush(
                bannerRect,
                Color.FromArgb(0,   0, 0, 0),
                Color.FromArgb(160, 0, 0, 0),
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(bannerBrush, bannerRect);
            }

            string label = (name ?? string.Empty).Length > 28
                ? name.Substring(0, 25) + "…"
                : name ?? string.Empty;

            using (Font lf = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Pixel))
            {
                SizeF ls = g.MeasureString(label, lf);
                g.DrawString(label, lf, Brushes.White,
                    (size.Width - ls.Width) / 2,
                    size.Height - bannerH + (bannerH - ls.Height) / 2);
            }
        }

        // ── Event handlers ────────────────────────────────────────────────────
        private void BtnAddToCart_Click(object sender, EventArgs e)
        {
            if (!(sender is Button btn) || !(btn.Tag is AddToCartContext ctx))
                return;

            int qty = (int)ctx.QuantityControl.Value;
            if (qty <= 0)
            {
                MessageBox.Show("Please select a quantity greater than zero.",
                    "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cart.ContainsKey(ctx.Product.ProductId))
                cart[ctx.Product.ProductId] += qty;
            else
                cart[ctx.Product.ProductId] = qty;

            UpdateCartBadge();
            MessageBox.Show($"Added {qty} x {ctx.Product.Name} to your cart.",
                "Added to Cart", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnDetails_Click(object sender, EventArgs e)
        {
            if (!(sender is Button btn) || !(btn.Tag is Product product))
                return;

            string details = $"Product: {product.Name}\n\n" +
                             $"Category: {product.Category}\n" +
                             $"Supplier: {product.Supplier}\n" +
                             $"Price: LKR {product.Price:F2}\n" +
                             $"Discount: {product.Discount}%\n" +
                             $"Final Price: LKR {product.DiscountedPrice:F2}\n" +
                             $"Stock: {product.Stock} units\n" +
                             $"Status: {product.StockStatus}\n\n" +
                             $"Description:\n{product.Description}";

            MessageBox.Show(details, "Product Details",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnViewCart_Click(object sender, EventArgs e)
        {
            if (cart.Count == 0)
            {
                MessageBox.Show("Your cart is empty!", "Empty Cart",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ShoppingCartForm cartForm = new ShoppingCartForm(currentCustomer, cart);
            cartForm.ShowDialog();

            if (cartForm.DialogResult == DialogResult.OK)
            {
                cart.Clear();
                UpdateCartBadge();
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
                string category   = cmbCategory.SelectedItem?.ToString() ?? "All";
                List<Product> results = productService.SearchProducts(searchTerm, category);
                DisplayProducts(results);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateCartBadge()
        {
            int total = 0;
            foreach (int qty in cart.Values)
                total += qty;
            lblCartCount.Text = total.ToString();
        }

        // ── Helpers ───────────────────────────────────────────────────────────
        private static (string Emoji, Color Bg) GetTheme(string category)
        {
            if (!string.IsNullOrWhiteSpace(category) &&
                CategoryThemes.TryGetValue(category.Trim(), out var t))
                return t;
            return DefaultTheme;
        }

        // ── Star string builder ──────────────────────────────────────────────
        private static string BuildStarString(decimal rating)
        {
            int fullStars = (int)Math.Floor(rating);
            bool halfStar = (rating - fullStars) >= 0.5m;
            int emptyStars = 5 - fullStars - (halfStar ? 1 : 0);

            return new string('★', fullStars)
                 + (halfStar ? "★" : "")
                 + new string('☆', emptyStars);
        }

        // ── Cart context ──────────────────────────────────────────────────────
        private sealed class AddToCartContext
        {
            public AddToCartContext(Product product, NumericUpDown qty)
            {
                Product         = product;
                QuantityControl = qty;
            }

            public Product       Product         { get; }
            public NumericUpDown QuantityControl { get; }
        }
    }
}
