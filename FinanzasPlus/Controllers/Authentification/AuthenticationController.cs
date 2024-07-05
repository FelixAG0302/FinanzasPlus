using FinanzasPlus.Models;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanzasPlus.Controllers.Authentification
{
    public class AuthenticationController(FinancesContext context) : Controller
    {
        private const string DefaultRole = "USER";

        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(User modelUsuario)
        {
            var usuario = new User
            {
                Name = modelUsuario.Name,
                Email = modelUsuario.Email,
                Password = modelUsuario.Password,
                PersonType = modelUsuario.PersonType
            };

            var rol = await context.Roles
                .Where(rol => DefaultRole.Equals(rol.Name))
                .FirstOrDefaultAsync() ?? new Role
                {
                    Name = "USER",
                    Description = "Default user permissions."
                };

            await context.UserRoles.AddAsync(
                new UserRole
                {
                    User = usuario,
                    Role = rol
                }
            );

            await context.Users.AddAsync(usuario);
            await context.SaveChangesAsync();

            return RedirectToAction("Login", "Authentication");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User modelUsuario)
        {
            var usuario = await context.Users
                .Where(u => u.Name == modelUsuario.Name && u.Password == modelUsuario.Password)
                .FirstOrDefaultAsync();
            if (usuario == null)
            {
                ViewData["Mensaje"] = "¡Las credenciales son incorrectas!";
                return View(modelUsuario);
            }

            Response.Cookies.Append(
                "Authorization",
                "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(usuario.Name + ":" + usuario.Password)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(1),
                    HttpOnly = true
                }
            );

            return RedirectToAction("Index", "Home");
        }
    }
}
