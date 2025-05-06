using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Thoughts.Dtos;
using Thoughts.services.thought.interactions;
using Thoughts.services.thought.thought;

namespace Thoughts.Controllers {
    public class DeshboardController : Controller {

        private readonly IThoughtsInterface _thoughtsServices;
        private readonly IInteractionsInterface _interactionsService;
        public DeshboardController(IThoughtsInterface thoughtsServices, IInteractionsInterface interactionsService) {
            _thoughtsServices = thoughtsServices;
            _interactionsService = interactionsService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Deshboard(string? search, string? options) {
            var resposta = _thoughtsServices.GetThoughtsAuthToken();

            var thought = resposta.Dados!.AsQueryable();

            if(!string.IsNullOrEmpty(search)) {
                thought = thought.Where(t => t.Thought.Contains(search));
            }
            
            switch(options) {
                case "recentes":
                    thought = thought.OrderByDescending(t => t.Created_at);
                    break;
                case "antigos": 
                    thought = thought.OrderBy(t => t.Created_at);
                    break;
                case "az": 
                    thought = thought.OrderBy(t => t.Thought);
                    break;
                case "za":
                    thought = thought.OrderByDescending(t => t.Thought);
                    break;
            }

            var thoughtResposta = thought.ToList();
            return View(thoughtResposta);
        }

        [HttpGet]
        [Authorize]
        public IActionResult CreateThoughts() {
            return View(new ThoughtsDtos());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateThoughts(ThoughtsDtos thoughtsDtos) {
            if(!ModelState.IsValid) {
                return View(thoughtsDtos);
            }
            var resposta = await _thoughtsServices.CreateThoughts(thoughtsDtos);
            if(!resposta.Status) {
                ModelState.AddModelError(string.Empty, resposta.Message);
                return View(thoughtsDtos);
            }
            return RedirectToAction("Deshboard", "Deshboard");
        }

        [Authorize]
        [HttpGet("DeleteThoughts/{id}")]
        public async Task<IActionResult> DeleteThoughts(int id) {
            var resposta = await _thoughtsServices.DeleteThoughts(id);

            if(!resposta.Status) {
                System.Console.WriteLine(resposta.Message);
            }

            return RedirectToAction("Deshboard", "Deshboard");
        }

    
        [HttpGet("EditThoughts/{id}")]
        [Authorize]
        public IActionResult EditThoughts(int id) {
            var resposta = _thoughtsServices.GetThoughtsById(id);
            var thoughtDtos = new ThoughtsDtos(){
                Thought = resposta.Dados!.Thought
            };
            return View(thoughtDtos);
        }

        [HttpPost("EditThoughts/{id}")]
        [Authorize]
        public async Task<IActionResult> EditThoughts(ThoughtsDtos thoughtsDtos, int id) {
            if(!ModelState.IsValid) {
                return View(thoughtsDtos);
            }

            await _thoughtsServices.EditThoughts(id, thoughtsDtos);

            return RedirectToAction("Deshboard", "Deshboard");
        }

        [Authorize]
        [HttpGet("{id}")] 
        public IActionResult ReadThoughts(int id) {
            var comments = _interactionsService.GetComments();
            ViewBag.Comment = comments;
            var thought = _thoughtsServices.GetThoughtById(id);
            return View(thought);
        }

    }
}