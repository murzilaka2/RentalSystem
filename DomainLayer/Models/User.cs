namespace RentalSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string? FullName { get; set; }
        public string HashPassword { get; set; }
        public string Salt { get; set; }

        public Role? Role { get; set; }
        public int RoleId { get; set; }
        public UserProfile? Profile { get; set; }
    }
}
