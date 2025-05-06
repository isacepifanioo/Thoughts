namespace Thoughts.model {
    public class Resposta<T> {
        public T? Dados {get; set;}
        public string Message {get; set;} = string.Empty;
        public bool Status {get; set;}
    }
}