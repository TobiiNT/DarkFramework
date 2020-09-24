using DarkSecurity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleUnityGameServer
{
    public static class Configuration
    {
        public static CryptoKeySize AsymmetricKeySize = CryptoKeySize.Key1024;
        public static CryptoKeySize SymmetricKeySize = CryptoKeySize.Key256;
    }
}
