using AllUp.Interfaces;
using AllUp.Models;
using AllUp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;



namespace AllUp.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IEmailService _emailService;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _emailService = emailService;
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Register(RegisterVM register)
    {
        if (!ModelState.IsValid) return View(register);
        AppUser user = new() { Fullname = register.Fullname, Email = register.Email, UserName = register.Username };
        IdentityResult result = await _userManager.CreateAsync(user, register.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(register);
        }
        await _userManager.AddToRoleAsync(user, "member");

        string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        string link = Url.Action(nameof(VerifyEmail), "Account", new { email = user.Email, token = token }, Request.Scheme, Request.Host.ToString());

        string body = "";
        using (StreamReader stream = new StreamReader("wwwroot/templates/verifyEmail.html"))
        {
            body = stream.ReadToEnd();
        };
        body = body.Replace("{{link}}", link);
        body = body.Replace("{{username}}", user.UserName);

        _emailService.SendEmail(user.Email, "Verify Email", body);

        return RedirectToAction(nameof(Login));
    }

    public async Task<IActionResult> VerifyEmail(string token, string email)
    {
        AppUser? appUser = await _userManager.FindByEmailAsync(email);
        if (appUser == null) return NotFound();
        await _userManager.ConfirmEmailAsync(appUser, token);
        return RedirectToAction(nameof(Login));
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Login(LoginVM loginVM, string? returnUrl)
    {
        if (!ModelState.IsValid) return View(loginVM);
        AppUser? user = await _userManager.FindByEmailAsync(loginVM.UsernameOrEmail);
        if (user == null)
        {
            user = await _userManager.FindByNameAsync(loginVM.UsernameOrEmail);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found");
                return View(loginVM);
            }
        }
        bool doesPasswordMatch = await _userManager.CheckPasswordAsync(user, loginVM.Password);
        if (!doesPasswordMatch)
        {
            ModelState.AddModelError("", "Username or password is wrong.");
            return View(loginVM);
        }
        if (user.IsBlocked)
        {
            ModelState.AddModelError("", "User is blocked.");
            return View(loginVM);
        }
        var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, true);
        if (result.IsLockedOut)
        {
            ModelState.AddModelError("", "User is locked out. Try again lates.");
            return View(loginVM);
        }
        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Verify email.");
            return View(loginVM);
        }
        if (returnUrl is null)
            return RedirectToAction("Index", "Home");
        return Redirect(returnUrl);
    }


    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost, AutoValidateAntiforgeryToken]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        AppUser? appUser = await _userManager.FindByEmailAsync(email);
        if (appUser == null)
        {
            ModelState.AddModelError("Error1", "User not found");
            return View();
        };

        string token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
        string link = Url.Action(nameof(ResetPassword), "Account", new { email = appUser.Email, token = token }, Request.Scheme, Request.Host.ToString());

        string body = "";
        using (StreamReader stream = new StreamReader("wwwroot/templates/forgotPassword.html"))
        {
            body = stream.ReadToEnd();
        };
        body = body.Replace("{{link}}", link);
        body = body.Replace("{{username}}", appUser.UserName);

        _emailService.SendEmail(appUser.Email, "Reset Password", body);

        return View();
    }

    public async Task<IActionResult> ResetPassword(string email, string token)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return NotFound();
        bool result = await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token);
        if (!result) return BadRequest();
        return View();
    }

    [HttpPost, AutoValidateAntiforgeryToken]
    public async Task<IActionResult> ResetPassword(string email, string token, ResetPasswordVM resetPasswordVM)
    {
        AppUser? appUser = await _userManager.FindByEmailAsync(email);
        if (!ModelState.IsValid) return View();
        if (appUser == null)
        {
            ModelState.AddModelError("Error1", "User not found");
            return View(resetPasswordVM);
        };

        var result = await _userManager.ResetPasswordAsync(appUser, token, resetPasswordVM.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(resetPasswordVM);
        }
        await _userManager.UpdateSecurityStampAsync(appUser);

        return RedirectToAction("login");
    }
}
