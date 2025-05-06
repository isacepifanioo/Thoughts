namespace Thoughts.model {
    public class LikeModel {
        public int Id {get; set;}
        public bool Like {get; set;}
        public int ThoughtId {get; set;}
        public int UserId {get; set;}
    }
}