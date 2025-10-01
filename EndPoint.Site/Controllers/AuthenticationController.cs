using Azure.Core;
using EndPoint.Site.Models.ViewModels.AuthenticationViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.Commands.RegisterUser;
using Store.Application.Services.Users.Commands.UserLogin;
using Store.Common.Dto;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace EndPoint.Site.Controllers
{
    public class AuthenticationController(IRegisterUserService registerUserService, IUserLoginService userLoginService) : Controller
    {
        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(RequestRegisterUserDto request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Json(new ResultDto
                {
                    IsSuccess = false,
                    Message = string.Join(" | ", errors)
                });
            }

            if (User.Identity!.IsAuthenticated)
            {
                return Json(new ResultDto
                {
                    IsSuccess = false,
                    Message = "شما وارد حساب کاربری خود شده‌اید و نمی‌توانید دوباره ثبت‌نام کنید"
                });
            }

            var signupResult = registerUserService.Execute(new RequestRegisterUserDto
            {
                Email = request.Email,
                FullName = request.FullName,
                Password = request.Password,
                RePassword = request.RePassword,
                roles = new List<RolesInRegisterUserDto>
        {
            new RolesInRegisterUserDto { Id = 3 }
        }
            });

            if (signupResult.IsSuccess)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, signupResult.Data.UserId.ToString()),
            new Claim(ClaimTypes.Email, request.Email!),
            new Claim(ClaimTypes.Name, request.FullName!),
            new Claim(ClaimTypes.Role, "Customer"),
        };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var properties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                HttpContext.SignInAsync(principal, properties);
            }

            return Json(signupResult);
        }


        public IActionResult Signin(string ReturnUrl = "/")
        {
            ViewBag.url = ReturnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult Signin(string Email, string Password, string url = "/")
        {
            var signupResult = userLoginService.Execute(Email, Password);
            if (signupResult.IsSuccess == true)
            {
                var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,signupResult.Data.UserId.ToString()),
                new Claim(ClaimTypes.Email, Email),
                new Claim(ClaimTypes.Name, signupResult.Data.Name),
                new Claim(ClaimTypes.Role, signupResult.Data.Roles ),
            };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var properties = new AuthenticationProperties()
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.Now.AddDays(5),
                };
                HttpContext.SignInAsync(principal, properties);

            }
            return Json(signupResult);
        }


        public new IActionResult SignOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}
