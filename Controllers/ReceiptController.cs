using BookStore.Data;
using BookStore.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Text;
using Hangfire;
using BookStore.Service;

namespace BookStore.Controllers
{
    [Authorize]
    public class ReceiptController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ReceiptController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartCheckout = _db.Carts
                .Include(c => c.Book)
                .Include(c => c.Account)
                .Where(c => c.AccountId == int.Parse(userId))
                .ToList();
            if (cartCheckout == null || !cartCheckout.Any())
            {
                TempData["error"] = "Cart is empty!";
                return RedirectToAction("Index", "Cart");
            }
            ViewBag.Address = _db.Accounts.Find(int.Parse(userId)).Address;
            ViewBag.Email = _db.Accounts.Find(int.Parse(userId)).Email;
            return View(cartCheckout);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmCheckout(List<Cart> CartItems)
        {
            if (CartItems == null || !CartItems.Any())
            {
                TempData["error"] = "Your cart is empty!";
                return RedirectToAction("Index", "Cart");
            }

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var checkOut = new Receipt
            {
                AccountId = userId,
                TotalAmount = CartItems.Sum(c => _db.Books.First(b => b.BookId == c.BookId).Price * c.Quantity),
            };
            _db.Receipts.Add(checkOut);
            _db.SaveChanges(); 

            var receiptItems = CartItems.Select(c => new ReceiptItem
            {
                ReceiptId = checkOut.ReceiptId,
                BookId = c.BookId,
                Quantity = c.Quantity,
            }).ToList();

            _db.ReceiptItems.AddRange(receiptItems);

            var cartItemsToRemove = _db.Carts.Where(c => c.AccountId == userId).ToList();
            _db.Carts.RemoveRange(cartItemsToRemove);

            _db.SaveChanges();
            TempData["success"] = "Checkout successfully!";

            //Send email to user
            var receipt = _db.Receipts.Include(r=>r.ReceiptItems).FirstOrDefault(rid=>rid.ReceiptId == checkOut.ReceiptId);
            var userEmail = _db.Accounts.Find(userId).Email;
            if(userEmail != null)
            {
                BackgroundJob.Enqueue<SendMailService>(mail => mail.SendConfirmOrderEmail(userEmail, receipt));
                TempData["success"] = "Email sent successfully!";
                return RedirectToAction("Index", "Home");
            }
            TempData["error"] = "Email not found!";
            return RedirectToAction("Index", "Home");
        }


        //Send Email method
        //public async Task<IActionResult> SendEmail(string email, Receipt item)
        //{
        //    var message = new MailMessage();
        //    message.To.Add(new MailAddress(email));
        //    message.Subject = "Your Order Receipt from HQDStore";
        //    message.From = new MailAddress("hoquangdat123@gmail.com");
        //    message.IsBodyHtml = true;

        //    var receiptItems = _db.ReceiptItems.Where(r => r.ReceiptId == item.ReceiptId).ToList();
        //    var books = _db.Books.ToDictionary(b => b.BookId, b => b);

        //    var bodyBuilder = new StringBuilder();
        //    bodyBuilder.Append(@"
        //        <div style='font-family: Arial, sans-serif; padding: 20px; max-width: 600px; margin: auto; border: 1px solid #e0e0e0; border-radius: 10px;'>
        //            <h2 style='text-align: center; color: #4CAF50;'>HQDStore - Order Confirmation</h2>
        //            <p>Thank you for your purchase! Here is your receipt:</p>
        //            <table style='width: 100%; border-collapse: collapse; margin-top: 20px;'>
        //                <thead>
        //                    <tr style='background-color: #f2f2f2;'>
        //                        <th style='padding: 10px; text-align: left;'>Item</th>
        //                        <th style='padding: 10px; text-align: center;'>Quantity</th>
        //                        <th style='padding: 10px; text-align: right;'>Price</th>
        //                        <th style='padding: 10px; text-align: right;'>Total</th>
        //                    </tr>
        //                </thead>
        //                <tbody>
        //    ");

        //    foreach (var itemRow in receiptItems)
        //    {
        //        if (books.TryGetValue(itemRow.BookId, out var book))
        //        {
        //            var totalPrice = book.Price * itemRow.Quantity;
        //            bodyBuilder.Append($@"
        //        <tr>
        //            <td style='padding: 10px;'>{book.BookName}</td>
        //            <td style='padding: 10px; text-align: center;'>{itemRow.Quantity}</td>
        //            <td style='padding: 10px; text-align: right;'>${book.Price:F2}</td>
        //            <td style='padding: 10px; text-align: right;'>${totalPrice:F2}</td>
        //        </tr>");
        //        }
        //    }

        //    bodyBuilder.Append($@"
        //                </tbody>
        //            </table>
        //            <hr style='margin-top: 20px;'>
        //            <h3 style='text-align: right; color: #d9534f;'>Total: ${item.TotalAmount:F2}</h3>
        //            <p style='text-align: center; margin-top: 30px;'>We hope to see you again soon!</p>
        //            <p style='text-align: center; font-size: 12px; color: #888;'>This is an automated message. Please do not reply.</p>
        //        </div>
        //    ");

        //    message.Body = bodyBuilder.ToString();

        //    using (var smtp = new SmtpClient("smtp.gmail.com", 587))
        //    {
        //        smtp.Credentials = new NetworkCredential("hoquangdat123@gmail.com", "vpkp ssdb coam qfxp");
        //        smtp.EnableSsl = true;
        //        await smtp.SendMailAsync(message);
        //    }

        //    TempData["success"] = "Email sent successfully";
        //    return Content("Email sent successfully!");
        //}

    }
}
