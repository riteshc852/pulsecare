using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PulseCareClinic.Data;
using PulseCareClinic.Models;

namespace PulseCareClinic.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Doctors
        public async Task<IActionResult> Index(int? departmentId)
        {
            var departments = await _context.Departments.ToListAsync();
            ViewBag.Departments = departments;
            ViewBag.SelectedDepartment = departmentId;

            var query = _context.Doctors.Include(d => d.Department).AsQueryable();

            if (departmentId.HasValue && departmentId.Value > 0)
            {
                query = query.Where(d => d.DepartmentId == departmentId.Value);
            }

            var doctors = await query.ToListAsync();
            return View(doctors);
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var doctor = await _context.Doctors
                .Include(d => d.Department)
                .Include(d => d.Appointments)
                    .ThenInclude(a => a.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (doctor == null) return NotFound();

            return View(doctor);
        }

        // GET: Doctors/Create
        public IActionResult Create()
        {
            // Simple role/session check demonstration
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Admin")
            {
                TempData["ErrorMessage"] = "Only administrators can add new doctors. Please login as Admin.";
                return RedirectToAction("Login", "Account");
            }

            ViewBag.DepartmentId = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        // POST: Doctors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(doctor);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Doctor '{doctor.FullName}' added successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.DepartmentId = new SelectList(_context.Departments, "Id", "Name", doctor.DepartmentId);
            return View(doctor);
        }
    }
}
