using Clinc.Models;
using Clinc.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    public AccountController(SignInManager<AppUser> signInManager,
                             UserManager<AppUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
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
            var roles = await _userManager.GetRolesAsync(user);


            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "Invalid login");
        return View(vm);
    }

    // ================= LOGOUT =================
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
}