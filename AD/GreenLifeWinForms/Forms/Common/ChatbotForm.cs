using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using GreenLifeWinForms.Services;

namespace GreenLifeWinForms.Forms.Common
{
    public partial class ChatbotForm : Form
    {
        private readonly ChatbotService chatbotService;
        private Panel pnlMessages;
        private TextBox txtInput;
        private Button btnSend;
        private FlowLayoutPanel pnlQuickReplies;

        public ChatbotForm()
        {
            chatbotService = new ChatbotService();
            InitializeComponent();
            ShowWelcomeMessage();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "GreenLife Assistant";
            this.Size = new Size(520, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 253, 244);

            // ?? Header ????????????????????????????????????????????????????
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 65,
                BackColor = Color.FromArgb(22, 163, 74)
            };
            this.Controls.Add(pnlHeader);

            Label lblBotIcon = new Label
            {
                Text = "??",
                Font = new Font("Segoe UI Emoji", 22F),
                AutoSize = true,
                ForeColor = Color.White,
                Location = new Point(14, 14)
            };
            pnlHeader.Controls.Add(lblBotIcon);

            Label lblBotName = new Label
            {
                Text = "GreenLife Assistant",
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(58, 10)
            };
            pnlHeader.Controls.Add(lblBotName);

            Label lblStatus = new Label
            {
                Text = "? Online — Ask me anything!",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(187, 247, 208),
                AutoSize = true,
                Location = new Point(60, 38)
            };
            pnlHeader.Controls.Add(lblStatus);

            Button btnClose = new Button
            {
                Text = "?",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Size = new Size(36, 36),
                Location = new Point(460, 14),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(22, 163, 74),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();
            pnlHeader.Controls.Add(btnClose);

            // ?? Messages area ?????????????????????????????????????????????
            pnlMessages = new Panel
            {
                Location = new Point(0, 65),
                Size = new Size(504, 460),
                AutoScroll = true,
                BackColor = Color.White
            };
            this.Controls.Add(pnlMessages);

            // ?? Quick replies ?????????????????????????????????????????????
            pnlQuickReplies = new FlowLayoutPanel
            {
                Location = new Point(0, 525),
                Size = new Size(504, 50),
                BackColor = Color.FromArgb(245, 250, 246),
                AutoScroll = false,
                WrapContents = false,
                Padding = new Padding(8, 10, 8, 4)
            };
            this.Controls.Add(pnlQuickReplies);

            LoadQuickReplies();

            // ?? Input area ????????????????????????????????????????????????
            Panel pnlInput = new Panel
            {
                Location = new Point(0, 575),
                Size = new Size(504, 85),
                BackColor = Color.White
            };
            this.Controls.Add(pnlInput);

            txtInput = new TextBox
            {
                Font = new Font("Segoe UI", 12F),
                Size = new Size(390, 35),
                Location = new Point(12, 20),
                ForeColor = Color.Gray,
                Text = "Type your message..."
            };
            txtInput.Enter += (se, ev) =>
            {
                if (txtInput.ForeColor == Color.Gray)
                {
                    txtInput.Text = "";
                    txtInput.ForeColor = Color.Black;
                }
            };
            txtInput.Leave += (se, ev) =>
            {
                if (string.IsNullOrWhiteSpace(txtInput.Text))
                {
                    txtInput.Text = "Type your message...";
                    txtInput.ForeColor = Color.Gray;
                }
            };
            txtInput.KeyDown += TxtInput_KeyDown;
            pnlInput.Controls.Add(txtInput);

            btnSend = new Button
            {
                Text = "?",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                Size = new Size(60, 40),
                Location = new Point(415, 16),
                BackColor = Color.FromArgb(22, 163, 74),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSend.FlatAppearance.BorderSize = 0;
            btnSend.Click += BtnSend_Click;
            pnlInput.Controls.Add(btnSend);

            this.ResumeLayout(false);
        }

        // ?? Quick reply chips ?????????????????????????????????????????????
        private void LoadQuickReplies()
        {
            pnlQuickReplies.Controls.Clear();
            List<string> replies = chatbotService.GetQuickReplies();
            foreach (string reply in replies)
            {
                Button chip = new Button
                {
                    Text = reply,
                    Font = new Font("Segoe UI", 8F),
                    AutoSize = true,
                    Height = 28,
                    BackColor = Color.FromArgb(220, 252, 231),
                    ForeColor = Color.FromArgb(20, 83, 45),
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand,
                    Margin = new Padding(2),
                    Padding = new Padding(6, 0, 6, 0)
                };
                chip.FlatAppearance.BorderColor = Color.FromArgb(187, 247, 208);
                chip.Click += (s, e) =>
                {
                    txtInput.Text = reply;
                    BtnSend_Click(s, e);
                };
                pnlQuickReplies.Controls.Add(chip);
            }
        }

        // ?? Welcome ???????????????????????????????????????????????????????
        private void ShowWelcomeMessage()
        {
            AddBotMessage("Hello! ?? I'm the GreenLife Assistant.\n\nI can help you with products, orders, payments, account questions, and more.\n\nType a message or tap a quick reply below to get started!");
        }

        // ?? Send message ??????????????????????????????????????????????????
        private void BtnSend_Click(object sender, EventArgs e)
        {
            string message = txtInput.Text.Trim();
            if (string.IsNullOrEmpty(message) || txtInput.ForeColor == Color.Gray) return;

            AddUserMessage(message);
            txtInput.Clear();
            txtInput.Focus();

            // Small delay for natural feel
            Timer timer = new Timer { Interval = 400 };
            string captured = message;
            timer.Tick += (ts, te) =>
            {
                timer.Stop();
                timer.Dispose();
                string response = chatbotService.GetResponse(captured);
                AddBotMessage(response);
            };
            timer.Start();
        }

        private void TxtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                BtnSend_Click(sender, e);
            }
        }

