using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using GreenLifeWinForms.Services;

namespace GreenLifeWinForms.Forms.Admin
{
    public partial class ReportsForm : Form
    {
        private readonly OrderService orderService;
        private readonly ProductService productService;
        private readonly CustomerService customerService;

        private Button btnSalesTab;
        private Button btnStockTab;
        private Button btnCustomerTab;
        private Panel pnlTabs;
        private Panel pnlCard;
        private DataGridView dgvSales;
        private Chart chartSales;
        private Button btnExportCsv;
        private Button btnExportPdf;
        private Button btnPreview;
        private Label lblCardTitle;
        private int printPageIndex;
        private int printPageNumber;

        private enum ReportTab
        {
            Sales,
            Stock,
            Customers
        }

        public ReportsForm()
        {
            orderService = new OrderService();
            productService = new ProductService();
            customerService = new CustomerService();
            InitializeComponent();
            ActivateTab(ReportTab.Sales);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Reports - GreenLife";
            this.Size = new Size(1100, 750);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 253, 244);

            Panel header = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(this.ClientSize.Width, 120),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            header.Paint += (s, e) =>
            {
                using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(header.ClientRectangle,
                    Color.FromArgb(248, 255, 248), Color.FromArgb(230, 252, 232), 0f))
                {
                    e.Graphics.FillRectangle(brush, header.ClientRectangle);
                }
            };
            this.Controls.Add(header);

            Label lblTitle = new Label
            {
                Text = "Reports",
                Font = new Font("Segoe UI", 26, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 163, 74),
                AutoSize = true,
                Location = new Point(30, 35)
            };
            header.Controls.Add(lblTitle);

            Button btnBack = new Button
            {
                Text = "<< Back to Dashboard",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(190, 38),
                Location = new Point(880, 35),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand
            };
            btnBack.FlatAppearance.BorderColor = Color.LightGray;
            btnBack.Click += (s, e) => this.Close();
            header.Controls.Add(btnBack);

            pnlTabs = new Panel
            {
                Location = new Point(30, 130),
                Size = new Size(600, 40)
            };
            this.Controls.Add(pnlTabs);

            btnSalesTab = CreateTabButton("Sales Report", 0, (s, e) => ActivateTab(ReportTab.Sales));
            btnStockTab = CreateTabButton("Stock Report", 150, (s, e) => ActivateTab(ReportTab.Stock));
            btnCustomerTab = CreateTabButton("Customer History", 300, (s, e) => ActivateTab(ReportTab.Customers));
            pnlTabs.Controls.AddRange(new Control[] { btnSalesTab, btnStockTab, btnCustomerTab });

            pnlCard = new Panel
            {
                Location = new Point(30, 180),
                Size = new Size(1025, 520),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(25)
            };
            this.Controls.Add(pnlCard);

