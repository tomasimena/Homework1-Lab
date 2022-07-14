using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using movie_ticket_application.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace movie_ticket_application.Controllers
{
   // [Authorize(Roles = "Administrator")]
    public class AdministratorUserImportController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;

        public AdministratorUserImportController (UserManager<IdentityUser> userManager)
        {
            this._userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ImportUsers(IFormFile file)
        {

            string email;
            string password;
            string role;

            if( file == null ) return RedirectToAction("Index", "AdministratorUserImport");

            if (file.FileName.EndsWith(".xlsx"))
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        while (reader.Read()) //Each row of the file
                        {
                            email = reader.GetValue(0).ToString();
                            password = reader.GetValue(1).ToString();
                            role = reader.GetValue(2).ToString();

                            var user = new User
                            {
                                UserName = email,
                                Username = email
                            };

                            //var result = _userManager.CreateAsync(user, password);
                            var result = Task.Run(async () => await _userManager.CreateAsync(user, password)).Result;
                            if (result.Succeeded)
                            {
                                if (role.Equals("Administrator", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    Task.Run(async () => await _userManager.AddToRoleAsync(user, "Administrator"));
                                    Task.Run(async () => await _userManager.AddToRoleAsync(user, "BasicUser"));
                                } else if (role.Equals("BasicUser", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    Task.Run(async () => await _userManager.AddToRoleAsync(user, "BasicUser"));
                                }
                            }

                        }
                    }
                }
            }

            return RedirectToAction("Index", "AdministratorUserImport");
        }

    }
}
