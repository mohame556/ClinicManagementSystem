using Clinc.Data;
using Clinc.Models;
using Clinc.Services;
using Clinc.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Clinc.Data;
using Microsoft.EntityFrameworkCore;

namespace Clinc.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class LicenseController : Controller
    {
        private readonly LicenseService _licenseService;
        private readonly AppDbcontext _context;

        public LicenseController(LicenseService licenseService, AppDbcontext context)
        {
            _licenseService = licenseService;
            _context = context;
        }

        // عرض صفحة التفعيل
        public IActionResult Index()
        {
            var vm = new LicenseVM
            {
                MachineId = _licenseService.GetMachineId()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Activate(LicenseVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.MachineId = _licenseService.GetMachineId();
                return View("Index", vm);
            }

            // كود التفعيل الحالي
            if (vm.LicenseKey != "Mohamed_Azzam@01029659972@JWT")
            {
                TempData["Error"] = "كود التفعيل غير صحيح.";

                vm.MachineId = _licenseService.GetMachineId();
                return View("Index", vm);
            }

            var license = _context.Licenses.FirstOrDefault();

            if (license == null)
            {
                license = new License();

                _context.Licenses.Add(license);
            }

            license.MachineId = vm.MachineId;
            license.LicenseKey = vm.LicenseKey;
            license.IsActivated = true;
            license.ActivationDate = DateTime.Now;

            _context.SaveChanges();

            TempData["Success"] = "تم تفعيل البرنامج بنجاح.";

            return RedirectToAction(nameof(Index));
        }
    }
}