            lblCardTitle = new Label
            {
                Text = "Sales by Date",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 163, 74),
                AutoSize = true,
                Location = new Point(0, 0)
            };
            pnlCard.Controls.Add(lblCardTitle);

            btnExportCsv = new Button
            {
                Text = "Export CSV",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(130, 34),
                Location = new Point(760, 0),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnExportCsv.FlatAppearance.BorderColor = Color.LightGray;
            btnExportCsv.Click += BtnExportCsv_Click;
            pnlCard.Controls.Add(btnExportCsv);

            btnExportPdf = new Button
            {
                Text = "Export PDF",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(130, 34),
                Location = new Point(900, 0),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnExportPdf.FlatAppearance.BorderColor = Color.LightGray;
            btnExportPdf.Click += BtnExportPdf_Click;
            pnlCard.Controls.Add(btnExportPdf);

            btnPreview = new Button
            {
                Text = "Preview",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(100, 34),
                Location = new Point(650, 0),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnPreview.FlatAppearance.BorderColor = Color.LightGray;
            btnPreview.Click += BtnPreview_Click;
            pnlCard.Controls.Add(btnPreview);

            chartSales = new Chart
            {
                Size = new Size(960, 250),
                Location = new Point(0, 40)
            };
            ChartArea area = new ChartArea("SalesArea");
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 9);
            area.AxisY.MajorGrid.LineColor = Color.FromArgb(230, 230, 230);
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 9);
            chartSales.ChartAreas.Add(area);
            Series revenueSeries = new Series("Revenue")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.FromArgb(22, 163, 74),
                BorderWidth = 0,
                XValueType = ChartValueType.String,
                YValueType = ChartValueType.Double
            };
            Series ordersSeries = new Series("Orders")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.FromArgb(234, 179, 8),
                BorderWidth = 3,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 8,
                XValueType = ChartValueType.String,
                YValueType = ChartValueType.Int32
            };
            chartSales.Series.Add(revenueSeries);
            chartSales.Series.Add(ordersSeries);
            pnlCard.Controls.Add(chartSales);

            dgvSales = new DataGridView
            {
                Location = new Point(0, 305),
                Size = new Size(960, 180),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                EnableHeadersVisualStyles = false,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(220, 252, 231)
            };
            dgvSales.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dgvSales.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            dgvSales.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(20, 83, 45);
            dgvSales.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvSales.DefaultCellStyle.SelectionBackColor = Color.FromArgb(240, 253, 244);
            dgvSales.DefaultCellStyle.SelectionForeColor = Color.FromArgb(20, 83, 45);
            pnlCard.Controls.Add(dgvSales);

            this.ResumeLayout(false);
        }

        private Button CreateTabButton(string text, int left, EventHandler onClick)
        {
            Button btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(140, 35),
                Location = new Point(left, 0),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(20, 83, 45),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += onClick;
            return btn;
        }

        private void ActivateTab(ReportTab tab)
        {
            btnSalesTab.BackColor = Color.White;
            btnStockTab.BackColor = Color.White;
            btnCustomerTab.BackColor = Color.White;
            btnSalesTab.ForeColor = Color.FromArgb(20, 83, 45);
            btnStockTab.ForeColor = Color.FromArgb(20, 83, 45);
            btnCustomerTab.ForeColor = Color.FromArgb(20, 83, 45);

            switch (tab)
            {
                case ReportTab.Sales:
                    btnSalesTab.BackColor = Color.FromArgb(22, 163, 74);
                    btnSalesTab.ForeColor = Color.White;
                    lblCardTitle.Text = "Sales by Date";
                    LoadSalesReport();
                    break;
                case ReportTab.Stock:
                    btnStockTab.BackColor = Color.FromArgb(22, 163, 74);
                    btnStockTab.ForeColor = Color.White;
                    lblCardTitle.Text = "Stock Report";
                    LoadStockReport();
                    break;
                case ReportTab.Customers:
                    btnCustomerTab.BackColor = Color.FromArgb(22, 163, 74);
                    btnCustomerTab.ForeColor = Color.White;
                    lblCardTitle.Text = "Customer History";
                    LoadCustomerReport();
                    break;
            }
        }

        private void LoadSalesReport()
        {
            var rawOrders = orderService.GetAllOrders();
            if (rawOrders.Count == 0)
            {
                chartSales.Series["Revenue"].Points.Clear();
                chartSales.Series["Orders"].Points.Clear();
                dgvSales.Columns.Clear();
                dgvSales.Rows.Clear();
                return;
            }
            var summaries = rawOrders
                .GroupBy(o => o.OrderDate.Date)
                .Select(g => new SalesSummary
                {
                    Date = g.Key,
                    Orders = g.Count(),
                    Revenue = g.Sum(o => o.GrandTotal)
                })
                .OrderBy(g => g.Date)
                .ToList();

            chartSales.Series["Revenue"].Enabled = true;
            chartSales.Series["Orders"].Enabled = true;
            chartSales.Series["Revenue"].Points.Clear();
            chartSales.Series["Orders"].Points.Clear();

            foreach (var item in summaries)
            {
                chartSales.Series["Revenue"].Points.AddXY(item.Date.ToString("yyyy-MM-dd"), (double)item.Revenue);
                chartSales.Series["Orders"].Points.AddXY(item.Date.ToString("yyyy-MM-dd"), item.Orders);
            }

            dgvSales.Columns.Clear();
            dgvSales.Rows.Clear();
            dgvSales.Columns.Add("Date", "Date");
            dgvSales.Columns.Add("Orders", "Orders");
            dgvSales.Columns.Add("Revenue", "Revenue");

            foreach (var item in summaries)
            {
                dgvSales.Rows.Add(item.Date.ToString("yyyy-MM-dd"), item.Orders, $"LKR {item.Revenue:F2}");
            }
        }

        private void LoadStockReport()
        {
            var products = productService.GetAllProducts()
                .OrderBy(p => p.Stock)
                .Select(p => new StockSummary
                {
                    Name = p.Name,
                    Category = p.Category,
                    Stock = p.Stock
                })
                .ToList();

            if (products.Count == 0)
            {
                chartSales.Series["Revenue"].Points.Clear();
                chartSales.Series["Orders"].Points.Clear();
                dgvSales.Columns.Clear();
                dgvSales.Rows.Clear();
                return;
            }

            chartSales.Series["Revenue"].Enabled = true;
            chartSales.Series["Orders"].Enabled = false;
            chartSales.Series["Revenue"].Points.Clear();
            chartSales.Series["Orders"].Points.Clear();

            foreach (var product in products.Take(12))
            {
                chartSales.Series["Revenue"].Points.AddXY(product.Name, product.Stock);
            }

            dgvSales.Columns.Clear();
            dgvSales.Rows.Clear();
            dgvSales.Columns.Add("Product", "Product");
            dgvSales.Columns.Add("Category", "Category");
            dgvSales.Columns.Add("Stock", "Stock");

            foreach (var product in products)
            {
                dgvSales.Rows.Add(product.Name, product.Category, product.Stock);
            }
        }

        private void LoadCustomerReport()
        {
            var customers = customerService.GetAllCustomers();
            var orders = orderService.GetAllOrders();

            if (customers.Count == 0)
            {
                chartSales.Series["Revenue"].Points.Clear();
                chartSales.Series["Orders"].Points.Clear();
                dgvSales.Columns.Clear();
                dgvSales.Rows.Clear();
                return;
            }

            var summaries = customers.Select(c =>
            {
                var customerOrders = orders.Where(o => o.CustomerId == c.CustomerId).ToList();
                return new CustomerSummary
                {
                    Name = c.FullName,
                    Email = c.Email,
                    TotalOrders = customerOrders.Count,
                    TotalSpent = customerOrders.Sum(o => o.GrandTotal)
                };
            }).OrderByDescending(s => s.TotalSpent).ToList();

            chartSales.Series["Revenue"].Enabled = true;
            chartSales.Series["Orders"].Enabled = true;
            chartSales.Series["Revenue"].Points.Clear();
            chartSales.Series["Orders"].Points.Clear();

            foreach (var summary in summaries.Take(10))
            {
                chartSales.Series["Revenue"].Points.AddXY(summary.Name, (double)summary.TotalSpent);
                chartSales.Series["Orders"].Points.AddXY(summary.Name, summary.TotalOrders);
            }

            dgvSales.Columns.Clear();
            dgvSales.Rows.Clear();
            dgvSales.Columns.Add("Customer", "Customer");
            dgvSales.Columns.Add("Email", "Email");
            dgvSales.Columns.Add("Orders", "Orders");
            dgvSales.Columns.Add("Spent", "Spent");

            foreach (var summary in summaries)
            {
                dgvSales.Rows.Add(summary.Name, summary.Email, summary.TotalOrders, $"LKR {summary.TotalSpent:F2}");
            }
        }

        private void BtnExportCsv_Click(object sender, EventArgs e)
        {
            if (dgvSales.Rows.Count == 0)
            {
                MessageBox.Show("No data available to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "CSV Files (*.csv)|*.csv";
                dialog.FileName = $"GreenLife_Report_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(dialog.FileName, false, Encoding.UTF8))
                        {
                            var headers = dgvSales.Columns.Cast<DataGridViewColumn>().Select(c => c.HeaderText);
                            writer.WriteLine(string.Join(",", headers));

                            foreach (DataGridViewRow row in dgvSales.Rows)
                            {
                                if (!row.IsNewRow)
                                {
                                    var cells = row.Cells.Cast<DataGridViewCell>().Select(c => EscapeCsv(c.Value?.ToString() ?? string.Empty));
                                    writer.WriteLine(string.Join(",", cells));
                                }
                            }
                        }

                        if (MessageBox.Show("Report exported successfully. Open file now?", "Export",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            Process.Start(new ProcessStartInfo(dialog.FileName) { UseShellExecute = true });
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to export CSV: {ex.Message}", "Export Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnExportPdf_Click(object sender, EventArgs e)
        {
            if (dgvSales.Rows.Count == 0)
            {
                MessageBox.Show("No data available to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "PDF Files (*.pdf)|*.pdf";
                dialog.FileName = $"GreenLife_Report_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ExportToSimplePdf(dialog.FileName);

                        if (MessageBox.Show("PDF exported successfully. Open file now?", "Export",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            Process.Start(new ProcessStartInfo(dialog.FileName) { UseShellExecute = true });
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to export PDF: {ex.Message}", "Export Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnPreview_Click(object sender, EventArgs e)
        {
            if (dgvSales.Rows.Count == 0)
            {
                MessageBox.Show("No data available to preview.", "Preview", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                printPageIndex = 0;
                printPageNumber = 0;

                PrintDocument printDoc = new PrintDocument();
                printDoc.DocumentName = $"GreenLife Report - {lblCardTitle.Text}";
                printDoc.PrintPage += PrintDoc_PrintPage;

                PrintPreviewDialog previewDialog = new PrintPreviewDialog();
                previewDialog.Document = printDoc;
                previewDialog.Width = 900;
                previewDialog.Height = 700;
                previewDialog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to generate preview: {ex.Message}", "Preview Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            float leftMargin = e.MarginBounds.Left;
            float topMargin = e.MarginBounds.Top;
            float y = topMargin;
            float pageWidth = e.MarginBounds.Width;
            float pageBottom = e.MarginBounds.Bottom;

            printPageNumber++;
            int colCount = dgvSales.Columns.Count;
            float colWidth = pageWidth / colCount;

            // Page header
            using (Font titleFont = new Font("Segoe UI", 16, FontStyle.Bold))
            {
                g.DrawString("GreenLife Organic Store", titleFont, Brushes.Green, leftMargin, y);
                y += 30;
            }

            using (Font subtitleFont = new Font("Segoe UI", 12, FontStyle.Bold))
            {
                g.DrawString(lblCardTitle.Text, subtitleFont, Brushes.DarkGreen, leftMargin, y);

                string pageLabel = $"Page {printPageNumber}";
                SizeF pageLabelSize = g.MeasureString(pageLabel, subtitleFont);
                g.DrawString(pageLabel, subtitleFont, Brushes.Gray,
                    leftMargin + pageWidth - pageLabelSize.Width, y);
                y += 25;
            }

            using (Font dateFont = new Font("Segoe UI", 9))
            {
                g.DrawString($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm}", dateFont, Brushes.Gray, leftMargin, y);
                y += 22;
            }

            // Separator line below header
            using (Pen headerLine = new Pen(Color.FromArgb(22, 163, 74), 1.5f))
            {
                g.DrawLine(headerLine, leftMargin, y, leftMargin + pageWidth, y);
            }
            y += 8;

            // Table column headers
            using (Font headerFont = new Font("Segoe UI", 10, FontStyle.Bold))
            using (Font cellFont = new Font("Segoe UI", 10))
            {
                using (SolidBrush headerBg = new SolidBrush(Color.FromArgb(22, 163, 74)))
                {
                    g.FillRectangle(headerBg, leftMargin, y, pageWidth, 25);
                }
                for (int c = 0; c < colCount; c++)
                {
                    g.DrawString(dgvSales.Columns[c].HeaderText, headerFont, Brushes.White,
                        leftMargin + c * colWidth + 5, y + 4);
                }
                y += 28;

                // Data rows
                // Reserve 80px at bottom for potential summary + footer
                float reservedBottom = pageBottom - 80;

                while (printPageIndex < dgvSales.Rows.Count && y + 22 <= reservedBottom)
                {
                    if (printPageIndex % 2 == 0)
                    {
                        using (SolidBrush altBg = new SolidBrush(Color.FromArgb(240, 253, 244)))
                        {
                            g.FillRectangle(altBg, leftMargin, y, pageWidth, 22);
                        }
                    }

                    for (int c = 0; c < colCount; c++)
                    {
                        string val = dgvSales.Rows[printPageIndex].Cells[c].Value?.ToString() ?? "";
                        g.DrawString(val, cellFont, Brushes.Black,
                            leftMargin + c * colWidth + 5, y + 3);
                    }
                    y += 22;
                    printPageIndex++;
                }

                bool morePages = printPageIndex < dgvSales.Rows.Count;
                e.HasMorePages = morePages;

                // Summary footer (last page only)
                if (!morePages && dgvSales.Rows.Count > 0)
                {
                    y += 6;
                    using (Pen linePen = new Pen(Color.FromArgb(22, 163, 74), 2))
                    {
                        g.DrawLine(linePen, leftMargin, y, leftMargin + pageWidth, y);
                    }
                    y += 8;

                    using (Font summaryFont = new Font("Segoe UI", 10, FontStyle.Bold))
                    {
                        g.DrawString($"Total Records: {dgvSales.Rows.Count}", summaryFont,
                            Brushes.Black, leftMargin, y);
                    }
                    y += 22;

                    // Compute grand total from the last column
                    int lastCol = colCount - 1;
                    if (lastCol >= 0)
                    {
                        decimal runningTotal = 0;
                        bool hasCurrency = false;
                        foreach (DataGridViewRow row in dgvSales.Rows)
                        {
                            string raw = row.Cells[lastCol].Value?.ToString() ?? "";
                            string cleaned = raw.Replace("LKR", "").Trim();
                            if (decimal.TryParse(cleaned, System.Globalization.NumberStyles.Any,
                                System.Globalization.CultureInfo.InvariantCulture, out decimal val))
                            {
                                runningTotal += val;
                                if (!hasCurrency) hasCurrency = raw.Contains("LKR");
                            }
                        }
                        if (runningTotal != 0)
                        {
                            string totalLabel = hasCurrency
                                ? $"Grand Total: LKR {runningTotal:N2}"
                                : $"Total {dgvSales.Columns[lastCol].HeaderText}: {runningTotal:N0}";

                            using (Font totalFont = new Font("Segoe UI", 11, FontStyle.Bold))
                            using (SolidBrush greenBrush = new SolidBrush(Color.FromArgb(22, 163, 74)))
                            {
                                g.DrawString(totalLabel, totalFont, greenBrush, leftMargin, y);
                            }
                        }
                    }
                }
            }

            // Page footer (every page)
            using (Font footerFont = new Font("Segoe UI", 8))
            {
                string footerText = $"GreenLife Organic Store  |  {lblCardTitle.Text}  |  Page {printPageNumber}  |  {DateTime.Now:yyyy-MM-dd HH:mm}";
                SizeF footerSize = g.MeasureString(footerText, footerFont);
                float footerX = leftMargin + (pageWidth - footerSize.Width) / 2;
                g.DrawString(footerText, footerFont, Brushes.Gray, footerX, pageBottom - 5);
            }
        }

        private void ExportToSimplePdf(string filePath)
        {
            // Collect all content lines
            List<string> allLines = new List<string>();
            allLines.Add(SanitizeForPdf("GreenLife Organic Store"));
            allLines.Add(SanitizeForPdf(lblCardTitle.Text));
            allLines.Add(SanitizeForPdf($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm}"));
            allLines.Add(SanitizeForPdf(new string('=', 80)));
            allLines.Add(string.Empty);

            var headers = dgvSales.Columns.Cast<DataGridViewColumn>().Select(c => c.HeaderText).ToList();
            string headerLine = string.Join("    ", headers);
            allLines.Add(SanitizeForPdf(headerLine));
            allLines.Add(SanitizeForPdf(new string('-', Math.Max(20, Math.Min(100, headerLine.Length)))));

            foreach (DataGridViewRow row in dgvSales.Rows)
            {
                if (!row.IsNewRow)
                {
                    var cells = row.Cells.Cast<DataGridViewCell>()
                        .Select(c => (c.Value?.ToString() ?? string.Empty).Replace("\r", " ").Replace("\n", " "));
                    allLines.Add(SanitizeForPdf(string.Join("    ", cells)));
                }
            }

            // Summary footer
            allLines.Add(string.Empty);
            allLines.Add(SanitizeForPdf(new string('=', 80)));
            allLines.Add(SanitizeForPdf($"Total Records: {dgvSales.Rows.Count}"));

            int lastCol = dgvSales.Columns.Count - 1;
            if (lastCol >= 0)
            {
                decimal runningTotal = 0;
                bool hasCurrency = false;
                foreach (DataGridViewRow row in dgvSales.Rows)
                {
                    string raw = row.Cells[lastCol].Value?.ToString() ?? "";
                    string cleaned = raw.Replace("LKR", "").Trim();
                    if (decimal.TryParse(cleaned, NumberStyles.Any,
                        CultureInfo.InvariantCulture, out decimal val))
                    {
                        runningTotal += val;
                        if (!hasCurrency) hasCurrency = raw.Contains("LKR");
                    }
                }
                if (runningTotal != 0)
                {
                    string totalLabel = hasCurrency
                        ? $"Grand Total: LKR {runningTotal:N2}"
                        : $"Total {dgvSales.Columns[lastCol].HeaderText}: {runningTotal:N0}";
                    allLines.Add(SanitizeForPdf(totalLabel));
                }
            }

            // Split lines into pages (~45 lines per A4 page at 16pt line height)
            int linesPerPage = 45;
            List<List<string>> pages = new List<List<string>>();
            for (int i = 0; i < allLines.Count; i += linesPerPage)
            {
                pages.Add(allLines.GetRange(i, Math.Min(linesPerPage, allLines.Count - i)));
            }
            if (pages.Count == 0)
                pages.Add(new List<string>());

            int pageCount = pages.Count;

            // Build PDF content stream for each page
            List<string> contentStreams = new List<string>();
            for (int p = 0; p < pageCount; p++)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("BT");
                sb.AppendLine("/F1 12 Tf");
                sb.AppendLine("1 0 0 1 50 780 Tm");
                foreach (string line in pages[p])
                {
                    sb.AppendLine($"({EscapePdfText(line)}) Tj");
                    sb.AppendLine("0 -16 Td");
                }
                sb.AppendLine("ET");
                // Page footer
                sb.AppendLine("BT");
                sb.AppendLine("/F1 8 Tf");
                sb.AppendLine("1 0 0 1 180 30 Tm");
                sb.AppendLine($"(Page {p + 1} of {pageCount}  |  GreenLife Organic Store) Tj");
                sb.AppendLine("ET");
                contentStreams.Add(sb.ToString());
            }

            // Write multi-page PDF file
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            using (StreamWriter writer = new StreamWriter(fs, Encoding.ASCII))
            {
                writer.WriteLine("%PDF-1.4");
                writer.Flush();

                List<long> offsets = new List<long>();
                int objNumber = 0;

                Action<string> WriteObject = (body) =>
                {
                    objNumber++;
                    writer.Flush();
                    offsets.Add(fs.Position);
                    writer.Write($"{objNumber} 0 obj\n{body}\nendobj\n");
                    writer.Flush();
                };

                // Object 1: Catalog
                WriteObject("<< /Type /Catalog /Pages 2 0 R >>");

                // Object 2: Pages
                string kidsRefs = string.Join(" ", Enumerable.Range(0, pageCount).Select(i => $"{3 + i * 2} 0 R"));
                WriteObject($"<< /Type /Pages /Kids [{kidsRefs}] /Count {pageCount} >>");

                // Font object number (after all page + content objects)
                int fontObjNum = 3 + pageCount * 2;

                // For each page: Page object + Content stream object
                for (int p = 0; p < pageCount; p++)
                {
                    int contentObjNum = 4 + p * 2;
                    int contentLen = Encoding.ASCII.GetByteCount(contentStreams[p]);

                    WriteObject($"<< /Type /Page /Parent 2 0 R /MediaBox [0 0 595 842] /Contents {contentObjNum} 0 R /Resources << /Font << /F1 {fontObjNum} 0 R >> >> >>");
                    WriteObject($"<< /Length {contentLen} >>\nstream\n{contentStreams[p]}\nendstream");
                }

                // Font object
                WriteObject("<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>");

                // Cross-reference table
                writer.Flush();
                long xrefPosition = fs.Position;
                writer.WriteLine("xref");
                writer.WriteLine($"0 {offsets.Count + 1}");
                writer.WriteLine("0000000000 65535 f ");
                foreach (long offset in offsets)
                {
                    writer.WriteLine(offset.ToString("0000000000") + " 00000 n ");
                }

                writer.WriteLine("trailer");
                writer.WriteLine($"<< /Size {offsets.Count + 1} /Root 1 0 R >>");
                writer.WriteLine("startxref");
                writer.WriteLine(xrefPosition);
                writer.WriteLine("%%EOF");
            }
        }

        private string EscapeCsv(string value)
        {
            if (value.Contains(",") || value.Contains("\""))
            {
                value = "\"" + value.Replace("\"", "\"\"") + "\"";
            }
            return value;
        }

        private string EscapePdfText(string value)
        {
            return value
                .Replace("\\", "\\\\")
                .Replace("(", "\\(")
                .Replace(")", "\\)");
        }

        private string SanitizeForPdf(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder(value.Length);
            foreach (char ch in value)
            {
                if (ch >= 32 && ch <= 126)
                {
                    builder.Append(ch);
                }
                else
                {
                    builder.Append('?');
                }
            }
            return builder.ToString();
        }

        private class SalesSummary
        {
            public DateTime Date { get; set; }
            public int Orders { get; set; }
            public decimal Revenue { get; set; }
        }

        private class StockSummary
        {
            public string Name { get; set; }
            public string Category { get; set; }
            public int Stock { get; set; }
        }

        private class CustomerSummary
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public int TotalOrders { get; set; }
            public decimal TotalSpent { get; set; }
        }
    }
}
