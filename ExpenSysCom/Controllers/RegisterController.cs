using ExpenSysCom.Modelos; 
using ExpenSysCom.ViewModels; 
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExpenSysCom.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public RegisterController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newUser = new ApplicationUser
            {
                UserName = model.Nombre,
                Email = model.Email,
                NoDocumento = model.NoDocumento,
                PhoneNumber = model.Telefono
            };

            //Crea el usuario
            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (result.Succeeded)
            {
                // Asigna el rol por defecto (Usuario)
                await _userManager.AddToRoleAsync(newUser, "Usuario");

                // Loguea al usuario
                await _signInManager.SignInAsync(newUser, isPersistent: false);
                TempData["SuccessMessage"] = "Usuario registrado correctamente.";
                return RedirectToAction("Index", "Login");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
        }
    }
}
