using BookStore.Data;
using BookStore.DataModels;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace BookStore.Service
{
    public class SendMailService
    {
        private readonly ApplicationDbContext _db;

        public SendMailService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task SendEmail(string email, string resetLink)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress(email));
            message.Subject = "Reset your account password!";
            message.Body = $"<p>Click <a href='{resetLink}'>here</a> to reset your password.</p>";
            message.IsBodyHtml = true;
            message.From = new MailAddress("hoquangdat123@gmail.com");
            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential("hoquangdat123@gmail.com", "vpkp ssdb coam qfxp");
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
            }
            Log.Information("Email sent successfully");
        }

        public async Task SendConfirmOrderEmail(string email, Receipt item)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress(email));
            message.Subject = "Your Order Receipt from HQDStore";
            message.From = new MailAddress("hoquangdat123@gmail.com");
            message.IsBodyHtml = true;

            var receiptItems = _db.ReceiptItems.Where(r => r.ReceiptId == item.ReceiptId).ToList();
            var books = _db.Books.ToDictionary(b => b.BookId, b => b);

            var bodyBuilder = new StringBuilder();
            bodyBuilder.Append(@"
                <div style='font-family: Arial, sans-serif; padding: 20px; max-width: 600px; margin: auto; border: 1px solid #e0e0e0; border-radius: 10px;'>
                    <h2 style='text-align: center; color: #4CAF50;'>HQDStore - Order Confirmation</h2>
                    <p>Thank you for your purchase! Here is your receipt:</p>
                    <table style='width: 100%; border-collapse: collapse; margin-top: 20px;'>
                        <thead>
                            <tr style='background-color: #f2f2f2;'>
                                <th style='padding: 10px; text-align: left;'>Item</th>
                                <th style='padding: 10px; text-align: center;'>Quantity</th>
                                <th style='padding: 10px; text-align: right;'>Price</th>
                                <th style='padding: 10px; text-align: right;'>Total</th>
                            </tr>
                        </thead>
                        <tbody>
            ");

            foreach (var itemRow in receiptItems)
            {
                if (books.TryGetValue(itemRow.BookId, out var book))
                {
                    var totalPrice = book.Price * itemRow.Quantity;
                    bodyBuilder.Append($@"
                <tr>
                    <td style='padding: 10px;'>{book.BookName}</td>
                    <td style='padding: 10px; text-align: center;'>{itemRow.Quantity}</td>
                    <td style='padding: 10px; text-align: right;'>${book.Price:F2}</td>
                    <td style='padding: 10px; text-align: right;'>${totalPrice:F2}</td>
                </tr>");
                }
            }

            bodyBuilder.Append($@"
                        </tbody>
                    </table>
                    <hr style='margin-top: 20px;'>
                    <h3 style='text-align: right; color: #d9534f;'>Total: ${item.TotalAmount:F2}</h3>
                    <p style='text-align: center; margin-top: 30px;'>We hope to see you again soon!</p>
                    <p style='text-align: center; font-size: 12px; color: #888;'>This is an automated message. Please do not reply.</p>
                </div>
            ");

            message.Body = bodyBuilder.ToString();

            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential("hoquangdat123@gmail.com", "vpkp ssdb coam qfxp");
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
            }
            Log.Information("Order confirmation email sent successfully");
        }
    }
}
