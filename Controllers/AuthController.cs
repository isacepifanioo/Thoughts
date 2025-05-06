using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Thoughts.Dtos;
using Thoughts.services;

namespace Thoughts.Controllers {
    public class AuthController : Controller{

        private readonly IAuthInterface _authServices;
        public AuthController(IAuthInterface authServices) {
             _authServices = authServices;
        }

        [HttpGet]
        public IActionResult Register() {
            return View(new UsuarioDtos());
        }

        [HttpPost]
        public async Task<IActionResult> Register(UsuarioDtos User) {
            if(!ModelState.IsValid) {
                return View(User);
            }
            var resposta = await _authServices.Register(User);
            if(!resposta.Status) {
                ModelState.AddModelError(string.Empty, resposta.Message);
                return View(User);
            }
            return RedirectToAction("Login", "Auth");
        }
        
        [HttpGet]
        public IActionResult Login() {
            return View(new LoginDtos());
        }

        [HttpPost]
        public IActionResult Login(LoginDtos User) {
            if(!ModelState.IsValid) {
                return View(User);
            }
            var resposta = _authServices.Login(User);
            if(!resposta.Status) {
                ModelState.AddModelError(string.Empty, resposta.Message);
                return View(User);
            }

            Response.Cookies.Append("AuthToken", resposta.Dados!, new CookieOptions{
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddDays(1)
            });
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout() {
           Response.Cookies.Delete("AuthToken");
           return RedirectToAction("Login", "Auth");
        }

    }
}