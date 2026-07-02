using Clinc.Data;
using Clinc.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clinc.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class ReportsController : Controller
    {
        private readonly AppDbcontext _context;

        public ReportsController(AppDbcontext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DoctorReport()
        {
            var vm = new DoctorReportVM
            {
                Doctors = _context.Doctors.ToList()
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult DoctorReport(DoctorReportVM vm)
        {
            vm.Doctors = _context.Doctors.ToList();

            var query = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.DoctorId == vm.DoctorId);

            if (vm.FromDate.HasValue)
            {
                query = query.Where(a =>
                    a.AppointmentDate >= vm.FromDate.Value);
            }

            if (vm.ToDate.HasValue)
            {
                query = query.Where(a =>
                    a.AppointmentDate <= vm.ToDate.Value);
            }

            vm.Appointments = query.ToList();

            vm.TotalAppointments = vm.Appointments.Count;

            vm.TotalPatients = vm.Appointments
                .Select(a => a.PatientId)
                .Distinct()
                .Count();

            return View(vm);
        }

        [HttpGet]
        public IActionResult PatientReport()
        {
            var vm = new PatientReportVM
            {
                Patients = _context.Patients.ToList()
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult PatientReport(PatientReportVM vm)
        {
            vm.Patients = _context.Patients.ToList();

            vm.Patient = _context.Patients
                .FirstOrDefault(p => p.Id == vm.PatientId);

            vm.Appointments = _context.Appointments
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == vm.PatientId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToList();

            return View(vm);
        }
        [HttpGet]
        public IActionResult AppointmentsReport()
        {
            return View(new AppointmentsReportVM());
        }

        [HttpPost]
        public IActionResult AppointmentsReport(AppointmentsReportVM vm)
        {
            var query = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .AsQueryable();

            if (vm.FromDate.HasValue)
            {
                query = query.Where(a =>
                    a.AppointmentDate >= vm.FromDate.Value);
            }

            if (vm.ToDate.HasValue)
            {
                query = query.Where(a =>
                    a.AppointmentDate <= vm.ToDate.Value);
            }

            vm.Appointments = query.ToList();

            return View(vm);
        }
        public IActionResult TodayAppointments()
        {
            var today = DateTime.Today;

            var appointments = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.AppointmentDate.Date == today)
                .ToList();

            return View(appointments);
        }
        public IActionResult PendingAppointments()
        {
            var appointments = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.Status == "Pending")
                .ToList();

            return View(appointments);
        }
        public IActionResult Dashboard()
        {
            DashboardVM vm = new DashboardVM();

            vm.DoctorsCount = _context.Doctors.Count();

            vm.PatientsCount = _context.Patients.Count();

            vm.AppointmentsCount = _context.Appointments.Count();

            return View(vm);
        }
        public IActionResult Statistics()
        {
            var today = DateTime.Today;

            var vm = new StatisticsVM
            {
                DoctorsCount = _context.Doctors.Count(),

                PatientsCount = _context.Patients.Count(),

                AppointmentsCount = _context.Appointments.Count(),

                TodayAppointments = _context.Appointments
                    .Count(a => a.AppointmentDate.Date == today),

                CompletedAppointments = _context.Appointments
                    .Count(a => a.Status == "Completed"),

                CancelledAppointments = _context.Appointments
                    .Count(a => a.Status == "Cancelled")
            };

            return View(vm);
        }
    }
}