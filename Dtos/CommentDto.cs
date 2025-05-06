using System.ComponentModel.DataAnnotations;

namespace Thoughts.Dtos {
    public class CommentDto {
        [Required(ErrorMessage ="Seu cometario não possui nenhum argumento")]
        public string Comment {get; set;} = string.Empty;
    }
}