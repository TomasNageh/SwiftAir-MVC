using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwiftAir.Models
{
    public class Passenger
    {
        public int PassengerId { get; set; }

        [DisplayName("Full Name")]
        [Required(ErrorMessage = "You have to provide a valid full name.")]
        [MinLength(10, ErrorMessage = "Full Name must not be less than 10 characters.")]
        [MaxLength(50, ErrorMessage = "Full Name must not exceed 50 characters.")]
        public string FullName { get; set; }

        [DisplayName("Email Address")]
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Please provide a valid email address.")]
        public string Email { get; set; }

        [DisplayName("Phone Number")]
        [Phone(ErrorMessage = "Please provide a valid phone number.")]
        public string PhoneNumber { get; set; }

        [DisplayName("Passport Number")]
        [Required(ErrorMessage = "Passport number is required.")]
        [StringLength(20, ErrorMessage = "Passport number must not exceed 20 characters.")]
        public string PassportNumber { get; set; }

        [DisplayName("Date of Birth")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateTime DateOfBirth { get; set; }

        public  int FlightId { get; set; }

        //Navigation Property
        [DeleteBehavior(DeleteBehavior.NoAction)]
        [ValidateNever]
        public Flight Flight { get; set; }

    }
}
