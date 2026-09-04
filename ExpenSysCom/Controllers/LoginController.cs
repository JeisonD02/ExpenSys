using ExpenSysCom.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ExpenSysCom.Models;

namespace ExpenSysCom.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginController(SignInManager<ApplicationUser> signInManager, 
                                UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }
            return View(new LoginViewModel());

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            // 1. Buscar el usuario por Email o UserName
            ApplicationUser user;
            if (viewModel.LoginIdentifier.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(viewModel.LoginIdentifier);
            }
            else
            {
                user = await _userManager.FindByNameAsync(viewModel.LoginIdentifier);
            }

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Credenciales de acceso inválidas.");
                return View(viewModel);
            }

            // 2. Verificar si el usuario está activo
            if (!user.IsActive)
            {
                ModelState.AddModelError(string.Empty, "Tu cuenta está desactivada. Contacta al administrador.");
                return View(viewModel);
            }

            // 3. Intentar iniciar sesión
            var result = await _signInManager.PasswordSignInAsync(
                user.UserName,
                viewModel.Password,
                viewModel.RememberMe,
                lockoutOnFailure: false
            );

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Bienvenido de nuevo.";
                return RedirectToAction("Index", "Dashboard");
            }

            ModelState.AddModelError(string.Empty, "Credenciales de acceso inválidas.");
            return View(viewModel);
        }
    }
}
