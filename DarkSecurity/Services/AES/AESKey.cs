using DarkSecurity.Interfaces.Keys;

namespace DarkSecurity.Services.AES
{
    public struct AESKey : ICryptoKey
    {
        public AESKey(int KeySize, byte[] Key, byte[] IV)
        {
            this.KeySize = KeySize;
            this.Key = Key;
            this.IV = IV;
        }
        public byte[] Key { get; }
        public byte[] IV { get; }
        public int KeySize { get; }
    }
}
