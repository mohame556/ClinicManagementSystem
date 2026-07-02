using Clinc.Data;
using Clinc.Models;
using Clinc.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinc.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class DoctorsController : Controller
    {
        private readonly AppDbcontext _context;

        public DoctorsController(AppDbcontext context)
        {
            _context = context;
        }

        // عرض جميع الدكاترة
        public IActionResult Index()
        {
            var data = _context.Doctors.ToList();
            return View(data);
        }

        // عرض تفاصيل دكتور
        public IActionResult Details(int id)
        {
            var doctor = _context.Doctors.FirstOrDefault(d => d.Id == id);

            if (doctor == null)
                return NotFound();

            return View(doctor);
        }
        // GET
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST
        [HttpPost]
        public IActionResult Create(DoctorVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            Doctors doctor = new Doctors
            {
                Doctor_Name = vm.Doctor_Name,
                Specialty = vm.Specialty,
                Phone = vm.Phone,
                ExaminationFee=vm.ExaminationFee,
                ReExaminationFee=vm.ReExaminationFee
            };

            _context.Doctors.Add(doctor);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // فتح صفحة التعديل
        public IActionResult Edit(int id)
        {
            var doctor = _context.Doctors.FirstOrDefault(d => d.Id == id);

            if (doctor == null)
                return NotFound();

            var vm = new DoctorVM
            {
                Id = doctor.Id,
                Doctor_Name = doctor.Doctor_Name,
                Specialty = doctor.Specialty,
                Phone = doctor.Phone,
                ExaminationFee = doctor.ExaminationFee,
                ReExaminationFee = doctor.ReExaminationFee
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(DoctorVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var doctor = _context.Doctors.FirstOrDefault(d => d.Id == vm.Id);

            if (doctor == null)
                return NotFound();

            doctor.Doctor_Name = vm.Doctor_Name;
            doctor.Specialty = vm.Specialty;
            doctor.Phone = vm.Phone;
            doctor.ExaminationFee = vm.ExaminationFee;
            doctor.ReExaminationFee = vm.ReExaminationFee;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        // فتح صفحة الحذف
        public IActionResult Delete(int id)
        {
            var doctor = _context.Doctors.Find(id);

            if (doctor == null)
                return NotFound();

            return View(doctor);
        }

        // تنفيذ الحذف
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var doctor = _context.Doctors.Find(id);

            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}