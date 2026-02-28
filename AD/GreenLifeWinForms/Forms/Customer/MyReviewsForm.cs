using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using GreenLifeWinForms.Models;
using GreenLifeWinForms.Services;

namespace GreenLifeWinForms.Forms.Customer
{
    public partial class MyReviewsForm : Form
    {
        // ?? Services & state ??????????????????????????????????????????????????
        private readonly Models.Customer currentCustomer;
        private readonly ReviewService reviewService;
        private readonly ProductService productService;

        private List<Product> availableProducts;
        private List<Review>  myReviews;
        private int           selectedRating = 5;

        // ?? Controls ??????????????????????????????????????????????????????????
        private ComboBox      cmbProduct;
        private Label[]       starLabels;
        private Label         lblStarHint;
        private TextBox       txtComment;
        private Label         lblCharCount;
        private Button        btnSubmit;
        private Panel         reviewsPanel;
        private Label         lblReviewBadge;
        private Label         lblTotalReviews;
        private Label         lblAvgRating;
        private Label         lblAvailableToReview;

        // ?? Constructor ???????????????????????????????????????????????????????
        public MyReviewsForm(Models.Customer customer)
        {
            currentCustomer = customer;
            reviewService   = new ReviewService();
            productService  = new ProductService();

            InitializeComponent();
            LoadData();
        }

        // ?? InitializeComponent ???????????????????????????????????????????????
        private void InitializeComponent()
        {
            SuspendLayout();

            Text          = "My Reviews - GreenLife";
            Size          = new Size(1380, 820);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor     = Color.FromArgb(240, 249, 244);
            DoubleBuffered = true;

            // ?? Title ?????????????????????????????????????????????????????????
            Controls.Add(new Label
            {
                Text      = "My Reviews",
                Font      = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = Color.FromArgb(21, 128, 61),
                AutoSize  = true,
                Location  = new Point(30, 22)
            });

            // ?? Back button ???????????????????????????????????????????????????
            Button btnBack = new Button
            {
                Text      = "? Back",
                Font      = new Font("Segoe UI", 10F, FontStyle.Bold),
                Size      = new Size(90, 36),
                Location  = new Point(1260, 22),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(21, 128, 61),
                FlatStyle = FlatStyle.Flat,
                Cursor    = Cursors.Hand
            };
            btnBack.FlatAppearance.BorderColor = Color.FromArgb(21, 128, 61);
            btnBack.FlatAppearance.BorderSize  = 1;
            btnBack.Click += (s, e) => Close();
            Controls.Add(btnBack);

            // ?? TOP ROW: Write panel + My Reviews panel ???????????????????????
            Panel writePanel = BuildWritePanel();
            writePanel.Location = new Point(30, 80);
            Controls.Add(writePanel);

            Panel myReviewsPanel = BuildMyReviewsPanel();
            myReviewsPanel.Location = new Point(470, 80);
            Controls.Add(myReviewsPanel);

            // ?? BOTTOM ROW: Statistics panel ??????????????????????????????????
            Panel statsPanel = BuildStatsPanel();
            statsPanel.Location = new Point(30, 620);
            Controls.Add(statsPanel);

            ResumeLayout(false);
            PerformLayout();
        }

