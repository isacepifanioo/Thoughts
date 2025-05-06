using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thoughts.Dtos;
using Thoughts.services;
using Thoughts.services.thought.interactions;
using Thoughts.services.thought.thought;

namespace Thoughts.Controllers {
    public class InteractionsController : Controller {
        private readonly IInteractionsInterface _interactionsService;
        private readonly IThoughtsInterface _thoughtsServices;
        private readonly IAuthInterface _authServices;
        public InteractionsController(IInteractionsInterface interactionsService, IThoughtsInterface thoughtsServices, IAuthInterface authServices) {
            _interactionsService = interactionsService;
            _thoughtsServices = thoughtsServices;
            _authServices = authServices;
        }
        [Authorize]
        [HttpGet("Interactions/Like/{id}")]
        public async Task<IActionResult> Like(int id) {
            await _interactionsService.Like(id);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet("Interactions/RemoveLike/{id}")]
        public async Task<IActionResult> RemoveLike(int id) {
            await _interactionsService.RemoveLikes(id);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet("Interactions/Save/{id}")]
        public async Task<IActionResult> Save(int id) {
            await _interactionsService.Save(id);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet("Interactions/RemoveSave/{id}")]
        public async Task<IActionResult> RemoveSave(int id) {
            await _interactionsService.RemoveSave(id);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet("Interactions/Comment/{id}")]
        public IActionResult Comment(int id) {
            var thought = _thoughtsServices.GetThoughtById(id);
            var userAuth = _authServices.GetClaimAuthToken();
            var likes = _interactionsService.GetLikes();
            var saves = _interactionsService.GetSave();
            var comments = _interactionsService.GetComments();
            ViewBag.Comment = comments;
            ViewBag.Saves = saves;
            ViewBag.UserAuth = userAuth;
            ViewBag.Likes = likes;
            ViewBag.Thought = thought;
            return View(new CommentDto());
        }
        [Authorize]
        [HttpPost("Interactions/Comment/{id}")]
        public async Task<IActionResult> Comment(int id, CommentDto commentDto) {
            var thought = _thoughtsServices.GetThoughtById(id);
            var userAuth = _authServices.GetClaimAuthToken();
            var comments = _interactionsService.GetComments();
            ViewBag.Comment = comments;
            ViewBag.UserAuth = userAuth;
            ViewBag.Thought = thought;
            if(!ModelState.IsValid) {
                return View(commentDto);
            } 
            await _interactionsService.AddComment(id, commentDto);
            return RedirectToAction("Comment", "Interactions");
        }

        [Authorize]
        [HttpGet("Interactions/DeletaComment/{id}")]
        public async Task<IActionResult> DeletaComment(int id) {
            await _interactionsService.DeleteComment(id);
            return RedirectToAction("Comment", "Interactions");
        }
    }
}