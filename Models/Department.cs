using System.ComponentModel.DataAnnotations;

namespace PulseCareClinic.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Department Name is required.")]
        [StringLength(100, ErrorMessage = "Department Name cannot exceed 100 characters.")]
        [Display(Name = "Department Name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(300, ErrorMessage = "Description cannot exceed 300 characters.")]
        public string? Description { get; set; }

        // Navigation property
        public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
