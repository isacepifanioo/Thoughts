using Thoughts.Dtos;
using Thoughts.model;
using Thoughts.Model;

namespace Thoughts.services.thought.interactions {
    public interface IInteractionsInterface {
        Task Like(int thoughtId);
        List<LikeModel> GetLikes();
        Task RemoveLikes(int id);
        Task Save(int thoughtId);
        Task RemoveSave(int ThoughtId);
        List<SaveModel> GetSave();
        Task AddComment(int thoughtId, CommentDto commentDto);
        List<CommentModel> GetComments();
        Task DeleteComment(int ThoughtId);
        List<ThoughtsModel> GetSavedThoughtsFromTheUserAuth();
    }
}