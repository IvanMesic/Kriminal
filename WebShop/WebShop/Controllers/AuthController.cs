using AutoMapper;
using DAL.Model.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

using WebShop.Services.Interfaces;

namespace WebShop.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult HelloWorld()
        {
            string? userId = _authService.GetAuthenticatedUserIdAsync().Result;

            var model = new
            {
                authenticated = _authService.IsAuthenticated(),
                userId = userId
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            if (!ModelState.IsValid)
                return View(login);

            if (!await _authService.LoginAsync(login))
            {
                ModelState.AddModelError("Username", "Invalid username or password");
                return View(login);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogoutConfirmed()
        {
            await _authService.LogoutAsync();
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!await _authService.RegisterAsync(model))
            {
                ModelState.AddModelError("", "User already exists or cannot be created.");
                return View(model);
            }

            return RedirectToAction("Login");
        }
    }
}
