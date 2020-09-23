namespace DarkSecurity.Interfaces.Services
{
    public interface ICryptoService
    {
        void Encrypt(ref byte[] Data);
        void Decrypt(ref byte[] Data);
    }
}
