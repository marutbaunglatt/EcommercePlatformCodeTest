using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EcommercePlatformCodeTest.Models;
using EcommercePlatformCodeTest.Interfaces;

namespace EcommercePlatformCodeTest.Controllers
{
    public class UserController : Controller
    {
        
        private readonly IUser _userRepo;
        public IRequestCookieCollection Cookies { get; }
        public UserController(IUser userRepo)
        {
            _userRepo = userRepo;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> UserProfile()
        {
            var currentUser = HttpContext.User;
            if (currentUser != null && User.Identity.IsAuthenticated && int.TryParse(currentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                var user = await _userRepo.GetUserById(userId);
                return View("UserProfile", user);
            }
            return View("login");
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null || id == 0)
            {
                ViewBag.btnName = "Add New User";
                return View(new User());
            }

            var user = await _userRepo.GetUserById(id);
            if (user == null)
                return NotFound();
            ViewBag.btnName = "Edit User";
            return View(user);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            try
            {
                bool alreadyExists = await _userRepo.GetUserByEmail(user.Email);

                if (alreadyExists)
                {
                    TempData["Error"] = user.Username + " is already exists.";
                    return View(user);
                }

                await _userRepo.SaveUser(user);
                TempData["Success"] = "Account was created successfully.";
                return RedirectToAction("login");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred during the process." + ex.Message;
                return RedirectToAction("Error");
            }
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login(string returnUrl)
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var getUser = await _userRepo.AuthenticateLogin(email, password);
            if (getUser != null)
            {
                var claims = new List<Claim>();

                claims.Add(new Claim("userEmail", getUser.Email));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, getUser.Id+""));
                claims.Add(new Claim(ClaimTypes.Name, getUser.Username));
                var ClaimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var ClaimsPriciple = new ClaimsPrincipal(ClaimsIdentity);

                await HttpContext.SignInAsync(ClaimsPriciple);

                //TempData["Success"] = "Login successful. Welcome, " + getUser.Username + "!";
                return RedirectToAction("Index", "Home");
            }

            TempData["Error"] = "Error. Username or password is invalid";
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        public IActionResult Success()
        {
            return View();
        }

        [Authorize]
        [HttpGet("denied")]
        public IActionResult Denied()
        {
            return View();
        }
    }
}
