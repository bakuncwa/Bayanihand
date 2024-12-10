using Bayanihand.App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bayanihand.App.Controllers
{
    public class AccountController: Controller
    {
        private readonly UserManager<IdentityUser> userMgr;
        private readonly RoleManager<IdentityRole> roleMgr;
        private readonly SignInManager<IdentityUser> signInMgr;
        public AccountController(UserManager<IdentityUser> userMgr, RoleManager<IdentityRole> roleMgr, SignInManager<IdentityUser> signInMgr) 
        {
            this.userMgr = userMgr;
            this.roleMgr = roleMgr;
            this.signInMgr = signInMgr;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (string.IsNullOrEmpty(model.PhoneNo))
            {
                ModelState.AddModelError(nameof(model.PhoneNo), "Phone number is required.");
                return View(model);
            }

            var existingUser = await userMgr.FindByNameAsync(model.PhoneNo);
            if (existingUser != null)
            {
                ModelState.AddModelError(nameof(model.PhoneNo), "Phone number already in use.");
                return View(model);
            }

            IdentityUser user = new()
            {
                UserName = model.PhoneNo,
                PhoneNumber = model.PhoneNo
            };

            var result = await userMgr.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (!await roleMgr.RoleExistsAsync(model.Role))
                {
                    await roleMgr.CreateAsync(new IdentityRole(model.Role));
                }
                await userMgr.AddToRoleAsync(user, model.Role);

                return RedirectToAction("SignIn", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult SignIn(string? returnUrl)
        {
            SignInVM loginVM = new();
            if (!string.IsNullOrEmpty(returnUrl))
                loginVM.ReturnUrl = returnUrl;
            return View(loginVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInVM model, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.UserName))
                {
                    ModelState.AddModelError("Username", "Username is required.");
                    return View(model);
                }

                IdentityUser? user = await userMgr.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    var result = await signInMgr.PasswordSignInAsync(model.UserName, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        var roles = await userMgr.GetRolesAsync(user);
                        if (roles.Contains("Handyman"))
                        {
                            return RedirectToAction("Profile", "Handyman");
                        }
                        else if (roles.Contains("Customer"))
                        {
                            return RedirectToAction("Index", "Service");
                        }

                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return LocalRedirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "Invalid credentials.");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("Error", "Invalid credentials.");
                    return View(model);
                }
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public new async Task<IActionResult> LogOut()
        {
            await signInMgr.SignOutAsync();
            return RedirectToAction("SignIn", "Account");
        }

    }
}
