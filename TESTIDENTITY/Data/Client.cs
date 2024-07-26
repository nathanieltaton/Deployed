namespace TESTIDENTITY.Data
{
    public class Client
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string? Address { get; set; }
        public string? Notes { get; set; }
        public DateTime? Birthday { get; set; }
        public bool IsAdmin { get; set; }
    }
}
