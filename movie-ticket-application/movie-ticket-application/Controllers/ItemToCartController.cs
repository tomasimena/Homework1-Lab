using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using movie_ticket_application.Data;
using movie_ticket_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace movie_ticket_application.Controllers
{
    public class ItemToCartController : Controller
    {

        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<IdentityUser> userManager;

        public ItemToCartController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {

            ViewData.Model = id;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int quantity, int id)
        {
            MovieTicket movieTicket;
            IdentityUser currentUser;
            CartItem cartItem = new CartItem();
            ShoppingCart shoppingCart = new ShoppingCart();
            bool cartExists = false;

            currentUser = await userManager.GetUserAsync(HttpContext.User);
            movieTicket = await this.dbContext.MovieTicket.FindAsync(id);

            cartItem.MovieTicket = movieTicket;
            cartItem.MovieTicketId = movieTicket.Id;
            cartItem.Quantity = quantity;

            foreach (ShoppingCart shoppingCart1 in dbContext.ShoppingCarts)
            {
                if (shoppingCart1.IdentityUserId == currentUser.Id)
                {
                    shoppingCart = shoppingCart1;
                    if (shoppingCart.CartItems == null)
                    {
                        shoppingCart.CartItems = new List<CartItem>();
                    }
                    cartExists = true;
                }
            }
            if (!cartExists)
            {
                shoppingCart = new ShoppingCart();
                shoppingCart.CartItems = new List<CartItem>();
                shoppingCart.IdentityUser = currentUser;
                shoppingCart.IdentityUserId = currentUser.Id;
            }

            cartItem.ShoppingCart = shoppingCart;
            cartItem.ShoppingCartId = shoppingCart.Id;
            dbContext.Add(cartItem);

            if(!cartExists)
            {
                dbContext.Add(shoppingCart);
            }
            else
            {
                dbContext.Update(shoppingCart);
            }

            dbContext.SaveChanges();

            return RedirectToAction("Index", "MovieTickets");
        }

    }
}
