namespace Thoughts.model {
    public class CommentModel {
        public int Id {get; set;}
        public string Comment {get; set;} = string.Empty;
        public string Usuario {get; set;} = string.Empty;
        public int ThoughtId {get; set;}
        public int UserId {get; set;}
    }
}