        // ?? Message bubbles ???????????????????????????????????????????????
        private int messageTop = 10;

        private void AddUserMessage(string text)
        {
            Panel bubble = CreateBubble(text, isUser: true);
            bubble.Location = new Point(pnlMessages.ClientSize.Width - bubble.Width - 18, messageTop);
            pnlMessages.Controls.Add(bubble);
            messageTop += bubble.Height + 10;
            ScrollToBottom();
        }

        private void AddBotMessage(string text)
        {
            Panel bubble = CreateBubble(text, isUser: false);
            bubble.Location = new Point(12, messageTop);
            pnlMessages.Controls.Add(bubble);
            messageTop += bubble.Height + 10;
            ScrollToBottom();
        }

        private Panel CreateBubble(string text, bool isUser)
        {
            Color bgColor = isUser
                ? Color.FromArgb(22, 163, 74)
                : Color.FromArgb(240, 253, 244);
            Color fgColor = isUser
                ? Color.White
                : Color.FromArgb(20, 83, 45);

            int maxWidth = 340;

            Label lblText = new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 10F),
                ForeColor = fgColor,
                MaximumSize = new Size(maxWidth - 20, 0),
                AutoSize = true,
                Padding = new Padding(0)
            };
            Size textSize = lblText.GetPreferredSize(new Size(maxWidth - 20, 0));

            int bubbleWidth = textSize.Width + 24;
            int bubbleHeight = textSize.Height + 20;

            Panel bubble = new Panel
            {
                Size = new Size(bubbleWidth, bubbleHeight),
                BackColor = bgColor
            };
            bubble.Paint += (s, e) =>
            {
                using (GraphicsPath path = CreateRoundedRect(new Rectangle(0, 0, bubble.Width, bubble.Height), 12))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using (SolidBrush brush = new SolidBrush(bgColor))
                        e.Graphics.FillPath(brush, path);
                }
            };

            lblText.Location = new Point(12, 10);
            lblText.BackColor = Color.Transparent;
            bubble.Controls.Add(lblText);

            // Timestamp
            Label lblTime = new Label
            {
                Text = DateTime.Now.ToString("HH:mm"),
                Font = new Font("Segoe UI", 7F),
                ForeColor = isUser ? Color.FromArgb(200, 255, 200) : Color.Gray,
                AutoSize = true,
                BackColor = Color.Transparent
            };
            lblTime.Location = new Point(bubbleWidth - lblTime.PreferredWidth - 10, bubbleHeight - 14);
            bubble.Controls.Add(lblTime);

            bubble.Height = Math.Max(bubbleHeight, lblTime.Bottom + 4);

            return bubble;
        }

        private static GraphicsPath CreateRoundedRect(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void ScrollToBottom()
        {
            pnlMessages.AutoScrollPosition = new Point(0, pnlMessages.DisplayRectangle.Height);
        }
    }
}
