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
        public byte[] Key { private set; get; }
        public byte[] IV { private set; get; }
        public int KeySize { private set; get; }
    }
}
