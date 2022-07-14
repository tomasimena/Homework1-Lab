using Microsoft.AspNetCore.Authorization;
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
   // [Authorize(Roles = "Administrator")]
    public class AdministratorController : Controller
    {

        private readonly UserManager<IdentityUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public AdministratorController (UserManager<IdentityUser> userManager, 
            ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.dbContext = context;
        }

        public async Task<IActionResult> Index()
        {

            List<IdentityUser> users = userManager.Users.ToList();
            users.Sort( (user1, user2) => user1.UserName.CompareTo(user2.UserName) );

            ViewData["users"] = users;


            return View();
        }

        public async Task<IActionResult> MakeBasicUser(string id)
        {

            IdentityUser user = userManager.FindByIdAsync(id).Result;
            await userManager.RemoveFromRoleAsync(user, "Administrator");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MakeAdmin(string id)
        {

            IdentityUser user = userManager.FindByIdAsync(id).Result;
            await userManager.AddToRoleAsync(user, "Administrator");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete (string id)
        {

            IdentityUser user = userManager.FindByIdAsync(id).Result;
            await userManager.DeleteAsync(user);

            return RedirectToAction(nameof(Index));
        }


    }
}
