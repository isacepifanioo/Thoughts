using System.ComponentModel.DataAnnotations;

namespace Thoughts.Dtos {
    public class ThoughtsDtos {
        [Required(ErrorMessage="Campo vazio")]
        public string Thought {get; set;} = string.Empty;
    }
}