namespace Thoughts.model {
    public class UsuarioModel {
        public int Id {get; set;}
        public string Usuario {get; set;} = string.Empty;
        public string Email {get; set;} = string.Empty;
        public byte[] SenhaHash {get; set;} = [];
        public byte[] SenhaSalt {get; set;} = [];
        public DateTime Created_at {get; set;} = DateTime.Now;
    }
}