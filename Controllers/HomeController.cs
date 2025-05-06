using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thoughts.Data;
using Thoughts.services;
using Thoughts.services.thought.interactions;
using Thoughts.services.thought.thought;

namespace Thoughts.Controllers {
    public class HomeController : Controller {

        private readonly IThoughtsInterface _thoughtsServices;
        private readonly IInteractionsInterface _interactionsService;
        private readonly IAuthInterface _authServices;
        private readonly AppDbContext _context;

        public HomeController(IThoughtsInterface thoughtsServices, IInteractionsInterface interactionsService, IAuthInterface authServices, AppDbContext context) {
            _thoughtsServices = thoughtsServices;
            _interactionsService = interactionsService;
            _authServices = authServices;
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index(string? search){
            var resposta = _thoughtsServices.GetThoughts();
            var likes = _interactionsService.GetLikes();
            var userAuth = _authServices.GetClaimAuthToken();
            var saves = _interactionsService.GetSave();
            ViewBag.Saves = saves;
            ViewBag.UserAuth = userAuth;
            ViewBag.Likes = likes;

            System.Console.WriteLine(search);

            var thought = resposta.Dados!.AsQueryable();

            if(!string.IsNullOrEmpty(search)) {
                thought = thought.Where(e => e.Thought.Contains(search));
            }

            var listThoughts = thought.ToList();

            return View(listThoughts);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Favorites() {
            var itensSaved = _interactionsService.GetSavedThoughtsFromTheUserAuth(); 
            var likes = _interactionsService.GetLikes();
            var userAuth = _authServices.GetClaimAuthToken();
            var saves = _interactionsService.GetSave();
            ViewBag.Saves = saves;
            ViewBag.UserAuth = userAuth;
            ViewBag.Likes = likes;
            return View(itensSaved);
        }

    }
}