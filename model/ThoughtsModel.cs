namespace Thoughts.Model {
    public class ThoughtsModel {
        public int Id {get; set;}
        public string Thought {get; set;} = string.Empty;
        public string Usuario {get; set;} = string.Empty;
        public int UserId {get; set;}
        public DateTime Created_at {get; set;} = DateTime.Now;
    }
}