using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using movie_ticket_application.Data;
using movie_ticket_application.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace movie_ticket_application.Controllers
{
    //[Authorize(Roles = "Administrator")]
    public class AdministratorExportController : Controller
    {

        private readonly ApplicationDbContext dbContext;

        public AdministratorExportController (ApplicationDbContext applicationDbContext)
        {
            this.dbContext = applicationDbContext;
        }

        public IActionResult Index()
        {



            return View();
        }

        [HttpPost]
        public IActionResult Excel(string Genre)
        {
            using (var workbook = new XLWorkbook())
            {

                var worksheet = workbook.Worksheets.Add("Tickets");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Movie Name";
                worksheet.Cell(currentRow, 2).Value = "Hall Name";
                worksheet.Cell(currentRow, 3).Value = "Expiration Date";
                worksheet.Cell(currentRow, 4).Value = "Genre";
                worksheet.Cell(currentRow, 5).Value = "Price";

                Movie movie;

                this.dbContext.MovieTicket.ToList().ForEach(ticket =>
                {
                    
                    movie = dbContext.Movie.Find(ticket.MovieId);

                    if (movie != null && movie.Genre.Equals(Genre, StringComparison.InvariantCultureIgnoreCase))
                    {
                        currentRow++;

                        worksheet.Cell(currentRow, 1).Value = movie.Title != null && movie.Title.Length != 0 ? movie.Title : "/";
                        worksheet.Cell(currentRow, 2).Value = ticket.HallName != null && ticket.HallName.Length != 0 ? ticket.HallName : "/";
                        worksheet.Cell(currentRow, 3).Value = ticket.ExpirationDate;
                        worksheet.Cell(currentRow, 4).Value = movie.Genre != null && movie.Genre.Length != 0 ? movie.Genre : "/";
                        worksheet.Cell(currentRow, 5).Value = movie.Price;
                    }

                });

                //var worksheet = workbook.Worksheets.Add("Users");
                //var currentRow = 1;
                //worksheet.Cell(currentRow, 1).Value = "Id";
                //worksheet.Cell(currentRow, 2).Value = "Username";
                //foreach (var user in users)
                //{
                //    currentRow++;
                //    worksheet.Cell(currentRow, 1).Value = user.Id;
                //    worksheet.Cell(currentRow, 2).Value = user.Username;
                //}


                //-------------------

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "tickets.xlsx");
                }
            }
        }

    }
}
