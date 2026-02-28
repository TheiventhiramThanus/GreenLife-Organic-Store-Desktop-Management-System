using System;
using System.Net;
using System.Net.Mail;

namespace GreenLifeWinForms.Services
{
    public class EmailNotificationService
    {
        // ?? SMTP Configuration ?????????????????????????????????????????????
        // Change these to your real SMTP server settings before deploying.
        private readonly string smtpHost;
        private readonly int smtpPort;
        private readonly string smtpUser;
        private readonly string smtpPass;
        private readonly string fromAddress;
        private readonly string fromName;
        private readonly bool enableSsl;

        public EmailNotificationService()
        {
            smtpHost    = "smtp.gmail.com";
            smtpPort    = 587;
            smtpUser    = "greenlife.organic.store@gmail.com";
            smtpPass    = "your-app-password-here";
            fromAddress = "greenlife.organic.store@gmail.com";
            fromName    = "GreenLife Organic Store";
            enableSsl   = true;
        }

        // ?? Core send method ???????????????????????????????????????????????
        public bool SendEmail(string toAddress, string subject, string htmlBody)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(fromAddress, fromName);
                    mail.To.Add(new MailAddress(toAddress));
                    mail.Subject    = subject;
                    mail.Body       = htmlBody;
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient(smtpHost, smtpPort))
                    {
                        smtp.Credentials = new NetworkCredential(smtpUser, smtpPass);
                        smtp.EnableSsl   = enableSsl;
                        smtp.Timeout     = 15000;
                        smtp.Send(mail);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Email send failed: {ex.Message}");
                return false;
            }
        }

        // ?? Welcome email after registration ??????????????????????????????
        public bool SendWelcomeEmail(string toAddress, string customerName)
        {
            string subject = "Welcome to GreenLife Organic Store!";
            string body = BuildHtmlTemplate(
                "Welcome to GreenLife! ??",
                $@"<p>Dear <strong>{Escape(customerName)}</strong>,</p>
                   <p>Thank you for joining GreenLife Organic Store! We are excited to have you as part of our community.</p>
                   <p>You can now:</p>
                   <ul>
                     <li>Browse our wide range of organic products</li>
                     <li>Place orders and track deliveries</li>
                     <li>Leave reviews on your favorite products</li>
                   </ul>
                   <p>Start shopping today and enjoy fresh, healthy, organic products!</p>");

            return SendEmail(toAddress, subject, body);
        }

        // ?? Order confirmation ????????????????????????????????????????????
        public bool SendOrderConfirmation(string toAddress, string customerName,
                                          int orderId, decimal grandTotal, int itemCount)
        {
            string subject = $"Order #{orderId} Confirmed - GreenLife";
            string body = BuildHtmlTemplate(
                "Order Confirmed! ?",
                $@"<p>Dear <strong>{Escape(customerName)}</strong>,</p>
                   <p>Your order has been placed successfully!</p>
                   <table style='border-collapse:collapse;width:100%;margin:16px 0'>
                     <tr style='background:#dcfce7'>
                       <td style='padding:10px;border:1px solid #bbf7d0'><strong>Order ID</strong></td>
                       <td style='padding:10px;border:1px solid #bbf7d0'>#{orderId}</td>
                     </tr>
                     <tr>
                       <td style='padding:10px;border:1px solid #bbf7d0'><strong>Items</strong></td>
                       <td style='padding:10px;border:1px solid #bbf7d0'>{itemCount}</td>
                     </tr>
                     <tr style='background:#dcfce7'>
                       <td style='padding:10px;border:1px solid #bbf7d0'><strong>Total</strong></td>
                       <td style='padding:10px;border:1px solid #bbf7d0'><strong>LKR {grandTotal:F2}</strong></td>
                     </tr>
                   </table>
                   <p>We will notify you when your order is shipped.</p>
                   <p>Thank you for shopping with GreenLife!</p>");

            return SendEmail(toAddress, subject, body);
        }

        // ?? Order status update ??????????????????????????????????????????
        public bool SendOrderStatusUpdate(string toAddress, string customerName,
                                          int orderId, string newStatus)
        {
            string emoji = newStatus == "Shipped"    ? "??" :
                           newStatus == "Delivered"  ? "??" :
                           newStatus == "Cancelled"  ? "?" : "??";

            string subject = $"Order #{orderId} - {newStatus} {emoji}";
            string body = BuildHtmlTemplate(
                $"Order Status Update {emoji}",
                $@"<p>Dear <strong>{Escape(customerName)}</strong>,</p>
                   <p>Your order <strong>#{orderId}</strong> status has been updated to:</p>
                   <p style='font-size:24px;font-weight:bold;color:#16a34a;text-align:center;
                             padding:16px;background:#dcfce7;border-radius:8px;margin:16px 0'>
                     {Escape(newStatus)}
                   </p>
                   <p>Thank you for choosing GreenLife Organic Store!</p>");

            return SendEmail(toAddress, subject, body);
        }

        // ?? Password reset ??????????????????????????????????????????????
        public bool SendPasswordResetEmail(string toAddress, string customerName)
        {
            string subject = "Password Changed - GreenLife";
            string body = BuildHtmlTemplate(
                "Password Changed ??",
                $@"<p>Dear <strong>{Escape(customerName)}</strong>,</p>
                   <p>Your password has been changed successfully.</p>
                   <p>If you did not make this change, please contact our support team immediately.</p>");

            return SendEmail(toAddress, subject, body);
        }

        // ?? HTML template builder ????????????????????????????????????????
        private static string BuildHtmlTemplate(string heading, string content)
        {
            return $@"<!DOCTYPE html>
<html>
<head><meta charset='utf-8'></head>
<body style='margin:0;padding:0;font-family:Segoe UI,Arial,sans-serif;background:#f0fdf4'>
  <table width='100%' cellpadding='0' cellspacing='0'>
    <tr><td align='center' style='padding:30px 0'>
      <table width='600' cellpadding='0' cellspacing='0' style='background:#fff;border-radius:12px;overflow:hidden;box-shadow:0 2px 8px rgba(0,0,0,0.08)'>
        <tr><td style='background:#16a34a;padding:24px;text-align:center'>
          <h1 style='color:#fff;margin:0;font-size:24px'>?? GreenLife Organic Store</h1>
        </td></tr>
        <tr><td style='padding:32px'>
          <h2 style='color:#14532d;margin:0 0 16px'>{heading}</h2>
          {content}
        </td></tr>
        <tr><td style='background:#f0fdf4;padding:16px;text-align:center;color:#6b7280;font-size:12px'>
          &copy; {DateTime.Now.Year} GreenLife Organic Store. All rights reserved.
        </td></tr>
      </table>
    </td></tr>
  </table>
</body>
</html>";
        }

        private static string Escape(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return System.Security.SecurityElement.Escape(value);
        }
    }
}
