using System.Security.Cryptography;

namespace Thoughts.services {
    public class SenhaServices : ISenhaInterface {

        public void EncryptPassword(string senha, out byte[] senhaHash, out byte[] senhaSalt) {
            using(var hmc = new HMACSHA512()) {
                senhaSalt = hmc.Key;
                senhaHash = hmc.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senha));
            }
        }

        public bool ComparePassword(string senha, byte[] senhaHash, byte[] senhaSalt) {
            using(var hmc = new HMACSHA512(senhaSalt)) {
                var newSenhaHash = hmc.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senha));
                return newSenhaHash.SequenceEqual(senhaHash);
            }
        }
    }
}