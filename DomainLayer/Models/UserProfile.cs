namespace RentalSystem.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Passport { get; set; }
        public int DrivingExperience { get; set; }
        public string? Description { get; set; }     
        public string? ProfileImage { get; set; }     

        public User? User { get; set; }
        public int UserId { get; set; }
    }
}
