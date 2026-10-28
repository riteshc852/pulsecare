using PulseCareClinic.Models;

namespace PulseCareClinic.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalAppointments { get; set; }
        public int PendingAppointments { get; set; }
        public int ConfirmedAppointments { get; set; }
        public int CompletedAppointments { get; set; }
        public int TotalDoctors { get; set; }
        public int TotalPatients { get; set; }
        public decimal TotalConsultationRevenue { get; set; }

        public IEnumerable<Appointment> RecentAppointments { get; set; } = new List<Appointment>();
        public IEnumerable<Doctor> FeaturedDoctors { get; set; } = new List<Doctor>();
    }
}
