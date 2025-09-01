using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SwiftAir.Data;
using SwiftAir.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SwiftAir.Data;
using SwiftAir.Models;

namespace SwiftAir.Controllers
{
    public class PassengersController : Controller
    {
        private readonly ApplicationDbContext Context;

        public PassengersController()
        {
            Context = new ApplicationDbContext();
        }

        // ----------------------- INDEX + SEARCH -----------------------

        [HttpGet]
        public IActionResult GetIndexView(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                // Eagerly load Flight navigation property
                return View("Index", Context.Passengers.Include(p => p.Flight).ToList());
            }
            else
            {
                var passengers = Context.Passengers
                    .Include(p => p.Flight) // ADD THIS LINE  Flight Status
                    .Where(p => p.FullName.Contains(search) ||
                                p.Email.Contains(search) ||
                                p.PassportNumber.Contains(search))
                    .ToList();

                ViewBag.CurrentSearch = search;
                return View("Index", passengers);
            }
        }

        [HttpGet]
        public IActionResult Index(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return View(Context.Passengers.Include(p => p.Flight).ToList()); // ADD .Include(p => p.Flight)
            }
            else
            {
                List<Passenger> filteredPassengers =
                    Context.Passengers
                    .Include(p => p.Flight) // ADD THIS LINE        Flight Status
                    .Where(e => e.FullName.Contains(search) ||
                               e.Email.Contains(search))
                    .ToList();
                ViewBag.CurrentSearch = search;
                return View(filteredPassengers);
            }
        }
        // ----------------------- DETAILS -----------------------

        [HttpGet]
        public IActionResult GetDetailsView(int id)
        {
            Passenger passenger = Context.Passengers.FirstOrDefault(p => p.PassengerId == id);
            if (passenger is null)
            {
                return NotFound();
            }
            return View("Details", passenger);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            Passenger passenger = Context.Passengers
                .Include(p => p.Flight)
                .FirstOrDefault(p => p.PassengerId == id);
            if (passenger is null)
            {
                return NotFound();
            }
            return View(passenger);
        }

        // ----------------------- CREATE -----------------------

        [HttpGet]
        public IActionResult GetCreateView()
        {
            ViewBag.FlightsSelectList = new SelectList(
               Context.Flights.Select(f => new
               {
                   f.Id,
                   DisplayText = $"{f.FlightNumber} — {f.Departure} → {f.Destination} — {f.DepartureTime:dd/MM/yyyy HH:mm}"
               }),
               "Id",
               "DisplayText"
           );
            return View("Create");
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.FlightsSelectList = new SelectList(
                Context.Flights.Select(f => new
                {
                    f.Id,
                    DisplayText = $"{f.FlightNumber} — {f.Departure} → {f.Destination} — {f.DepartureTime:dd/MM/yyyy HH:mm}"
                }),
                "Id",
                "DisplayText"
            );

            return View();
        }



        [HttpPost]
        public IActionResult AddNew(Passenger passenger)
        {
            // Age validation
            if (!IsAbove18(passenger.DateOfBirth))
            {
                ModelState.AddModelError("DateOfBirth", "Passenger must be above 18 to book a ticket.");
            }

            if (!Context.Flights.Any(f => f.Id == passenger.FlightId))
            {
                ModelState.AddModelError("FlightId", "Selected flight does not exist.");
            }

            if (ModelState.IsValid)
            {
                Context.Passengers.Add(passenger);
                Context.SaveChanges();
                return RedirectToAction("GetIndexView");
            }
            return View("Create", passenger);
        }

        // ----------------------- EDIT -----------------------

        [HttpGet]
        public IActionResult GetEditView(int id)
        {
            Passenger passenger = Context.Passengers.Find(id);
            if (passenger is null)
            {
                return NotFound();
            }
            ViewBag.FlightsSelectList = new SelectList(Context.Flights, "Id", "FlightNumber");
            return View("Edit", passenger);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Passenger passenger = Context.Passengers.Find(id);
            if (passenger is null)
            {
                return NotFound();
            }
            ViewBag.FlightsSelectList = new SelectList(Context.Flights, "Id", "FlightNumber");
            return View(passenger);
        }

        [HttpPost]
        public IActionResult EditCurrent(Passenger passenger)
        {
            // Age validation
            if (!IsAbove18(passenger.DateOfBirth))
            {
                ModelState.AddModelError("DateOfBirth", "Passenger must be above 18 to book a ticket.");
            }

            if (ModelState.IsValid)
            {
                Context.Passengers.Update(passenger);
                Context.SaveChanges();
                return RedirectToAction("GetIndexView");
            }
            return View("Edit", passenger);
        }

        // ----------------------- DELETE -----------------------

        [HttpGet]
        public IActionResult GetDeleteView(int id)
        {
            Passenger passenger = Context.Passengers.Include(e => e.Flight).FirstOrDefault(p => p.PassengerId == id);
            if (passenger is null)
            {
                return NotFound();
            }
            ViewBag.FlightsSelectList = new SelectList(Context.Flights, "Id", "FlightNumber");
            return View("Delete", passenger);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Passenger passenger = Context.Passengers.Include(e => e.Flight).FirstOrDefault(p => p.PassengerId == id);
            if (passenger is null)
            {
                return NotFound();
            }
            ViewBag.FlightsSelectList = new SelectList(Context.Flights, "Id", "FlightNumber");
            return View(passenger);
        }

        [HttpPost]
        public IActionResult DeleteCurrent(int id)
        {
            Passenger passenger = Context.Passengers.Find(id);
            Context.Passengers.Remove(passenger);
            Context.SaveChanges();

            return RedirectToAction("GetIndexView");
        }

        // ----------------------- Above(+18) METHOD -----------------------

        private bool IsAbove18(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            int age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age)) age--;
            return age >= 18;
        }
    }
}
