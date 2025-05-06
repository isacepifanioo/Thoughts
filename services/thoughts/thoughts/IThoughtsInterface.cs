using Thoughts.Dtos;
using Thoughts.model;
using Thoughts.Model;

namespace Thoughts.services.thought.thought {
    public interface IThoughtsInterface {
        Resposta<List<ThoughtsModel>> GetThoughts();
        Task<Resposta<string>> CreateThoughts(ThoughtsDtos thoughts);
        Resposta<List<ThoughtsModel>> GetThoughtsAuthToken();
        Task<Resposta<string>> DeleteThoughts(int id);
        Resposta<ThoughtsModel> GetThoughtsById(int id);
        Task<Resposta<string>> EditThoughts(int id, ThoughtsDtos thoughtsDtos);
        ThoughtsModel GetThoughtById(int id);
    }
}