using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using movie_ticket_application.Data;
using movie_ticket_application.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using System.IO;
using Syncfusion.Pdf.Grid;
using Microsoft.AspNetCore.Authorization;

namespace movie_ticket_application.Controllers
{
    //[Authorize(Roles = "BasicUser")]
    public class OrdersController : Controller
    {

        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<IdentityUser> userManager;

        public OrdersController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task<IActionResult> IndexAsync()
        {
            IdentityUser currentUser = await userManager.GetUserAsync(HttpContext.User);
            List<Order> orders = new List<Order>();

            foreach (Order order in dbContext.Orders)
            {
                if (order.IdentityUserId == currentUser.Id)
                {
                    orders.Add(order);
                }
            }


            //Create a new PDF document.
            PdfDocument doc = new PdfDocument();
            //Add a page.
            PdfPage page = doc.Pages.Add();
            //Create a PdfGrid.
            PdfGrid pdfGrid = new PdfGrid();
            //Add values to list
            List<object> data = new List<object>();


            orders.ForEach(o =>
            {
                data.Add(new
                {
                    User = o.IdentityUser.UserName, Created_On = o.CreatedOn, Total_Price = o.TotalPrice
                }
                );
            });

            //Add list to IEnumerable
            IEnumerable<object> dataTable = data;
            //Assign data source.
            pdfGrid.DataSource = dataTable;
            //Draw grid to the page of PDF document.
            pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(10, 10));
            //Save the PDF document to stream
            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            //If the position is not set to '0' then the PDF will be empty.
            stream.Position = 0;
            //Close the document.
            doc.Close(true);
            //Defining the ContentType for pdf file.
            string contentType = "application/pdf";
            //Define the file name.
            string fileName = "Orders.pdf";
            //Creates a FileContentResult object by using the file contents, content type, and file name.
            return File(stream, contentType, fileName);
        }
    }
}
