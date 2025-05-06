namespace Thoughts.services {
    public interface ISenhaInterface {
         public void EncryptPassword(string senha, out byte[] senhaHash, out byte[] senhaSalt);
         public bool ComparePassword(string senha, byte[] senhaHash, byte[] senhaSalt);
    }
}