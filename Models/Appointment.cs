using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PulseCareClinic.Models
{
    public enum AppointmentStatus
    {
        Pending,
        Confirmed,
        Completed,
        Cancelled
    }

    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Appointment Date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Appointment Date")]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Please select a time slot.")]
        [Display(Name = "Time Slot")]
        public string TimeSlot { get; set; } = string.Empty;

        [Required(ErrorMessage = "Reason for visit is required.")]
        [StringLength(250, ErrorMessage = "Reason cannot exceed 250 characters.")]
        [Display(Name = "Reason for Visit")]
        public string ReasonForVisit { get; set; } = string.Empty;

        [Display(Name = "Status")]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        [StringLength(500)]
        [Display(Name = "Doctor Notes / Prescription")]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key: Doctor
        [Required(ErrorMessage = "Please select a doctor.")]
        [Display(Name = "Doctor")]
        public int DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        public virtual Doctor? Doctor { get; set; }

        // Foreign Key: Patient
        [Required(ErrorMessage = "Please select a patient.")]
        [Display(Name = "Patient")]
        public int PatientId { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient? Patient { get; set; }
    }
}
