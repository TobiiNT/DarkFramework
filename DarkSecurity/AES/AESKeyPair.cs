namespace DarkSecurity.Securities.AES
{
    public struct AESKeyPair
    {
        public AESKeyPair(int KeySize, byte[] Key, byte[] IV)
        {
            this.KeySize = KeySize;
            this.Key = Key;
            this.IV = IV;
        }
        public byte[] Key { get; set; }
        public byte[] IV { get; set; }
        public int KeySize { get; set; }
    }
}
