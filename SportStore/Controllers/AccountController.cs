using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportStore.Models;

namespace SportStore.Controllers
{
        [Authorize]
        public class AccountController : Controller
        {
            private UserManager<IdentityUser> userManager;
            private SignInManager<IdentityUser> signInManager;
     
            public AccountController(UserManager<IdentityUser> userMgr,
            SignInManager<IdentityUser> signInMgr)
            {
                userManager = userMgr;
                signInManager = signInMgr;
           // IdentitySeedData.EnsurePopulated(userMgr);
        }
            [AllowAnonymous]
            public ViewResult Login(string returnUrl)
            {
                return View(new LoginModel
                {
                    ReturnUrl = returnUrl
                });
            }
            [HttpPost]
            [AllowAnonymous]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Login(LoginModel loginModel)
            {
           // loginModel.Name = "Admin";
              string adminPassword = "Secret123$";

                if (ModelState.IsValid)
                {
                // IdentityUser user = await userManager.FindByNameAsync(loginModel.Name);
                IdentityUser user = new IdentityUser("Admin");
                userManager.CreateAsync(user, adminPassword);

                if (user != null)
                    {
                        await signInManager.SignOutAsync();
                        var password = signInManager.PasswordSignInAsync("Admin", "Secret123$", false, false);
                        if (password != null)
                    {
                        return Redirect("/Admin/Index");
                    }                     
                    }
                }
                ModelState.AddModelError("", "Invalid name or password");
                return View(loginModel);
            }
            public async Task<RedirectResult> Logout(string returnUrl = "/")
            {
                await signInManager.SignOutAsync();
                return Redirect(returnUrl);
            }

        }
  }
