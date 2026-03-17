using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CLDV7111_POE_PART_1_EventEase.Data;
using CLDV7111_POE_PART_1_EventEase.Models;

namespace CLDV7111_POE_PART_1_EventEase.Controllers
{
    public class VenuesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VenuesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Venues
        public async Task<IActionResult> Index()
        {
            Console.WriteLine("INDEX LOADED");
            return View(await _context.Venues.ToListAsync());
        }

        // GET: Venues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Console.WriteLine("DETAILS LOADED");

            if (id == null)
            {
                Console.WriteLine("DETAILS: ID NULL");
                return NotFound();
            }

            var venue = await _context.Venues
                .FirstOrDefaultAsync(m => m.VenueId == id);

            if (venue == null)
            {
                Console.WriteLine("DETAILS: VENUE NOT FOUND");
                return NotFound();
            }

            return View(venue);
        }

        // GET: Venues/Create
        public IActionResult Create()
        {
            Console.WriteLine("GET Create called");
            return View();
        }

        // POST: Venues/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VenueId,VenueName,Location,Capacity,ImageUrl")] Venue venue)
        {
            Console.WriteLine("POST Create called");

            if (ModelState.IsValid)
            {
                Console.WriteLine("MODEL IS VALID — ATTEMPTING TO SAVE VENUE");

                _context.Add(venue);
                await _context.SaveChangesAsync();

                Console.WriteLine("SAVE SUCCESSFUL");

                return RedirectToAction(nameof(Index));
            }

            Console.WriteLine("MODELSTATE INVALID — VENUE NOT SAVED");
            return View(venue);
        }

        // GET: Venues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Console.WriteLine("GET Edit called");

            if (id == null)
            {
                Console.WriteLine("EDIT: ID NULL");
                return NotFound();
            }

            var venue = await _context.Venues.FindAsync(id);

            if (venue == null)
            {
                Console.WriteLine("EDIT: VENUE NOT FOUND");
                return NotFound();
            }

            return View(venue);
        }

        // POST: Venues/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VenueId,VenueName,Location,Capacity,ImageUrl")] Venue venue)
        {
            Console.WriteLine("POST Edit called");

            if (id != venue.VenueId)
            {
                Console.WriteLine("EDIT: ID MISMATCH");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine("EDIT: MODEL VALID — UPDATING");
                    _context.Update(venue);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("EDIT SAVE SUCCESSFUL");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueExists(venue.VenueId))
                    {
                        Console.WriteLine("EDIT: VENUE NOT FOUND DURING SAVE");
                        return NotFound();
                    }
                    else
                    {
                        Console.WriteLine("EDIT: CONCURRENCY ERROR");
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            Console.WriteLine("EDIT: MODEL INVALID");
            return View(venue);
        }

        // GET: Venues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            Console.WriteLine("GET Delete called");

            if (id == null)
            {
                Console.WriteLine("DELETE: ID NULL");
                return NotFound();
            }

            var venue = await _context.Venues
                .FirstOrDefaultAsync(m => m.VenueId == id);

            if (venue == null)
            {
                Console.WriteLine("DELETE: VENUE NOT FOUND");
                return NotFound();
            }

            return View(venue);
        }

        // POST: Venues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Console.WriteLine("POST Delete called");

            var venue = await _context.Venues.FindAsync(id);

            if (venue != null)
            {
                Console.WriteLine("DELETE: REMOVING VENUE");
                _context.Venues.Remove(venue);
            }

            await _context.SaveChangesAsync();
            Console.WriteLine("DELETE SUCCESSFUL");

            return RedirectToAction(nameof(Index));
        }

        private bool VenueExists(int id)
        {
            return _context.Venues.Any(e => e.VenueId == id);
        }
    }
}
