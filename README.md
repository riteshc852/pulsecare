# PulseCare - Smart Clinic & Patient Appointment Management System

A full-featured Web Application developed using **ASP.NET Core MVC**, **Entity Framework Core**, and **SQLite**, designed to digitize healthcare clinic operations and streamline doctor appointment bookings with zero configuration setup.

---

## 🎯 Academic Project Details
- **Subject:** Web Technologies with .NET
- **Evaluation Criteria:**
  - ✅ **MVC Architecture:** Strict separation of Models, Razor Views, and Controllers.
  - ✅ **Database Connectivity:** Entity Framework Core ORM with SQLite (`Microsoft.EntityFrameworkCore.Sqlite`). Portable, self-contained file database (`pulsecare.db`).
  - ✅ **CRUD Operations:** Complete Create, Read, Update, and Delete operations for Appointments and Doctors.
  - ✅ **Form Handling & Model Validation:** Comprehensive Data Annotations (`[Required]`, `[EmailAddress]`, `[Phone]`, `[StringLength]`, `[Range]`, custom business validation) with both client-side and server-side checks.
  - ✅ **State Management:** Session management (`HttpContext.Session`) for user roles/auth, and `TempData` flash messages for real-time user notifications.
  - ✅ **Responsive UI:** Modern, clean medical interface powered by Bootstrap 5 and Bootstrap Icons.

---

## 🏗️ System Architecture
```
┌────────────────────────────────────────────────────────┐
│                   PRESENTATION LAYER                   │
│   Razor Views (.cshtml), Bootstrap 5, HTML5/CSS3       │
└──────────────────────────┬─────────────────────────────┘
                           │ (HTTP GET / POST / AntiForgery)
                           ▼
┌────────────────────────────────────────────────────────┐
│                    BUSINESS LAYER                      │
│   ASP.NET Core MVC Controllers (Home, Appointments,    │
│   Doctors, Account) + Session State Management         │
└──────────────────────────┬─────────────────────────────┘
                           │ (LINQ / EF Core Queries)
                           ▼
┌────────────────────────────────────────────────────────┐
│                   PERSISTENCE LAYER                    │
│   Entity Framework Core (ApplicationDbContext)         │
│   Models: Doctor, Patient, Appointment, Department     │
└──────────────────────────┬─────────────────────────────┘
                           │ (EF Core SQLite Provider)
                           ▼
┌────────────────────────────────────────────────────────┐
│                    DATABASE LAYER                      │
│   SQLite Database File (`pulsecare.db`)                │
└────────────────────────────────────────────────────────┘
```

---

## 🚀 Key Modules
1. **Clinic Operational Dashboard:** Real-time statistics tracking total appointments, active doctor count, pending confirmations, and consultation revenue.
2. **Specialist Doctor Directory:** Categorized by medical departments (Cardiology, Dermatology, Orthopedics, Pediatrics, General Medicine) with fee details and availability schedules.
3. **Appointment Scheduling Engine:** Slot selection, date picking, patient details capture, and automatic token assignment.
4. **Appointment Management (CRUD):** Status tracking (`Pending` -> `Confirmed` -> `Completed` -> `Cancelled`), doctor clinical notes/prescriptions, and printable consultation slips.
5. **State Management & Authentication:** Session-based authentication storing user roles (`Admin`, `Doctor`, `Patient`) and TempData alerts.

---

## ⚙️ Running the Project
1. **Execute:**
   ```bash
   dotnet run
   ```
2. **Access the Portal in Browser:**
   - [https://localhost:7001](https://localhost:7001)
   - [http://localhost:5000](http://localhost:5000)

**Default Admin Credentials:**
- **Username:** `admin`
- **Password:** `admin123`
