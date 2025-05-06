using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Thoughts.Data;
using Thoughts.Dtos;
using Thoughts.model;
using Microsoft.AspNetCore.Authorization;

namespace Thoughts.services {
    public class AuthServices : IAuthInterface{
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;
        private readonly ISenhaInterface _passwordServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthServices(AppDbContext context, ISenhaInterface passwordServices, IConfiguration config, IHttpContextAccessor httpContextAccessor) {
            _context = context;
            _passwordServices = passwordServices;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public async Task<Resposta<string>> Register(UsuarioDtos User) {
            Resposta<string> resposta = new Resposta<string>();
            resposta.Status = false;

            try {
                if(CheckEmailExist(User.Email)) {
                    resposta.Dados = null;
                    resposta.Message = "Email ja cadastrado";
                    return resposta;
                }

                _passwordServices.EncryptPassword(User.Senha, out byte[] senhaHash, out byte[] senhaSalt);

                UsuarioModel usuario = new UsuarioModel() {
                    Usuario = User.Usuario,
                    Email = User.Email,
                    SenhaHash = senhaHash,
                    SenhaSalt = senhaSalt,
                };

                _context.Usuario.Add(usuario);
                await _context.SaveChangesAsync();

                resposta.Status = true;
                resposta.Dados = null;
                resposta.Message = "Cadastro realizado com sucesso!";

                return resposta;

            } catch(Exception er) {
                resposta.Dados = null;
                resposta.Message = er.Message;
            }

            return resposta;
        }

        public Resposta<string> Login(LoginDtos User) {
            Resposta<string> resposta = new Resposta<string>();
            resposta.Status = false;

            try {
                var UserDb = _context.Usuario.FirstOrDefault(usuario => usuario.Email == User.Email);

                if(!CheckEmailExist(User.Email)) {
                    resposta.Message = "O usuario não existe";
                    return resposta;
                }
                if(!_passwordServices.ComparePassword(User.Senha, UserDb!.SenhaHash, UserDb.SenhaSalt)) {
                    resposta.Message = "Senha incorreta";
                    return resposta;
                }

                var jwt = CreateToken(UserDb);
                resposta.Dados = jwt;
                resposta.Message = "Login efetuado com sucesso! Bem-vindo de volta.";
                resposta.Status = true;

            } catch(Exception er) {
                resposta.Dados = null;
                resposta.Message = er.Message;
            }

            return resposta;
        }

        public bool CheckEmailExist(string Email) {
            var userDb =_context.Usuario.FirstOrDefault(userDb => userDb.Email == Email);
            if(userDb == null) {
                return false;
            }
            return true;
        }

        public string CreateToken(UsuarioModel Usuario) {
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config.GetSection("DefaultToken:Token").Value!));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            List<Claim> claims = new List<Claim>(){
                new Claim("Usuario", Usuario.Usuario),
                new Claim("Email", Usuario.Email),
            };

            var token = new JwtSecurityToken(
                issuer: _config.GetSection("Jwt:Issuer").Value,         // 👈 necessário
                audience: _config.GetSection("Jwt:Audience").Value,     // 👈 necessário
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public UsuarioModel? GetClaimAuthToken() {
            UsuarioModel? user;
            var claims = _httpContextAccessor.HttpContext?.User.Claims;
            var email = claims!.FirstOrDefault(c => c.Type == "Email");
            if(!string.IsNullOrEmpty(email?.Value)) {
                user = _context.Usuario.FirstOrDefault(User => User.Email == email!.Value);
            } else {
                user = null;
            }
            return user;
        }

    }
}