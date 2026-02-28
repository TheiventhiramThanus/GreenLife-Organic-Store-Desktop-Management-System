using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace GreenLifeWinForms.Utils
{
    public static class ExportHelper
    {
        /// <summary>
        /// Export DataGridView to CSV file
        /// </summary>
        public static void ExportToCSV(DataGridView dataGridView, string defaultFileName = "export")
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                saveFileDialog.FileName = $"{defaultFileName}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                saveFileDialog.Title = "Export to CSV";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder sb = new StringBuilder();

                    // Add headers
                    List<string> headers = new List<string>();
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        if (column.Visible)
                        {
                            headers.Add(EscapeCSV(column.HeaderText));
                        }
                    }
                    sb.AppendLine(string.Join(",", headers));

                    // Add rows
                    foreach (DataGridViewRow row in dataGridView.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            List<string> cells = new List<string>();
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                if (cell.OwningColumn.Visible)
                                {
                                    string value = cell.Value?.ToString() ?? "";
                                    cells.Add(EscapeCSV(value));
                                }
                            }
                            sb.AppendLine(string.Join(",", cells));
                        }
                    }

                    File.WriteAllText(saveFileDialog.FileName, sb.ToString(), Encoding.UTF8);

                    MessageBox.Show($"Data exported successfully to:\n{saveFileDialog.FileName}", 
                        "Export Successful", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);

                    // Ask if user wants to open the file
                    DialogResult openResult = MessageBox.Show("Do you want to open the exported file?", 
                        "Open File", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Question);

                    if (openResult == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(saveFileDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to CSV: {ex.Message}", 
                    "Export Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Export DataGridView to PDF file
        /// </summary>
        public static void ExportToPDF(DataGridView dataGridView, string title, string defaultFileName = "export")
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveFileDialog.FileName = $"{defaultFileName}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                saveFileDialog.Title = "Export to PDF";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Create document
                    Document document = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
                    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(saveFileDialog.FileName, FileMode.Create));
                    
                    document.Open();

                    // Add title
                    iTextSharp.text.Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                    Paragraph titleParagraph = new Paragraph(title, titleFont);
                    titleParagraph.Alignment = Element.ALIGN_CENTER;
                    titleParagraph.SpacingAfter = 20;
                    document.Add(titleParagraph);

                    // Add date
                    iTextSharp.text.Font dateFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                    Paragraph dateParagraph = new Paragraph($"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm:ss}", dateFont);
                    dateParagraph.Alignment = Element.ALIGN_RIGHT;
                    dateParagraph.SpacingAfter = 20;
                    document.Add(dateParagraph);

                    // Count visible columns
                    int visibleColumnCount = 0;
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        if (column.Visible)
                            visibleColumnCount++;
                    }

                    // Create table
                    PdfPTable table = new PdfPTable(visibleColumnCount);
                    table.WidthPercentage = 100;

                    // Add headers
                    iTextSharp.text.Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.WHITE);
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        if (column.Visible)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, headerFont));
                            cell.BackgroundColor = new BaseColor(22, 163, 74); // Green
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                    }

                    // Add rows
                    iTextSharp.text.Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);
                    bool alternateRow = false;
                    foreach (DataGridViewRow row in dataGridView.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                if (cell.OwningColumn.Visible)
                                {
                                    string value = cell.Value?.ToString() ?? "";
                                    PdfPCell pdfCell = new PdfPCell(new Phrase(value, cellFont));
                                    pdfCell.Padding = 5;
                                    
                                    if (alternateRow)
                                    {
                                        pdfCell.BackgroundColor = new BaseColor(240, 253, 244); // Light green
                                    }
                                    
                                    table.AddCell(pdfCell);
                                }
                            }
                            alternateRow = !alternateRow;
                        }
                    }

                    document.Add(table);

                    // Add footer
                    Paragraph footer = new Paragraph($"\nTotal Records: {dataGridView.Rows.Count - 1}", dateFont);
                    footer.Alignment = Element.ALIGN_LEFT;
                    document.Add(footer);

                    document.Close();
                    writer.Close();

                    MessageBox.Show($"Data exported successfully to:\n{saveFileDialog.FileName}", 
                        "Export Successful", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);

                    // Ask if user wants to open the file
                    DialogResult openResult = MessageBox.Show("Do you want to open the exported file?", 
                        "Open File", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Question);

                    if (openResult == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(saveFileDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to PDF: {ex.Message}", 
                    "Export Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Escape CSV special characters
        /// </summary>
        private static string EscapeCSV(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            // If value contains comma, quote, or newline, wrap in quotes
            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n") || value.Contains("\r"))
            {
                // Escape quotes by doubling them
                value = value.Replace("\"", "\"\"");
                return $"\"{value}\"";
            }

            return value;
        }

        /// <summary>
        /// Export list to CSV
        /// </summary>
        public static void ExportListToCSV<T>(List<T> list, string defaultFileName = "export")
        {
            try
            {
                if (list == null || list.Count == 0)
                {
                    MessageBox.Show("No data to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                saveFileDialog.FileName = $"{defaultFileName}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                saveFileDialog.Title = "Export to CSV";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder sb = new StringBuilder();

                    // Get properties
                    var properties = typeof(T).GetProperties();

                    // Add headers
                    List<string> headers = new List<string>();
                    foreach (var prop in properties)
                    {
                        headers.Add(EscapeCSV(prop.Name));
                    }
                    sb.AppendLine(string.Join(",", headers));

                    // Add rows
                    foreach (var item in list)
                    {
                        List<string> values = new List<string>();
                        foreach (var prop in properties)
                        {
                            var value = prop.GetValue(item)?.ToString() ?? "";
                            values.Add(EscapeCSV(value));
                        }
                        sb.AppendLine(string.Join(",", values));
                    }

                    File.WriteAllText(saveFileDialog.FileName, sb.ToString(), Encoding.UTF8);

                    MessageBox.Show($"Data exported successfully to:\n{saveFileDialog.FileName}", 
                        "Export Successful", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);

                    // Ask if user wants to open the file
                    DialogResult openResult = MessageBox.Show("Do you want to open the exported file?", 
                        "Open File", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Question);

                    if (openResult == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(saveFileDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to CSV: {ex.Message}", 
                    "Export Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }
    }
}
