using Clinc.Data;
using Clinc.Models;
using Clinc.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace Clinc.Controllers
{
    public class PatientsController : Controller
    {
        private readonly AppDbcontext _context;

        public PatientsController(AppDbcontext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            var data = _context.Patients.ToList();
            return View(data);
        }
        public IActionResult Details(int id)
        {
            var Patient = _context.Patients.FirstOrDefault(d => d.Id == id);

            if (Patient == null)
                return NotFound();

            return View(Patient);
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PatientsVM VM)
        {
            if (!ModelState.IsValid)
            {
                return View(VM);
            }
            Patients patient = new Patients
            {
                Patient_Name=VM.Patient_Name,
                Phone=VM.Phone, 
                Address=VM.Address,
                Age=VM.Age
            };

            _context.Patients.Add(patient);
            _context.SaveChanges();

            return RedirectToAction("Create" , "Appointment");
        }

        // فتح صفحة التعديل
        public IActionResult Edit(int id)
        {
            var patient = _context.Patients.Find(id);

            if (patient == null)
                return NotFound();
            var vm = new PatientsVM
            { 
                Id= patient.Id,
                Patient_Name=patient.Patient_Name,
                Address = patient.Address,
                Age = patient.Age,
                Phone=patient.Phone
            };

            return View(vm);
        }

        // حفظ التعديل
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PatientsVM vm)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                return View(vm);
            }

            var patient = _context.Patients.Find(vm.Id);

            if (patient == null)
            {
                return NotFound();
            }

            patient.Patient_Name = vm.Patient_Name;
            patient.Address = vm.Address;
            patient.Age = vm.Age;
            patient.Phone = vm.Phone;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        // فتح صفحة الحذف
        public IActionResult Delete(int id)
        {
            var Patient = _context.Patients.Find(id);

            if (Patient == null)
                return NotFound();

            return View(Patient);
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        // تنفيذ الحذف
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var Patient = _context.Patients.Find(id);

            if (Patient != null)
            {
                _context.Patients.Remove(Patient);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
