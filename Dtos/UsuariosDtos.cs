using System.ComponentModel.DataAnnotations;

namespace Thoughts.Dtos {
    public class UsuarioDtos {
        [Required(ErrorMessage ="O campo usuario é obrigatorio")]
        public string Usuario {get; set;} = string.Empty;
        [Required(ErrorMessage ="O campo email é obrigatorio"), EmailAddress(ErrorMessage ="Email Invalido") ]
        public string Email {get; set;} = string.Empty;
        [Required(ErrorMessage ="O campo Senha é obrigatorio")]
        public string Senha {get; set;} = string.Empty;
        [Compare("Senha", ErrorMessage ="Senhas não Coincidem")]
        [Required(ErrorMessage ="O campo Confirm Password é obrigatorio")]
        public string ConfirmPassword {get; set;} = string.Empty;
    }
}