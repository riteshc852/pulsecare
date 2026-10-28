using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PulseCareClinic.Models
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Doctor name is required.")]
        [StringLength(100, ErrorMessage = "Doctor name cannot exceed 100 characters.")]
        [Display(Name = "Doctor Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Specialization is required.")]
        [StringLength(100)]
        public string Specialization { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact number is required.")]
        [Phone(ErrorMessage = "Invalid Phone Number.")]
        [Display(Name = "Contact Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Consultation fee is required.")]
        [Range(100, 5000, ErrorMessage = "Fee must be between ₹100 and ₹5,000.")]
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Consultation Fee (₹)")]
        public decimal ConsultationFee { get; set; }

        [Required(ErrorMessage = "Please select available working days.")]
        [Display(Name = "Available Days")]
        public string AvailableDays { get; set; } = "Mon, Wed, Fri";

        // Foreign Key
        [Required(ErrorMessage = "Department is required.")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department? Department { get; set; }

        // Navigation property
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
