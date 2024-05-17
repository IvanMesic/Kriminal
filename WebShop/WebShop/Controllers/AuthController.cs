using AutoMapper;
using DAL.Interfaces;
using DAL.Model.DTO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebShop.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public AuthController(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        [HttpGet]
        public IActionResult HelloWorld()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var model = new
            {
                authenticated = User.Identity.IsAuthenticated,
                userId = userId
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost()]
        public IActionResult Login(LoginDTO login)
        {
            if (!ModelState.IsValid)
                return View(login);

            var user = userRepository.GetUser(login.Username);

            if (user == null || !userRepository.Authenticate(login))
            {
                ModelState.AddModelError("Username", "Invalid username or password");
                return View(login);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var properties = new AuthenticationProperties
            {
                IsPersistent = false
            };

            HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties).Wait();

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
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userExists = userRepository.UserExists(model.Username);
            if (userExists)
            {
                ModelState.AddModelError("", "User already exists");
                return View(model);
            }

            if (!userRepository.CanCreate(model))
            {
                ModelState.AddModelError("", "Username or Email is already in use.");
                return View(model);
            }

            userRepository.Register(model);

            return RedirectToAction("Login");
        }
    }
}
