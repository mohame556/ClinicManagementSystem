using Clinc.Migrations;
using Clinc.Models;
using Clinc.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Clinc.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<AppUser> userManager,
                                RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // ================= INDEX =================
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();

            var userList = new List<(AppUser User, string Role)>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault() ?? "User";

                userList.Add((user, role));
            }

            ViewBag.UsersWithRoles = userList;

            return View();
        }

        // ================= CREATE GET =================
        public IActionResult Create()
        {
            var vm = new UserVM
            {
                RolesList = _roleManager.Roles
                .Where(r => r.Name == "Admin" || r.Name == "User")
                 .Select(r => r.Name!)
                 .ToList()
            };

            return View(vm);
        }

        // ================= CREATE POST =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.RolesList = _roleManager.Roles
                  .Where(r => r.Name == "Admin" || r.Name == "User")
                  .Select(r => r.Name!)
                  .ToList();
            }

            var user = new AppUser
            {
                FullName = vm.FullName,
                Email = vm.Email,
                UserName = vm.Email,
                Address = vm.Address
            };

            var result = await _userManager.CreateAsync(user, vm.Password);

            if (result.Succeeded)
            {
                var role = string.IsNullOrEmpty(vm.Role) ? "User" : vm.Role;
                await _userManager.AddToRoleAsync(user, role);

                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            vm.RolesList = _roleManager.Roles
               .Where(r => r.Name == "Admin" || r.Name == "User")
               .Select(r => r.Name!)
               .ToList();
            return View(vm);
        }

        // ================= EDIT GET =================
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            var vm = new UserVM
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Address = user.Address,
                Role = roles.FirstOrDefault(),
                RolesList = _roleManager.Roles
                .Where(r => r.Name == "Admin" || r.Name == "User")
                .Select(r => r.Name!)
                .ToList()
             };

            return View(vm);
        }

        // ================= EDIT POST =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserVM vm)
        {
            var user = await _userManager.FindByIdAsync(vm.Id);
            if (user == null) return NotFound();

            user.FullName = vm.FullName;
            user.Email = vm.Email;
            user.UserName = vm.Email;
            user.Address = vm.Address;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                var oldRoles = await _userManager.GetRolesAsync(user);

                if (oldRoles.Any())
                    await _userManager.RemoveFromRolesAsync(user, oldRoles);

                var role = string.IsNullOrEmpty(vm.Role) ? "User" : vm.Role;
                await _userManager.AddToRoleAsync(user, role);

                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            vm.RolesList = _roleManager.Roles
            .Where(r => r.Name == "Admin" || r.Name == "User")
            .Select(r => r.Name!)
            .ToList();

             return View(vm);
        }


        [Authorize(Roles = "SuperAdmin")]

        // ================= DELETE =================
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            await _userManager.DeleteAsync(user);

            return RedirectToAction(nameof(Index));
        }
    }
}