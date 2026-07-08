using Clinc.Data;
using Clinc.Models;
using Clinc.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
public class AccountController : Controller
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly AppDbcontext _context;

    public AccountController(
     SignInManager<AppUser> signInManager,
     UserManager<AppUser> userManager,
     AppDbcontext context)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _context = context;
    }

    // ================= LOGIN =================
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var result = await _signInManager.PasswordSignInAsync(
            vm.Email,
            vm.Password,
            vm.RememberMe,
            false);

        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(vm.Email);

            if (user == null)
            {
                await _signInManager.SignOutAsync();
                return View(vm);
            }

            var roles = await _userManager.GetRolesAsync(user);

            var license = _context.Licenses.FirstOrDefault();

            bool activated = license != null && license.IsActivated;

            if (!activated)
            {
                if (roles.Contains("SuperAdmin"))
                {
                    return RedirectToAction("Index", "License");
                }

                await _signInManager.SignOutAsync();

                ModelState.AddModelError("", "البرنامج غير مفعل، برجاء التواصل مع مسؤول النظام.");

                return View(vm);
            }

            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "البريد الإلكتروني أو كلمة المرور غير صحيحة");

        return View(vm);
    }

    // ================= LOGOUT =================
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
}