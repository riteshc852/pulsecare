using PulseCareClinic.Models;

namespace PulseCareClinic.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Ensure database is created
            context.Database.EnsureCreated();

            // Check if already seeded
            if (context.Departments.Any())
            {
                return; // DB has been seeded
            }

            // 1. Seed Departments
            var departments = new Department[]
            {
                new Department { Name = "General Medicine", Description = "Primary care, health screenings, and seasonal illnesses." },
                new Department { Name = "Cardiology", Description = "Heart care, blood pressure management, and cardiovascular health." },
                new Department { Name = "Dermatology", Description = "Skin, hair, and cosmetic medical consultations." },
                new Department { Name = "Pediatrics", Description = "Comprehensive child health, growth monitoring, and vaccinations." },
                new Department { Name = "Orthopedics", Description = "Bone, joint, spine, and sports injury treatments." }
            };
            context.Departments.AddRange(departments);
            context.SaveChanges();

            // 2. Seed Doctors
            var doctors = new Doctor[]
            {
                new Doctor
                {
                    FullName = "Dr. Rajesh Sharma",
                    Specialization = "Senior Physician",
                    DepartmentId = departments[0].Id,
                    Email = "rajesh.sharma@pulsecare.com",
                    PhoneNumber = "+91 98251 12345",
                    ConsultationFee = 500.00m,
                    AvailableDays = "Mon, Tue, Wed, Thu, Fri"
                },
                new Doctor
                {
                    FullName = "Dr. Priya Desai",
                    Specialization = "Consultant Cardiologist",
                    DepartmentId = departments[1].Id,
                    Email = "priya.desai@pulsecare.com",
                    PhoneNumber = "+91 98252 23456",
                    ConsultationFee = 800.00m,
                    AvailableDays = "Mon, Wed, Fri"
                },
                new Doctor
                {
                    FullName = "Dr. Amit Patel",
                    Specialization = "Dermatologist & Cosmetologist",
                    DepartmentId = departments[2].Id,
                    Email = "amit.patel@pulsecare.com",
                    PhoneNumber = "+91 98253 34567",
                    ConsultationFee = 600.00m,
                    AvailableDays = "Tue, Thu, Sat"
                },
                new Doctor
                {
                    FullName = "Dr. Sneha Verma",
                    Specialization = "Child Specialist (Pediatrician)",
                    DepartmentId = departments[3].Id,
                    Email = "sneha.verma@pulsecare.com",
                    PhoneNumber = "+91 98254 45678",
                    ConsultationFee = 550.00m,
                    AvailableDays = "Mon, Tue, Thu, Fri"
                },
                new Doctor
                {
                    FullName = "Dr. Vikram Joshi",
                    Specialization = "Orthopedic Surgeon",
                    DepartmentId = departments[4].Id,
                    Email = "vikram.joshi@pulsecare.com",
                    PhoneNumber = "+91 98255 56789",
                    ConsultationFee = 750.00m,
                    AvailableDays = "Wed, Fri, Sat"
                }
            };
            context.Doctors.AddRange(doctors);
            context.SaveChanges();

            // 3. Seed Patients
            var patients = new Patient[]
            {
                new Patient
                {
                    FullName = "Rahul Mehra",
                    Email = "rahul.mehra@example.com",
                    PhoneNumber = "+91 99090 11223",
                    DateOfBirth = new DateTime(1995, 4, 12),
                    Gender = "Male",
                    BloodGroup = "B+",
                    Address = "Ring Road, Surat, Gujarat"
                },
                new Patient
                {
                    FullName = "Ananya Iyer",
                    Email = "ananya.iyer@example.com",
                    PhoneNumber = "+91 98111 22334",
                    DateOfBirth = new DateTime(1998, 9, 23),
                    Gender = "Female",
                    BloodGroup = "O+",
                    Address = "Station Road, Bardoli, Gujarat"
                }
            };
            context.Patients.AddRange(patients);
            context.SaveChanges();

            // 4. Seed Appointments
            var appointments = new Appointment[]
            {
                new Appointment
                {
                    DoctorId = doctors[0].Id,
                    PatientId = patients[0].Id,
                    AppointmentDate = DateTime.Today.AddDays(1),
                    TimeSlot = "10:00 AM - 10:30 AM",
                    ReasonForVisit = "Seasonal flu symptoms and persistent cough.",
                    Status = AppointmentStatus.Confirmed,
                    Notes = "Prescribed rest and hydration."
                },
                new Appointment
                {
                    DoctorId = doctors[1].Id,
                    PatientId = patients[1].Id,
                    AppointmentDate = DateTime.Today.AddDays(2),
                    TimeSlot = "02:00 PM - 02:30 PM",
                    ReasonForVisit = "Routine blood pressure checkup and ECG evaluation.",
                    Status = AppointmentStatus.Pending
                }
            };
            context.Appointments.AddRange(appointments);

            // 5. Seed Default Admin User
            var adminUser = new User
            {
                Username = "admin",
                Email = "admin@pulsecare.com",
                PasswordHash = "admin123", // In production hash with BCrypt / ASP.NET Identity
                Role = UserRole.Admin
            };
            context.Users.Add(adminUser);

            context.SaveChanges();
        }
    }
}
