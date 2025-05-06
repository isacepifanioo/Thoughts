using Thoughts.Dtos;
using Thoughts.model;

namespace Thoughts.services {
    public interface IAuthInterface {
        Task<Resposta<string>> Register(UsuarioDtos User);
        public bool CheckEmailExist(string email);
        string CreateToken(UsuarioModel Usuario);
        Resposta<string> Login(LoginDtos User);
        UsuarioModel? GetClaimAuthToken();
    }
}