using System.ComponentModel.DataAnnotations;

namespace Thoughts.Dtos {
    public class LoginDtos {
        [Required(ErrorMessage ="O campo email é obrigatorio"), EmailAddress(ErrorMessage ="Email Invalido") ]
        public string Email {get; set;} = string.Empty;
        [Required(ErrorMessage ="O campo Senha é obrigatorio")]
        public string Senha {get; set;} = string.Empty;
    }
}