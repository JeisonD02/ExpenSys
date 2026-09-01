using ExpenSysCom.Modelos;
using ExpenSysCom.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenSysCom.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsuariosController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Usuarios
        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var usuarios = await _userManager.Users.ToListAsync(); // ahora es async

            var model = new List<UsuarioViewModel>();

            foreach (var u in usuarios)
            {
                var roles = await _userManager.GetRolesAsync(u);

                model.Add(new UsuarioViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    NoDocumento = u.NoDocumento,
                    Role = roles.FirstOrDefault() ?? "Sin rol",
                    IsActive = u.IsActive
                });
            }

            return View(model);
        }
        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            var model = new UsuarioViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                NoDocumento = user.NoDocumento,
                Role = roles.FirstOrDefault() ?? "Sin rol",
                IsActive = user.IsActive
            };

            return View(model);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                NoDocumento = model.NoDocumento,
                IsActive = true // Siempre lo dejamos activo al crear
            };

            // Crear usuario con su contraseña (se hashea automáticamente)
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // 👇 Si no viene rol, por defecto asignamos "Usuario"
                var role = string.IsNullOrEmpty(model.Role) ? "Usuario" : model.Role;

                // 👇 Verificar que el rol exista; si no, lo creamos
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }

                // 👇 Asignar rol al usuario
                await _userManager.AddToRoleAsync(user, role);

                TempData["Success"] = $"El usuario {user.UserName} fue creado correctamente con el rol {role}.";

                return RedirectToAction(nameof(Index));
            }

            // Manejo de errores de Identity (ejemplo: contraseña débil, email duplicado, etc.)
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            var model = new UsuarioViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                NoDocumento = user.NoDocumento,
                Role = roles.FirstOrDefault(),
                IsActive = user.IsActive
            };

            return View(model);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UsuarioViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return NotFound();

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.NoDocumento = model.NoDocumento;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                // Actualizar Rol
                var currentRoles = await _userManager.GetRolesAsync(user);
                if (currentRoles.Any())
                {
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                }

                if (!string.IsNullOrEmpty(model.Role))
                {
                    await _userManager.AddToRoleAsync(user, model.Role);
                }

                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        // POST: Usuarios/CambiarEstado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(string id, bool activar)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            user.IsActive = activar;
            await _userManager.UpdateAsync(user);

            TempData["Success"] = activar
                ? $"El usuario {user.UserName} fue reactivado correctamente."
                : $"El usuario {user.UserName} fue desactivado correctamente.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Usuarios/ResetPassword/5
        public async Task<IActionResult> ResetPassword(string id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var model = new UsuarioViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };

            return View(model);
        }

        // POST: Usuarios/ResetPassword/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string id, string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword))
            {
                ModelState.AddModelError("", "Debe ingresar una nueva contraseña.");
                return View(new UsuarioViewModel { Id = id });
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(new UsuarioViewModel { Id = id });
        }
    }
}
