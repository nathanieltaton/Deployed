using System.ComponentModel.DataAnnotations;

namespace TESTIDENTITY.Models
{
    public class AddClientViewModel
    {
		[Required(ErrorMessage = "Please Enter First Name")]
		public string FirstName { get; set; }
		[Required(ErrorMessage = "Please Enter Last Name")]
		public string LastName { get; set; }
		[Required]
		[DataType(DataType.PhoneNumber,ErrorMessage ="Please enter a valid phone number")]
		public int PhoneNumber { get; set; }
		[Required(ErrorMessage = "Please Enter Email")]
		[DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email")]
		public string EmailAddress { get; set; }
		public string? Address { get; set; }
		public string? Notes { get; set; }
		[DataType(DataType.DateTime)]
        public DateTime? Birthday { get; set; }
        public bool IsAdmin { get; set; }
    }
}
