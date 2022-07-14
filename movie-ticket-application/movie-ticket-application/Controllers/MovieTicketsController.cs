using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using movie_ticket_application.Data;
using movie_ticket_application.Models;

namespace movie_ticket_application.Controllers
{
    public class MovieTicketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MovieTicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MovieTickets
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MovieTicket.Include(m => m.Movie);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MovieTickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieTicket = await _context.MovieTicket
                .Include(m => m.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieTicket == null)
            {
                return NotFound();
            }

            return View(movieTicket);
        }

        // GET: MovieTickets/Create
        //[Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["MovieId"] = new SelectList(_context.Movie, "Id", "Title");
            return View();
        }

        // POST: MovieTickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       // [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("Id,MovieId,HallName,ExpirationDate")] MovieTicket movieTicket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movieTicket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieId"] = new SelectList(_context.Movie, "Id", "Id", movieTicket.MovieId);
            return View(movieTicket);
        }

        // GET: MovieTickets/Edit/5
       // [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieTicket = await _context.MovieTicket.FindAsync(id);
            if (movieTicket == null)
            {
                return NotFound();
            }
            ViewData["MovieId"] = new SelectList(_context.Movie, "Id", "Id", movieTicket.MovieId);
            return View(movieTicket);
        }

        // POST: MovieTickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MovieId,HallName,ExpirationDate")] MovieTicket movieTicket)
        {
            if (id != movieTicket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movieTicket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieTicketExists(movieTicket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieId"] = new SelectList(_context.Movie, "Id", "Id", movieTicket.MovieId);
            return View(movieTicket);
        }

        // GET: MovieTickets/Delete/5
        //[Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieTicket = await _context.MovieTicket
                .Include(m => m.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieTicket == null)
            {
                return NotFound();
            }

            return View(movieTicket);
        }

        // POST: MovieTickets/Delete/5
        //[Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movieTicket = await _context.MovieTicket.FindAsync(id);
            _context.MovieTicket.Remove(movieTicket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieTicketExists(int id)
        {
            return _context.MovieTicket.Any(e => e.Id == id);
        }
    }
}