        // ?? Write a Review panel ??????????????????????????????????????????????
        private Panel BuildWritePanel()
        {
            Panel panel = MakeRoundedPanel(new Size(420, 520));

            // Header
            panel.Controls.Add(new Label
            {
                Text      = "?? Write a Review",
                Font      = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(21, 128, 61),
                AutoSize  = true,
                Location  = new Point(20, 20)
            });

            // Select Product label
            panel.Controls.Add(MakeFieldLabel("Select Product *", new Point(20, 62)));

            cmbProduct = new ComboBox
            {
                Location      = new Point(20, 84),
                Size          = new Size(380, 32),
                Font          = new Font("Segoe UI", 10F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor     = Color.FromArgb(245, 248, 245)
            };
            panel.Controls.Add(cmbProduct);

            // Rating label
            panel.Controls.Add(MakeFieldLabel("Your Rating *", new Point(20, 130)));

            // Star rating row
            starLabels = new Label[5];
            for (int i = 0; i < 5; i++)
            {
                int starIndex = i;
                Label star = new Label
                {
                    Text      = "?",
                    Font      = new Font("Segoe UI", 26F),
                    ForeColor = Color.FromArgb(251, 191, 36),
                    AutoSize  = true,
                    Location  = new Point(20 + i * 52, 154),
                    Cursor    = Cursors.Hand,
                    Tag       = i + 1
                };
                star.Click     += Star_Click;
                star.MouseEnter += (s, e) => HighlightStars((int)(((Label)s).Tag));
                star.MouseLeave += (s, e) => HighlightStars(selectedRating);
                starLabels[i] = star;
                panel.Controls.Add(star);
            }

            lblStarHint = new Label
            {
                Text      = "5 stars selected",
                Font      = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(107, 114, 128),
                AutoSize  = true,
                Location  = new Point(20, 194)
            };
            panel.Controls.Add(lblStarHint);

            // Review text label
            panel.Controls.Add(MakeFieldLabel("Your Review *", new Point(20, 224)));

            txtComment = new TextBox
            {
                Location    = new Point(20, 246),
                Size        = new Size(380, 120),
                Font        = new Font("Segoe UI", 10F),
                Multiline   = true,
                ScrollBars  = ScrollBars.Vertical,
                BackColor   = Color.FromArgb(245, 248, 245),
                BorderStyle = BorderStyle.FixedSingle
            };
            txtComment.TextChanged += TxtComment_TextChanged;
            panel.Controls.Add(txtComment);

            lblCharCount = new Label
            {
                Text      = "0 characters",
                Font      = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(156, 163, 175),
                AutoSize  = true,
                Location  = new Point(20, 372)
            };
            panel.Controls.Add(lblCharCount);

            // Submit button
            btnSubmit = new Button
            {
                Text      = "? Submit Review",
                Font      = new Font("Segoe UI", 12F, FontStyle.Bold),
                Size      = new Size(380, 48),
                Location  = new Point(20, 400),
                BackColor = Color.FromArgb(22, 163, 74),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor    = Cursors.Hand
            };
            btnSubmit.FlatAppearance.BorderSize = 0;
            btnSubmit.Click += BtnSubmit_Click;
            panel.Controls.Add(btnSubmit);

            return panel;
        }

        // ?? My Reviews list panel ?????????????????????????????????????????????
        private Panel BuildMyReviewsPanel()
        {
            Panel outer = MakeRoundedPanel(new Size(870, 520));

            // Header row
            outer.Controls.Add(new Label
            {
                Text      = "My Reviews",
                Font      = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(21, 128, 61),
                AutoSize  = true,
                Location  = new Point(20, 20)
            });

            lblReviewBadge = new Label
            {
                Text      = "0 Reviews",
                Font      = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 163, 74),
                BackColor = Color.FromArgb(209, 250, 229),
                Size      = new Size(80, 26),
                TextAlign = ContentAlignment.MiddleCenter,
                Location  = new Point(770, 18)
            };
            outer.Controls.Add(lblReviewBadge);

            // Scrollable reviews area
            reviewsPanel = new Panel
            {
                Location   = new Point(14, 56),
                Size       = new Size(840, 446),
                AutoScroll = true,
                BackColor  = Color.Transparent
            };
            outer.Controls.Add(reviewsPanel);

            return outer;
        }

        // ?? Statistics panel ??????????????????????????????????????????????????
        private Panel BuildStatsPanel()
        {
            Panel panel = MakeRoundedPanel(new Size(1310, 140));

            panel.Controls.Add(new Label
            {
                Text      = "Review Statistics",
                Font      = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(21, 128, 61),
                AutoSize  = true,
                Location  = new Point(20, 16)
            });

            // Total Reviews stat
            Panel statTotal = BuildStatCard("Total Reviews", out lblTotalReviews);
            statTotal.Location = new Point(20, 60);
            panel.Controls.Add(statTotal);

            // Average Rating stat
            Panel statAvg = BuildStatCard("Average Rating", out lblAvgRating);
            statAvg.Location = new Point(480, 60);
            panel.Controls.Add(statAvg);

            // Available to Review stat
            Panel statAvail = BuildStatCard("Available to Review", out lblAvailableToReview);
            statAvail.Location = new Point(940, 60);
            panel.Controls.Add(statAvail);

            return panel;
        }

        private Panel BuildStatCard(string caption, out Label valueLabel)
        {
            Panel card = new Panel
            {
                Size      = new Size(360, 60),
                BackColor = Color.FromArgb(240, 253, 244)
            };

            Label val = new Label
            {
                Text      = "—",
                Font      = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = Color.FromArgb(21, 128, 61),
                AutoSize  = true,
                Location  = new Point(160, 4)
            };
            card.Controls.Add(val);

            card.Controls.Add(new Label
            {
                Text      = caption,
                Font      = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(107, 114, 128),
                AutoSize  = true,
                Location  = new Point(120, 38)
            });

            valueLabel = val;
            return card;
        }

