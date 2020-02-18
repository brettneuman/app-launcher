namespace Launch.Core.Services
{
    public interface ICipherService
    {
        public string Encrypt(string input);
        public string Decrypt(string cipherText);
    }
}
