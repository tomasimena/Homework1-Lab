using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using movie_ticket_application.Data;
using movie_ticket_application.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace movie_ticket_application.Controllers
{
    //[Authorize(Roles = "BasicUser")]
    public class ShoppingCartController : Controller
    {

        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<IdentityUser> userManager;
        private readonly MailSettings mailSettings;

        public ShoppingCartController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, 
            IOptions<MailSettings> mailSettings)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.mailSettings = mailSettings.Value;
        }

        public async Task<IActionResult> IndexAsync()
        {
            IdentityUser currentUser = await userManager.GetUserAsync(HttpContext.User);
            ShoppingCart shoppingCart = new ShoppingCart();

            foreach (ShoppingCart shoppingCart1 in dbContext.ShoppingCarts)
            {
                if (shoppingCart1.IdentityUserId == currentUser.Id)
                {
                    shoppingCart = shoppingCart1;
                }
            }

            List<CartItem> cartItems = new List<CartItem>();

            dbContext.CartItems.AsEnumerable()
                .ToList()
                .ForEach(item =>
                {
                    if (item.ShoppingCartId == shoppingCart.Id)
                    {

                        dbContext.MovieTicket.ToList().ForEach(ticket =>
                        {
                            if (ticket.Id == item.MovieTicketId)
                            {

                                dbContext.Movie.ToList().ForEach(movie =>
                                {
                                    if (movie.Id == ticket.MovieId)
                                    {
                                        ticket.Movie = movie;
                                    }
                                });

                                item.MovieTicket = ticket;
                            }
                        });

                        cartItems.Add(item);
                    }
                });

            ViewData["shoppingCartItems"] = cartItems;

            decimal totalCost = 0;
            cartItems.ForEach(item =>
            {
                totalCost += item.MovieTicket.Movie.Price * item.Quantity;
            });

            ViewData["totalCost"] = totalCost;


            return View();
        }

        public async Task<IActionResult> EmptyShoppingCart ()
        {
            IdentityUser currentUser = await userManager.GetUserAsync(HttpContext.User);
            ShoppingCart userShoppingCart = null;

            dbContext.ShoppingCarts.ToList().ForEach(cart =>
            {
                if (cart.IdentityUserId == currentUser.Id)
                {
                    userShoppingCart = cart;
                }
            });

            dbContext.CartItems.ToList().ForEach(item =>
            {
                if (item.ShoppingCartId == userShoppingCart.Id)
                {
                    dbContext.Remove(item);
                }
            });

            dbContext.SaveChanges();

            return RedirectToAction("Index", "ShoppingCart");
        }


        public async Task<IActionResult> Checkout()
        {
            IdentityUser currentUser = await userManager.GetUserAsync(HttpContext.User);
            ShoppingCart userShoppingCart = null;

            Order order = new Order();
            order.CartItems = new List<CartItem>();

            decimal totalCost = 0;

            dbContext.ShoppingCarts.ToList().ForEach(cart =>
            {
                if (cart.IdentityUserId == currentUser.Id)
                {
                    userShoppingCart = cart;
                }
            });



            dbContext.CartItems.AsEnumerable()
                .ToList()
                .ForEach(item =>
                {
                    if (item.ShoppingCartId == userShoppingCart.Id)
                    {

                        dbContext.MovieTicket.ToList().ForEach(ticket =>
                        {
                            if (ticket.Id == item.MovieTicketId)
                            {

                                dbContext.Movie.ToList().ForEach(movie =>
                                {
                                    if (movie.Id == ticket.MovieId)
                                    {
                                        ticket.Movie = movie;
                                    }
                                });

                                item.MovieTicket = ticket;
                            }
                        });

                    }
                    order.CartItems.Add(item);
                    dbContext.Remove(item);
                });


            order.CartItems.ForEach(item =>
            {
                totalCost += item.MovieTicket.Movie.Price * item.Quantity;
            });
            order.TotalPrice = totalCost;

            order.IdentityUser = currentUser;
            order.IdentityUserId = currentUser.Id;
            order.CreatedOn = DateTime.Now;

            dbContext.Add(order);
            dbContext.SaveChanges();


            //MailRequest mailRequest = new MailRequest();
            //mailRequest.ToEmail = currentUser.UserName;
            //mailRequest.Subject = "Order Purchase Confirmation";
            //mailRequest.Body = "Your order has been processed successfully.\nThank you for purchasing from us";

            //var email = new MimeMessage();
            //email.Sender = MailboxAddress.Parse(mailSettings.Mail);
            //email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            //email.Subject = mailRequest.Subject;
            //var builder = new BodyBuilder();
            //if (mailRequest.Attachments != null)
            //{
            //    byte[] fileBytes;
            //    foreach (var file in mailRequest.Attachments)
            //    {
            //        if (file.Length > 0)
            //        {
            //            using (var ms = new MemoryStream())
            //            {
            //                file.CopyTo(ms);
            //                fileBytes = ms.ToArray();
            //            }
            //            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
            //        }
            //    }
            //}
            //builder.HtmlBody = mailRequest.Body;
            //email.Body = builder.ToMessageBody();

            //using (var smtp = new SmtpClient())
            //{
            //    smtp.ServerCertificateValidationCallback = (s, c, h, e) => true; //Not recommended though...

            //    smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.Auto);
            //    smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
            //    await smtp.SendAsync(email);
            //    smtp.Disconnect(true);
            //}


            return RedirectToAction("Index", "ShoppingCart");
        }


    }
}
