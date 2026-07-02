using Clinc.Data;
using Clinc.Models;
using Clinc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Clinc.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbcontext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(AppDbcontext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var today = DateTime.Today;

            // ãæÇÚíÏ Çáíæã
            var todayAppointments = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.AppointmentDate.Date == today)
                .OrderBy(a => a.Doctor.Doctor_Name)
                .ThenBy(a => a.AppointmentDate)
                .ToList();

            var vm = new DashboardVM
            {
                // ÅÍÕÇÆíÇÊ Çáíæã ÝÞØ
                AppointmentsCount = todayAppointments.Count,

                PatientsCount = todayAppointments
                    .Select(a => a.PatientId)
                    .Distinct()
                    .Count(),

                DoctorsCount = todayAppointments
                    .Select(a => a.DoctorId)
                    .Distinct()
                    .Count(),

                TodayAppointments = todayAppointments,

                TodayAppointmentsCount = todayAppointments.Count,

                ConfirmedAppointmentsCount = todayAppointments
                    .Count(a => a.Status == "Confirmed"),

                CancelledAppointmentsCount = todayAppointments
                    .Count(a => a.Status == "Cancelled"),

                // ÊÌãíÚ ÇáãæÇÚíÏ ÍÓÈ ÇáÏßÊæÑ
                DoctorAppointments = todayAppointments
                    .GroupBy(a => a.Doctor?.Doctor_Name ?? "ÈÏæä ÏßÊæÑ")
                    .ToDictionary(
                        g => g.Key,
                        g => g.ToList()
                    )
            };

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}