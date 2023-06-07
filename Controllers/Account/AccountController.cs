using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUDCoreApplication.Data;
using Microsoft.AspNetCore.Mvc;
using CRUDCoreApplication.Models;
using CRUDCoreApplication.Models.ViewModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using CRUDCoreApplication.Models.Account;
using System.Text;
using DNTCaptcha.Core;

namespace CRUDCoreApplication.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly ApplicationContext context;
        private readonly IDNTCaptchaValidatorService validatorService;

        public AccountController(ApplicationContext context, IDNTCaptchaValidatorService validatorService)
        {
            this.context = context;
            this.validatorService = validatorService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel LoginModel)
        {
            if (ModelState.IsValid)
            {
                if (!validatorService.HasRequestValidCaptchaEntry(Language.English, DisplayMode.ShowDigits))
                {
                    TempData["CaptchaError"] = "Please enter valid security key";
                    return View(LoginModel);
                }
                var UserData = context.Users.Where(e => e.UserName == LoginModel.UserName).SingleOrDefault();
                if (UserData != null)
                {
                    bool IsValid = (UserData.UserName == LoginModel.UserName && DecryptPassword(UserData.Password) == LoginModel.Password);
                    if (IsValid)
                    {
                        var Identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, LoginModel.UserName) }, CookieAuthenticationDefaults.AuthenticationScheme);
                        var Principal = new ClaimsPrincipal(Identity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, Principal);
                        HttpContext.Session.SetString("UserName", LoginModel.UserName);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["ErrorPassword"] = "Password is Invalid!";
                        return View(LoginModel);
                    }
                }
                else
                {
                    TempData["ErrorUserName"] = "UserName is Invalid!";
                    return View(LoginModel);
                }
            }
            else
            {
                return View(LoginModel);
            }       
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult SignUp(SignupViewModel SignupModel)
        {
            if (ModelState.IsValid)
            {
                var UserData = new UserModel()
                {
                    UserName = SignupModel.UserName,
                    Email = SignupModel.Email,
                    Password = EncryptPassword(SignupModel.Password),
                    ConfirmPassword = EncryptPassword(SignupModel.ConfirmPassword),
                    Mobile = SignupModel.Mobile,
                    IsActive = SignupModel.IsActive,
                    CreatedOn = DateTime.Now,
                    CreatedBy = User.Identity.Name
                };
                context.Users.Add(UserData);
                context.SaveChanges();
                TempData["SignupSuccess"] = "You are eligible to login, Please fill own credential's then login!";
                return RedirectToAction("Login");
            }
            else
            {
                TempData["SignupError"] = "Empty form can't be submitted!";
                return View(SignupModel);
            }     
        }
        public static string EncryptPassword(string Password)
        {
            if (string.IsNullOrEmpty(Password))
            {
                return null;
            }
            else
            {
                byte[] StorePassword = ASCIIEncoding.ASCII.GetBytes(Password);
                string EncryptedPassword = Convert.ToBase64String(StorePassword);
                return EncryptedPassword;
            }
        }

        public static string DecryptPassword(string Password)
        {
            if (string.IsNullOrEmpty(Password))
            {
                return null;
            }
            else
            {
                byte[] EncryptedPassword = Convert.FromBase64String(Password);
                string DecryptedPassword = ASCIIEncoding.ASCII.GetString(EncryptedPassword);
                return DecryptedPassword;
            }
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var StoredCookies = Request.Cookies.Keys;
            foreach (var Cookies in StoredCookies)
            {
                Response.Cookies.Delete(Cookies);
            }
            return RedirectToAction("Login","Account");
        }
    }
}
