using Microsoft.EntityFrameworkCore;
using Thoughts.Data;
using Thoughts.Dtos;
using Thoughts.model;
using Thoughts.Model;

namespace Thoughts.services.thought.thought {
    public class ThoughtsServices : IThoughtsInterface {

        private readonly AppDbContext _context;
        private readonly IAuthInterface _AuthServices;
        public ThoughtsServices(AppDbContext context, IAuthInterface authServices) {
            _context = context;
            _AuthServices = authServices;
        }
        
        public Resposta<List<ThoughtsModel>> GetThoughts() {
            var resposta = new Resposta<List<ThoughtsModel>>();
            resposta.Status = false;

            try {
                var thought = _context.Thoughts.ToList();

                resposta.Dados = thought;
                resposta.Status = true;

            } catch(Exception er) {
                resposta.Dados = null;
                resposta.Message = er.Message;
            }
            return resposta;
        }

        public async Task<Resposta<string>> CreateThoughts(ThoughtsDtos thoughts) {

            Resposta<string> resposta = new Resposta<string>();
            resposta.Status = false;

            try {
                var usuario = _AuthServices.GetClaimAuthToken();
                
                if(usuario == null) {
                    resposta.Message = "Não foi possivel Encontra-lo, tente novamente mais tarde";
                    return resposta;
                } 

                var createThoughts = new ThoughtsModel(){
                    Thought = thoughts.Thought,
                    Usuario = usuario.Usuario,
                    UserId = usuario.Id
                };

                _context.Thoughts.Add(createThoughts);
                await _context.SaveChangesAsync();

                resposta.Status = true;
                resposta.Message = "Thoughts Criado Com Sucesso";
            } catch (Exception er) {
                resposta.Dados = null;
                resposta.Message = er.Message;
            }

            return resposta;

        }


        public Resposta<List<ThoughtsModel>> GetThoughtsAuthToken() {
            Resposta<List<ThoughtsModel>> resposta = new Resposta<List<ThoughtsModel>>();
            resposta.Status = false;
            try {

                var usuario = _AuthServices.GetClaimAuthToken();

                if(usuario == null) {
                    resposta.Message = "Não foi possivel Encontra-lo, tente novamente mais tarde";
                }

                var myThoughts = _context.Thoughts.Where(thought => thought.UserId == usuario!.Id).ToList() ?? [];
                
                resposta.Dados = myThoughts;

            } catch(Exception er) {
                resposta.Dados = null;
                resposta.Message = er.Message;
            }

            return resposta;
        }

        public async Task<Resposta<string>> DeleteThoughts(int id) {
            Resposta<string> resposta = new Resposta<string>();
            resposta.Status = false;
            try {

                var thought = await _context.Thoughts.FirstOrDefaultAsync(t => t.Id == id);
                
                if(thought == null) {
                    resposta.Message = "Não foi possivel deleta seu Thoughts, Tente Novamente Mais Tarde";
                    return resposta;
                }
                

                _context.Thoughts.Remove(thought!);
                await _context.SaveChangesAsync();

                resposta.Message = "Thoughts foi removido com sucesso";
                resposta.Status = true;

            } catch(Exception er) {
                resposta.Dados = null;
                resposta.Message = er.Message;
            }

            return resposta;
        }

        public Resposta<ThoughtsModel> GetThoughtsById(int id) {
            var resposta = new Resposta<ThoughtsModel>();
            resposta.Status = false;
            try {

                var thought = _context.Thoughts.Find(id);

                if(thought == null) {
                    resposta.Message = "Não foi possivel deleta seu Thoughts, Tente Novamente Mais Tarde";
                    return resposta;
                }

                resposta.Dados = thought;
                resposta.Status = true;

            } catch(Exception er) {
                resposta.Dados = null;
                resposta.Message = er.Message;
            }

            return resposta;
        }

        public async Task<Resposta<string>> EditThoughts(int id, ThoughtsDtos thoughtsDtos) {

            Resposta<string> resposta = new Resposta<string>();
            resposta.Status = false;

            try {

                var thought = _context.Thoughts.FirstOrDefault(t => t.Id == id);

                if(thought == null) {
                    resposta.Message = "Não foi possivel deleta seu Thoughts, Tente Novamente Mais Tarde";
                    return resposta;
                }

                thought.Thought = thoughtsDtos.Thought;
                _context.Thoughts.Update(thought);
                await _context.SaveChangesAsync();

                resposta.Status = true;
                resposta.Message = "Thoughts foi alterado com sucesso";

            } catch(Exception er) {
                resposta.Dados = null;
                resposta.Message = er.Message;
            }

            return resposta;
        }

        public ThoughtsModel GetThoughtById(int id) {
            ThoughtsModel? thought = _context.Thoughts.FirstOrDefault(t => t.Id == id);
            if(thought == null) {
                throw new Exception("Não foi encontrado thought");
            }
            return thought;
        }

    }
}