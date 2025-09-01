using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwiftAir.Models
{
    public class Flight
    {
        public int Id { get; set; }

        public string FlightNumber { get; set; }   // e.g., "SA123"
        public string Departure { get; set; }      // e.g., "Cairo"
        public string Destination { get; set; }    // e.g., "London"
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        public decimal TicketPrice { get; set; }

        [ValidateNever]
        public List<Passenger> Passengers { get; set; } // Navigation Property

        [DisplayName("Flight Status")]
        public string Status { get; set; }

        [ValidateNever]
        public string? ImagePath { get; set; }

        [DisplayName("Ticket Image")]
        [ValidateNever]
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
