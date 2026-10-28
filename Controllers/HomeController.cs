using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PulseCareClinic.Data;
using PulseCareClinic.ViewModels;

namespace PulseCareClinic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var doctors = await _context.Doctors.Include(d => d.Department).ToListAsync();
            var appointments = await _context.Appointments.Include(a => a.Doctor).Include(a => a.Patient).ToListAsync();

            var viewModel = new DashboardViewModel
            {
                TotalDoctors = doctors.Count,
                TotalPatients = await _context.Patients.CountAsync(),
                TotalAppointments = appointments.Count,
                PendingAppointments = appointments.Count(a => a.Status == Models.AppointmentStatus.Pending),
                ConfirmedAppointments = appointments.Count(a => a.Status == Models.AppointmentStatus.Confirmed),
                CompletedAppointments = appointments.Count(a => a.Status == Models.AppointmentStatus.Completed),
                TotalConsultationRevenue = appointments.Where(a => a.Status == Models.AppointmentStatus.Completed).Sum(a => a.Doctor?.ConsultationFee ?? 0),
                FeaturedDoctors = doctors.Take(4),
                RecentAppointments = appointments.OrderByDescending(a => a.CreatedAt).Take(5)
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
