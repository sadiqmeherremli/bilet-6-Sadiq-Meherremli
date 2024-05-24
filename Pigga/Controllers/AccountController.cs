using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Pigga.DAl;
using Pigga.Helpers.Account;
using Pigga.Models;
using Pigga.ViewModels;
using System;

namespace Pigga.Controllers
{


    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<User> userManager
            , SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm registerVm)
        {
            if (!ModelState.IsValid) return View();



            User appUser = new User()
            {
                UserName = registerVm.UserName,
                Name = registerVm.Name,
                Surname = registerVm.Surname,
                Email = registerVm.Email,

            };

            var result = await _userManager.CreateAsync(appUser, registerVm.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }


            await _userManager.AddToRoleAsync(appUser, UserRole.Member.ToString());



            return RedirectToAction("Login");
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVm loginVm, string? ReturnUrl = null)
        {
            if (!ModelState.IsValid) return View();


            //User user =await _userManager.FindByEmailAsync(loginVm.UserNameOrEmail);
            //if (user == null)
            //{
            //    user = await _userManager.FindByNameAsync(loginVm.UserNameOrEmail);
            //}

            User user;

            if (loginVm.UserNameOrEmail.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(loginVm.UserNameOrEmail);
            }
            else
            {
                user = await _userManager.FindByNameAsync(loginVm.UserNameOrEmail);
            }
            if (user == null)
            {
                ModelState.AddModelError("", "UsernameOrEmail ve ya Password sefdir");
                return View();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginVm.Password, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Birazdan yeniden cehd edin");
                return View();
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "UsernameOrEmail ve ya Password sefdir");
                return View();
            }

            await _signInManager.SignInAsync(user, loginVm.RememberMe);

            if (ReturnUrl != null)
            {
                return Redirect(ReturnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> CreateRole()
        {

            foreach (var item in Enum.GetValues(typeof(UserRole)))
            {
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = item.ToString()
                });
            }
            return Ok();
        }
    }
}


