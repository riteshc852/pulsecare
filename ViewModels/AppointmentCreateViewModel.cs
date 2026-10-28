using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using PulseCareClinic.Models;

namespace PulseCareClinic.ViewModels
{
    public class AppointmentCreateViewModel
    {
        [Required(ErrorMessage = "Appointment Date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Appointment Date")]
        public DateTime AppointmentDate { get; set; } = DateTime.Today.AddDays(1);

        [Required(ErrorMessage = "Please select a time slot.")]
        [Display(Name = "Preferred Time Slot")]
        public string TimeSlot { get; set; } = string.Empty;

        [Required(ErrorMessage = "Reason for visit is required.")]
        [StringLength(250, ErrorMessage = "Reason cannot exceed 250 characters.")]
        [Display(Name = "Symptoms / Reason for Visit")]
        public string ReasonForVisit { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please choose a doctor.")]
        [Display(Name = "Consulting Doctor")]
        public int DoctorId { get; set; }

        // Patient details (for quick registration or selection)
        [Required(ErrorMessage = "Patient Full Name is required.")]
        [StringLength(100)]
        [Display(Name = "Patient Full Name")]
        public string PatientName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Patient Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        [Display(Name = "Patient Email")]
        public string PatientEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone Number is required.")]
        [Phone(ErrorMessage = "Enter a valid phone number.")]
        [Display(Name = "Phone Number")]
        public string PatientPhone { get; set; } = string.Empty;

        // Select lists for dropdown binding
        public IEnumerable<SelectListItem>? DoctorsList { get; set; }
        public IEnumerable<SelectListItem>? TimeSlotsList { get; set; }
    }
}
