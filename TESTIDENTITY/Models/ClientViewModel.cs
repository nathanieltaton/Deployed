﻿using System.ComponentModel.DataAnnotations;

namespace TESTIDENTITY.Models
{
    public class ClientViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Please Enter First Name")]
        public string FirstName { get; set; }
		[Required (ErrorMessage = "Please Enter Last Name")]
		public string LastName { get; set; }
        [Required]
		[Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
		public int PhoneNumber { get; set; }
		[Required(ErrorMessage = "Please Enter Email")]
        [DataType(DataType.EmailAddress,ErrorMessage ="Please enter a valid email")]
		public string? EmailAddress { get; set; }
        public string? Address { get; set; }
        public string? Notes { get; set; }
        public DateTime? Birthday { get; set; }
        public int UserType { get; set; }
    }
}