        // ?? Data loading ??????????????????????????????????????????????????????
        private void LoadData()
        {
            try
            {
                availableProducts = productService.GetAllProducts();
                cmbProduct.Items.Clear();
                cmbProduct.Items.Add(new ComboItem(0, "Choose a product"));
                foreach (Product p in availableProducts)
                {
                    cmbProduct.Items.Add(new ComboItem(p.ProductId, p.Name));
                }
                cmbProduct.SelectedIndex = 0;

                HighlightStars(selectedRating);
                RefreshReviews();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshReviews()
        {
            try
            {
                myReviews = reviewService.GetReviewsByCustomerId(currentCustomer.CustomerId);
                RenderReviewCards();
                RefreshStats();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading reviews: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RenderReviewCards()
        {
            reviewsPanel.SuspendLayout();
            reviewsPanel.Controls.Clear();

            lblReviewBadge.Text = $"{myReviews.Count} Review{(myReviews.Count == 1 ? "" : "s")}";

            if (myReviews.Count == 0)
            {
                reviewsPanel.Controls.Add(new Label
                {
                    Text      = "You haven't written any reviews yet.",
                    Font      = new Font("Segoe UI", 11F, FontStyle.Italic),
                    ForeColor = Color.FromArgb(156, 163, 175),
                    AutoSize  = true,
                    Location  = new Point(20, 20)
                });
                reviewsPanel.ResumeLayout();
                return;
            }

            int cardTop = 6;
            foreach (Review review in myReviews)
            {
                Panel card = BuildReviewCard(review);
                card.Location = new Point(4, cardTop);
                reviewsPanel.Controls.Add(card);
                cardTop += card.Height + 10;
            }

            reviewsPanel.ResumeLayout();
        }

        private Panel BuildReviewCard(Review review)
        {
            // Look up product name
            string productName = "Unknown Product";
            string category    = "";
            foreach (Product p in availableProducts)
            {
                if (p.ProductId == review.ProductId)
                {
                    productName = p.Name;
                    category    = p.Category;
                    break;
                }
            }

            Panel card = new Panel
            {
                Size        = new Size(826, 158),
                BackColor   = Color.White,
                BorderStyle = BorderStyle.None
            };
            card.Paint += ReviewCard_Paint;

            // Product name + date row
            card.Controls.Add(new Label
            {
                Text      = productName,
                Font      = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize  = true,
                Location  = new Point(16, 14)
            });

            card.Controls.Add(new Label
            {
                Text      = review.ReviewDate.ToString("yyyy-MM-dd"),
                Font      = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(156, 163, 175),
                AutoSize  = true,
                Location  = new Point(16, 38)
            });

            // Rating badge top-right
            Label ratingBadge = new Label
            {
                Text      = $"? {review.Rating}/5",
                Font      = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(180, 120, 10),
                BackColor = Color.FromArgb(254, 243, 199),
                Size      = new Size(68, 26),
                TextAlign = ContentAlignment.MiddleCenter,
                Location  = new Point(742, 14)
            };
            card.Controls.Add(ratingBadge);

            // Star row
            string stars = BuildStarString(review.Rating);
            card.Controls.Add(new Label
            {
                Text      = stars,
                Font      = new Font("Segoe UI", 18F),
                ForeColor = Color.FromArgb(251, 191, 36),
                AutoSize  = true,
                Location  = new Point(14, 58)
            });

            // Comment
            if (!string.IsNullOrWhiteSpace(review.Comment))
            {
                card.Controls.Add(new Label
                {
                    Text      = $"\"{review.Comment}\"",
                    Font      = new Font("Segoe UI", 10F, FontStyle.Italic),
                    ForeColor = Color.FromArgb(22, 163, 74),
                    AutoSize  = false,
                    Size      = new Size(720, 36),
                    Location  = new Point(16, 100)
                });
            }

            // Category chip
            if (!string.IsNullOrWhiteSpace(category))
            {
                Label chip = new Label
                {
                    Text      = category,
                    Font      = new Font("Segoe UI", 9F),
                    ForeColor = Color.FromArgb(75, 85, 99),
                    BackColor = Color.FromArgb(243, 244, 246),
                    AutoSize  = true,
                    Padding   = new Padding(6, 2, 6, 2),
                    Location  = new Point(16, 134)
                };
                card.Controls.Add(chip);
            }

            return card;
        }

        private void RefreshStats()
        {
            int totalReviews = myReviews.Count;
            lblTotalReviews.Text = totalReviews.ToString();

            if (totalReviews > 0)
            {
                double sum = 0;
                foreach (Review r in myReviews)
                {
                    sum += r.Rating;
                }
                lblAvgRating.Text = (sum / totalReviews).ToString("F1");
            }
            else
            {
                lblAvgRating.Text = "—";
            }

            try
            {
                int available = availableProducts != null ? availableProducts.Count : 0;
                lblAvailableToReview.Text = available.ToString();
            }
            catch
            {
                lblAvailableToReview.Text = "—";
            }
        }

        // ?? Event handlers ????????????????????????????????????????????????????
        private void Star_Click(object sender, EventArgs e)
        {
            selectedRating = (int)((Label)sender).Tag;
            HighlightStars(selectedRating);
        }

        private void TxtComment_TextChanged(object sender, EventArgs e)
        {
            lblCharCount.Text = $"{txtComment.Text.Length} characters";
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (!(cmbProduct.SelectedItem is ComboItem item) || item.Id == 0)
            {
                MessageBox.Show("Please select a product to review.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtComment.Text))
            {
                MessageBox.Show("Please write a review comment.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtComment.Focus();
                return;
            }

            try
            {
                if (reviewService.HasCustomerReviewedProduct(currentCustomer.CustomerId, item.Id))
                {
                    MessageBox.Show("You have already reviewed this product.",
                        "Already Reviewed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Review review = new Review
                {
                    ProductId    = item.Id,
                    CustomerId   = currentCustomer.CustomerId,
                    CustomerName = currentCustomer.FullName,
                    Rating       = selectedRating,
                    Comment      = txtComment.Text.Trim(),
                    ReviewDate   = DateTime.Now
                };

                if (reviewService.AddReview(review))
                {
                    MessageBox.Show("Review submitted successfully! Thank you.",
                        "Review Submitted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    cmbProduct.SelectedIndex = 0;
                    txtComment.Clear();
                    selectedRating = 5;
                    HighlightStars(selectedRating);
                    RefreshReviews();
                }
                else
                {
                    MessageBox.Show("Failed to submit review. Please try again.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting review: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ?? Helpers ???????????????????????????????????????????????????????????
        private void HighlightStars(int count)
        {
            for (int i = 0; i < 5; i++)
            {
                starLabels[i].ForeColor = i < count
                    ? Color.FromArgb(251, 191, 36)
                    : Color.FromArgb(209, 213, 219);
            }
            lblStarHint.Text = $"{count} star{(count == 1 ? "" : "s")} selected";
        }

        private static string BuildStarString(int rating)
        {
            return new string('?', rating) + new string('?', 5 - rating);
        }

        private static Label MakeFieldLabel(string text, Point location)
        {
            return new Label
            {
                Text      = text,
                Font      = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(55, 65, 81),
                AutoSize  = true,
                Location  = location
            };
        }

        private static Panel MakeRoundedPanel(Size size)
        {
            Panel panel = new Panel
            {
                Size      = size,
                BackColor = Color.White
            };
            panel.Paint += RoundedPanel_Paint;
            return panel;
        }

        private static void RoundedPanel_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = (Panel)sender;
            Rectangle r = panel.ClientRectangle;
            r.Width  -= 1;
            r.Height -= 1;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath path = RoundedPath(r, 16))
            using (Pen pen = new Pen(Color.FromArgb(209, 240, 215), 1.5F))
            {
                e.Graphics.DrawPath(pen, path);
            }
        }

        private static void ReviewCard_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = (Panel)sender;
            Rectangle r = panel.ClientRectangle;
            r.Width  -= 1;
            r.Height -= 1;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath path = RoundedPath(r, 12))
            {
                using (SolidBrush fill = new SolidBrush(Color.White))
                {
                    e.Graphics.FillPath(fill, path);
                }
                using (Pen pen = new Pen(Color.FromArgb(226, 232, 240), 1F))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        private static GraphicsPath RoundedPath(Rectangle r, int radius)
        {
            int d = radius * 2;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(r.X,         r.Y,          d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y,          d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d,   0, 90);
            path.AddArc(r.X,         r.Bottom - d, d, d,  90, 90);
            path.CloseFigure();
            return path;
        }

        // ?? ComboBox item wrapper ?????????????????????????????????????????????
        private sealed class ComboItem
        {
            public ComboItem(int id, string name) { Id = id; Name = name; }
            public int    Id   { get; }
            public string Name { get; }
            public override string ToString() => Name;
        }
    }
}
