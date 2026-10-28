using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PulseCareClinic.Data;
using PulseCareClinic.Models;
using PulseCareClinic.ViewModels;

namespace PulseCareClinic.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index(string? statusFilter, string? searchString)
        {
            var query = _context.Appointments
                .Include(a => a.Doctor)
                    .ThenInclude(d => d!.Department)
                .Include(a => a.Patient)
                .AsQueryable();

            if (!string.IsNullOrEmpty(statusFilter) && Enum.TryParse<AppointmentStatus>(statusFilter, out var parsedStatus))
            {
                query = query.Where(a => a.Status == parsedStatus);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(a => a.Patient!.FullName.Contains(searchString) ||
                                         a.Doctor!.FullName.Contains(searchString) ||
                                         a.ReasonForVisit.Contains(searchString));
            }

            ViewBag.CurrentFilter = statusFilter;
            ViewBag.CurrentSearch = searchString;

            var appointments = await query.OrderByDescending(a => a.AppointmentDate).ToListAsync();
            return View(appointments);
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                    .ThenInclude(d => d!.Department)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (appointment == null) return NotFound();

            return View(appointment);
        }

        // GET: Appointments/Create
        public async Task<IActionResult> Create(int? selectedDoctorId)
        {
            var doctors = await _context.Doctors.Include(d => d.Department).ToListAsync();

            var viewModel = new AppointmentCreateViewModel
            {
                AppointmentDate = DateTime.Today.AddDays(1),
                DoctorId = selectedDoctorId ?? (doctors.FirstOrDefault()?.Id ?? 0),
                DoctorsList = doctors.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = $"{d.FullName} ({d.Department?.Name} - ₹{d.ConsultationFee})"
                }),
                TimeSlotsList = GetAvailableTimeSlots()
            };

            return View(viewModel);
        }

        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentCreateViewModel model)
        {
            // Business rule validation: Cannot book appointments in the past
            if (model.AppointmentDate < DateTime.Today)
            {
                ModelState.AddModelError("AppointmentDate", "Appointment date cannot be in the past.");
            }

            if (ModelState.IsValid)
            {
                // Find or create patient record by email
                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Email == model.PatientEmail);
                if (patient == null)
                {
                    patient = new Patient
                    {
                        FullName = model.PatientName,
                        Email = model.PatientEmail,
                        PhoneNumber = model.PatientPhone,
                        DateOfBirth = DateTime.Today.AddYears(-25),
                        Gender = "Not Specified"
                    };
                    _context.Patients.Add(patient);
                    await _context.SaveChangesAsync();
                }

                // Create appointment
                var appointment = new Appointment
                {
                    DoctorId = model.DoctorId,
                    PatientId = patient.Id,
                    AppointmentDate = model.AppointmentDate,
                    TimeSlot = model.TimeSlot,
                    ReasonForVisit = model.ReasonForVisit,
                    Status = AppointmentStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                // State Management: Set TempData flash alert
                TempData["SuccessMessage"] = $"Appointment booked successfully for {patient.FullName}! Your Booking Reference ID is #{appointment.Id}.";

                return RedirectToAction(nameof(Index));
            }

            // Re-populate lists if validation fails
            var doctors = await _context.Doctors.Include(d => d.Department).ToListAsync();
            model.DoctorsList = doctors.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = $"{d.FullName} ({d.Department?.Name} - ₹{d.ConsultationFee})"
            });
            model.TimeSlotsList = GetAvailableTimeSlots();

            TempData["ErrorMessage"] = "Please correct the form validation errors below.";
            return View(model);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null) return NotFound();

            ViewBag.Doctors = new SelectList(_context.Doctors, "Id", "FullName", appointment.DoctorId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Appointment appointment)
        {
            if (id != appointment.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"Appointment #{appointment.Id} updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Doctors = new SelectList(_context.Doctors, "Id", "FullName", appointment.DoctorId);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (appointment == null) return NotFound();

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Appointment #{id} was successfully cancelled and deleted.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }

        private List<SelectListItem> GetAvailableTimeSlots()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "09:00 AM - 09:30 AM", Text = "09:00 AM - 09:30 AM" },
                new SelectListItem { Value = "10:00 AM - 10:30 AM", Text = "10:00 AM - 10:30 AM" },
                new SelectListItem { Value = "11:00 AM - 11:30 AM", Text = "11:00 AM - 11:30 AM" },
                new SelectListItem { Value = "02:00 PM - 02:30 PM", Text = "02:00 PM - 02:30 PM" },
                new SelectListItem { Value = "03:30 PM - 04:00 PM", Text = "03:30 PM - 04:00 PM" },
                new SelectListItem { Value = "05:00 PM - 05:30 PM", Text = "05:00 PM - 05:30 PM" }
            };
        }
    }
}
