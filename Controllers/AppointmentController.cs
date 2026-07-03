using Clinc.Data;
using Clinc.Models;
using Clinc.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;

namespace Clinc.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly AppDbcontext _context;

        public AppointmentController(AppDbcontext context)
        {
            _context = context;
        }

        // ===================== INDEX =====================
        public IActionResult Index(int page = 1)
        {
            int pageSize = 5;

            var totalAppointments = _context.Appointments.Count();

            var appointments = _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .OrderByDescending(a => a.AppointmentDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalAppointments / pageSize);

            return View(appointments);
        } 
       

        // ===================== CREATE =====================
        public IActionResult Create()
        {
            var vm = new AppointmentsVM
            {
                Doctors = _context.Doctors.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Doctor_Name
                }).ToList(),

                Patients = _context.Patients.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Patient_Name
                }).ToList(),

                AppointmentDate = DateTime.Now
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AppointmentsVM vm)
        {
            if (ModelState.IsValid)
            {
                vm.IsFollowUp = vm.VisitType == "إعادة";

                var doctor = _context.Doctors.FirstOrDefault(d => d.Id == vm.DoctorId);

                decimal fees = 0;

                if (doctor != null)
                {
                    fees = vm.IsFollowUp
                        ? doctor.ReExaminationFee
                        : doctor.ExaminationFee;
                }
                var appointment = new Appointment
                {
                    DoctorId = vm.DoctorId,
                    PatientId = vm.PatientId,
                    AppointmentDate = vm.AppointmentDate,
                    Status = "Pending",
                    IsFollowUp = vm.IsFollowUp,
                    VisitType = vm.IsFollowUp ? "إعادة كشف" : "كشف جديد",
                    Notes = vm.Notes,
                    Fees = fees
                };

                _context.Appointments.Add(appointment);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            return View(vm);
        }

        // ===================== PAY =====================
        public IActionResult Pay(int id)
        {
            var appointment = _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefault(a => a.Id == id);

            if (appointment == null)
                return NotFound();

            // يسمح فقط لو Pending
            if (appointment.Status != "Pending")
                return RedirectToAction("Index", "Home");

            // منع التكرار
            var existingTransaction = _context.Transactions
                .FirstOrDefault(t => t.AppointmentId == id);

            if (existingTransaction != null)
                return RedirectToAction("Index", "Home");

            appointment.Status = "Paid";

            var transaction = new Transaction
            {
                Amount = appointment.Fees,
                Type = "Income",
                Description = $"دفع كشف الموعد رقم {appointment.Id}",
                DoctorId = appointment.DoctorId,
                PatientId = appointment.PatientId,
                AppointmentId = appointment.Id,
                Date = DateTime.Now
            };

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        // ===================== COMPLETE =====================
        public IActionResult Complete(int id)
        {
            var appointment = _context.Appointments.FirstOrDefault(a => a.Id == id);

            if (appointment == null)
                return NotFound();

            // لازم يكون Paid فقط
            if (appointment.Status != "Paid")
                return RedirectToAction("Index", "Home");

            appointment.Status = "Completed";
            appointment.CompletedAt = DateTime.Now;

            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        // ===================== CANCEL =====================
        public IActionResult Cancel(int id)
        {
            var appointment = _context.Appointments.FirstOrDefault(a => a.Id == id);

            if (appointment == null)
                return NotFound();

            // ممنوع بعد الدفع أو الإنهاء
            if (appointment.Status == "Paid" || appointment.Status == "Completed")
                return RedirectToAction("Index", "Home");

            appointment.Status = "Cancelled";

            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        // ===================== DETAILS =====================
        public IActionResult Details(int id)
        {
            var appointment = _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefault(a => a.Id == id);

            if (appointment == null)
                return NotFound();

            return View(appointment);
        }

        [Authorize(Roles = "Admin")]
        // ===================== DELETE =====================
        public IActionResult Delete(int id)
        {
            var appointment = _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefault(a => a.Id == id);

            if (appointment == null)
                return NotFound();

            // ممنوع حذف الموعد لو حصل عليه أي إجراء
            if (appointment.Status != "Pending")
                return RedirectToAction(nameof(Index));

            // ممنوع الحذف لو عليه حركة مالية
            bool hasTransaction = _context.Transactions
                .Any(t => t.AppointmentId == id);

            if (hasTransaction)
                return RedirectToAction(nameof(Index));

            return View(appointment);
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var appointment = _context.Appointments
                .FirstOrDefault(a => a.Id == id);

            if (appointment == null)
                return NotFound();

            // ممنوع حذف أي موعد غير Pending
            if (appointment.Status != "Pending")
                return RedirectToAction(nameof(Index));

            // ممنوع الحذف لو عليه حركة مالية
            bool hasTransaction = _context.Transactions
                .Any(t => t.AppointmentId == id);

            if (hasTransaction)
                return RedirectToAction(nameof(Index));

            _context.Appointments.Remove(appointment);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult GetDoctorFees(int id)
        {
            var doctor = _context.Doctors.FirstOrDefault(d => d.Id == id);

            if (doctor == null)
                return NotFound();

            return Json(new
            {
                examinationFee = doctor.ExaminationFee,
                reExaminationFee = doctor.ReExaminationFee
            });
        }
    }
}