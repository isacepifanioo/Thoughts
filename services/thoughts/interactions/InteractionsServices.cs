using Thoughts.Data;
using Thoughts.Dtos;
using Thoughts.model;
using Thoughts.Model;

namespace Thoughts.services.thought.interactions {
    public class InteractionsServices : IInteractionsInterface{
        private readonly AppDbContext _context;
        private readonly IAuthInterface _authService;
        public InteractionsServices(AppDbContext context, IAuthInterface authService) {
            _context = context;
            _authService = authService;
        }
        public async Task Like(int thoughtId) {

            try {
                var usuario = _authService.GetClaimAuthToken();

                var likes = _context.Likes.FirstOrDefault(l => l.UserId == usuario!.Id && l.ThoughtId == thoughtId);

                if(likes != null) {
                    throw new Exception("Você não pode curti duas vezes");
                }

                var like = new LikeModel(){
                    Like = true,
                    ThoughtId = thoughtId,
                    UserId = usuario!.Id
                };

                _context.Likes.Add(like);
                await _context.SaveChangesAsync();
                
            } catch(Exception er) {
                System.Console.WriteLine(er.Message);
            }

        }

        public List<LikeModel> GetLikes() {
            var likes = _context.Likes.ToList();
            return likes;
        }

        public async Task RemoveLikes(int id) {
            try {
                var usuario = _authService.GetClaimAuthToken();
                var likes = _context.Likes.FirstOrDefault(l => l.ThoughtId == id && l.UserId == usuario!.Id);
                
                if(likes == null) {
                    throw new Exception("O deslike nã existe");
                }
                _context.Likes.Remove(likes!);
                await _context.SaveChangesAsync();
            } catch(Exception er) {
                System.Console.WriteLine(er.Message);
            }
        }

        public async Task Save(int thoughtId) {
            try {

                var usuario = _authService.GetClaimAuthToken();
                var save = _context.Save.FirstOrDefault(s => s.UserId == usuario!.Id && s.ThoughtId == thoughtId);

                if(save != null) {
                    throw new Exception("Você não pode salva duas vezes o mesmo item");
                }

                var saveMOdel = new SaveModel() {
                    save = true,
                    ThoughtId = thoughtId,
                    UserId = usuario!.Id
                };

                _context.Save.Add(saveMOdel);
                await _context.SaveChangesAsync();

            }catch(Exception er) {
                System.Console.WriteLine(er.Message);
            }

        }

        public async Task RemoveSave(int ThoughtId) {
            try {

                var usuario = _authService.GetClaimAuthToken();
                var save = _context.Save.FirstOrDefault(s => s.UserId == usuario!.Id && s.ThoughtId == ThoughtId);

                if(save == null) {
                    throw new Exception("Você não pode salva duas vezes o mesmo item");
                }

                _context.Save.Remove(save);
                await _context.SaveChangesAsync();

            } catch(Exception er) {
                System.Console.WriteLine(er.Message);
            }
        }

        public List<SaveModel> GetSave() {
            var save = _context.Save.ToList();
            return save;
        }

        public List<ThoughtsModel> GetSavedThoughtsFromTheUserAuth() {
            var saveAll = GetSave();
            var usuario = _authService.GetClaimAuthToken();

            var Ids = saveAll.Where(s => s.UserId == usuario!.Id).ToList();  

            List<ThoughtsModel> thoughtSaveFromUser = new List<ThoughtsModel>();
            foreach(var id in Ids) {
              var thoughts = _context.Thoughts.Where(t => t.Id == id.ThoughtId).ToList();
              thoughtSaveFromUser.AddRange(thoughts);
            }
            

            return thoughtSaveFromUser;
        }

        public async Task AddComment(int thoughtId, CommentDto commentDto) {
            try {

                var usuario = _authService.GetClaimAuthToken();

                if(usuario == null) {
                    throw new Exception("Você precisa esta autenticado");
                }

                var comment = new CommentModel() {
                    Comment = commentDto.Comment,
                    ThoughtId = thoughtId,
                    Usuario = usuario.Usuario,
                    UserId = usuario.Id
                };

                _context.Comment.Add(comment);
                await _context.SaveChangesAsync();

            } catch (Exception er) {
                System.Console.WriteLine(er.Message);
            }
        }

        public List<CommentModel> GetComments() {
            var comments = _context.Comment.ToList() ?? [];
            return comments;
        }

        public async Task DeleteComment(int CommentId) {
            try {

                var usuario = _authService.GetClaimAuthToken();

                if(usuario == null) {
                    throw new Exception("Usuario não encontrado");
                }

                var comment = _context.Comment.FirstOrDefault(c => CommentId == c.Id && usuario.Id == c.UserId);
 
                if(comment == null) {
                    throw new Exception("Esse item não pode ser removido");
                }

                _context.Comment.Remove(comment);
                await _context.SaveChangesAsync();
            }catch(Exception er) {
                System.Console.WriteLine(er.Message);
            }
        }
        
    }
}