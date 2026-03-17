using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CLDV7111_POE_PART_1_EventEase.Data;
using CLDV7111_POE_PART_1_EventEase.Models;

namespace CLDV7111_POE_PART_1_EventEase.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            Console.WriteLine("EVENTS INDEX LOADED");

            var events = _context.Events
                .Include(e => e.Venue);

            return View(await events.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Console.WriteLine("EVENT DETAILS LOADED");

            if (id == null)
            {
                Console.WriteLine("DETAILS: ID NULL");
                return NotFound();
            }

            var eventItem = await _context.Events
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (eventItem == null)
            {
                Console.WriteLine("DETAILS: EVENT NOT FOUND");
                return NotFound();
            }

            return View(eventItem);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            Console.WriteLine("GET Event Create called");

            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName");
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,EventName,EventDate,Description,VenueId")] Event eventItem)
        {
            Console.WriteLine("POST Event Create called");

            if (ModelState.IsValid)
            {
                Console.WriteLine("EVENT MODEL VALID — SAVING");

                _context.Add(eventItem);
                await _context.SaveChangesAsync();

                Console.WriteLine("EVENT SAVE SUCCESSFUL");

                return RedirectToAction(nameof(Index));
            }

            Console.WriteLine("EVENT MODEL INVALID — NOT SAVED");

            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", eventItem.VenueId);
            return View(eventItem);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Console.WriteLine("GET Event Edit called");

            if (id == null)
            {
                Console.WriteLine("EDIT: ID NULL");
                return NotFound();
            }

            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
            {
                Console.WriteLine("EDIT: EVENT NOT FOUND");
                return NotFound();
            }

            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", eventItem.VenueId);
            return View(eventItem);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,EventName,EventDate,Description,VenueId")] Event eventItem)
        {
            Console.WriteLine("POST Event Edit called");

            if (id != eventItem.EventId)
            {
                Console.WriteLine("EDIT: ID MISMATCH");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine("EDIT: MODEL VALID — UPDATING");

                    _context.Update(eventItem);
                    await _context.SaveChangesAsync();

                    Console.WriteLine("EDIT SAVE SUCCESSFUL");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(eventItem.EventId))
                    {
                        Console.WriteLine("EDIT: EVENT NOT FOUND DURING SAVE");
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

            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", eventItem.VenueId);
            return View(eventItem);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            Console.WriteLine("GET Event Delete called");

            if (id == null)
            {
                Console.WriteLine("DELETE: ID NULL");
                return NotFound();
            }

            var eventItem = await _context.Events
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (eventItem == null)
            {
                Console.WriteLine("DELETE: EVENT NOT FOUND");
                return NotFound();
            }

            return View(eventItem);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Console.WriteLine("POST Event Delete called");

            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem != null)
            {
                Console.WriteLine("DELETE: REMOVING EVENT");
                _context.Events.Remove(eventItem);
            }

            await _context.SaveChangesAsync();

            Console.WriteLine("DELETE SUCCESSFUL");

            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }
    }
}
