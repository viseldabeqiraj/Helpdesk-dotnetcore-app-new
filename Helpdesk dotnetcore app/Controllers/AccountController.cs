using Helpdesk.Core.Interfaces;
using Helpdesk.Core.ViewModels.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Controllers
{
    public class AccountController : Controller
    {
        private IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(AccountViewModel.Login login)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.GetUserAsync(new Core.Dtos.User.LoginDto
                {
                    Password = GetMD5(login.Password),
                    Username = login.Username
                });

                if (user != null)
                {
                    //Identity id = new Identity(user.Id, user.Username, string.Join(",", user.Roles));

                    //DateTime expire = DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes);

                    //FormsAuthenticationTicket ticket = new FormsAuthenticationTicket
                    //    (user.Id, user.Username, DateTime.Now, expire, false, id.GetUserData());

                    //string hashTicket = FormsAuthentication.Encrypt(ticket);
                    //HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashTicket);
                    //HttpContext.Response.Cookies.Add(cookie);

                    ////add session
                    //Session["FullName"] = user.FirstName + " " + user.Surname;
                    //Session["Username"] = user.Username;
                    //Session["ID"] = user.Id;
                    //return RedirectToAction("Index", "Dashboard");

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim("FullName", user.FirstName + " " + user.Surname),
                        new Claim("Username", user.Username),
                        new Claim("ID", user.Id.ToString()),
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    HttpContext.Session.SetString("FullName", user.FirstName + " " + user.Surname);
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetInt32("ID", user.Id);

                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    ViewBag.error = "Përdorues ose fjalëkalim i gabuar!";
                }
            }
            return View();
        }

        //[Authorize(Roles ="Pergjegjes")]
        public ActionResult Register()
        {
            return View();
        }

        //POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(AccountViewModel.Register register)
        {
            if (ModelState.IsValid)
            {
                var exists = await _userRepository.UserExistsForRegistrationAsync(register.Username);
                if (exists)
                {
                    ViewBag.error = "Ky përdorues ekziston! Ju lutem vendosni një përdorues tjetër!";
                    return View();
                }
                else
                {
                    var success = await _userRepository.AddUserAsync(new Core.Dtos.User.RegisterDto
                    {
                        Username = register.Username,
                        Password = GetMD5(register.Password)
                    });

                    if (success)
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        ViewBag.error = "Ka ndodhur një gabim!";
                        return View();
                    }
                }

            }
            return View();
        }

        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> LogOutAsync()
        {
            //if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            //{
            //    var c = new HttpCookie(FormsAuthentication.FormsCookieName)
            //    {
            //        Expires = DateTime.Now.AddDays(-1)
            //    };
            //    Response.Cookies.Add(c);
            //}
            //Session.Clear();
            //FormsAuthentication.SignOut();

            // Clear the existing external cookie
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Account");
        }

        private static string GetMD5(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
    }
}