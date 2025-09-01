using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SwiftAir.Data;
using SwiftAir.Models;

namespace SwiftAir.Controllers
{
    public class FlightsController : Controller
    {

        ApplicationDbContext _context = new ApplicationDbContext();

        IWebHostEnvironment webHostEnvironment;

        public FlightsController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET: Flights
        public async Task<IActionResult> Index()
        {
            return View(await _context.Flights.ToListAsync());
        }

        // GET: Flights/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // GET: Flights/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Flights/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Flight flight) // Removed [Bind(...)] so ImageFile is included
        {
            if (ModelState.IsValid)
            {
                if (flight.ImageFile == null)
                {
                    flight.ImagePath = "\\images\\NO BOARDING PASS.png";
                }
                else
                {
                    // Globally Unique Identifier
                    Guid imgGuid = Guid.NewGuid();
                    string imgExtension = Path.GetExtension(flight.ImageFile.FileName);
                    //                 Guid   +   .png or .jpg
                    string imgName = imgGuid + imgExtension;
                    flight.ImagePath = "\\images\\tickets\\" + imgName;

                    string imgFullPath = Path.Combine(webHostEnvironment.WebRootPath, "images", "tickets", imgName);
                    using (var fileStream = new FileStream(imgFullPath, FileMode.Create))
                    {
                        flight.ImageFile.CopyTo(fileStream);
                    }
                }

                _context.Add(flight);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(flight);
        }

        // GET: Flights/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound();
            }
            return View(flight);
        }

        // POST: Flights/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Flight flight) // Removed [Bind(...)] so ImageFile is included
        {
            if (id != flight.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingFlight = await _context.Flights.FindAsync(id);
                    if (existingFlight == null)
                    {
                        return NotFound();
                    }

                    // Update text fields
                    existingFlight.FlightNumber = flight.FlightNumber;
                    existingFlight.Departure = flight.Departure;
                    existingFlight.Destination = flight.Destination;
                    existingFlight.DepartureTime = flight.DepartureTime;
                    existingFlight.ArrivalTime = flight.ArrivalTime;
                    existingFlight.TicketPrice = flight.TicketPrice;
                    existingFlight.Status = flight.Status;

                    // Handle image
                    if (flight.ImageFile != null)
                    {
                        Guid imgGuid = Guid.NewGuid();
                        string imgExtension = Path.GetExtension(flight.ImageFile.FileName);
                        string imgName = imgGuid + imgExtension;
                        existingFlight.ImagePath = "\\images\\tickets\\" + imgName;

                        string imgFullPath = Path.Combine(webHostEnvironment.WebRootPath, "images", "tickets", imgName);
                        using (var fileStream = new FileStream(imgFullPath, FileMode.Create))
                        {
                            flight.ImageFile.CopyTo(fileStream);
                        }
                    }

                    _context.Update(existingFlight);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlightExists(flight.Id))
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
            return View(flight);
        }

        // GET: Flights/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // POST: Flights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flight = await _context.Flights.FindAsync(id);

            if (flight.ImagePath != "\\images\\NO BOARDING PASS.png")
            {
                string imgFullPath = webHostEnvironment.WebRootPath + flight.ImagePath;
                if (System.IO.File.Exists(imgFullPath))
                {
                    System.IO.File.Delete(imgFullPath);
                }
            }

            if (flight != null)
            {
                _context.Flights.Remove(flight);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlightExists(int id)
        {
            return _context.Flights.Any(e => e.Id == id);
        }
    }
}
