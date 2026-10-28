using System.ComponentModel.DataAnnotations;

namespace PulseCareClinic.Models
{
    public enum UserRole
    {
        Admin,
        Doctor,
        Patient
    }

    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; } = UserRole.Patient;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
