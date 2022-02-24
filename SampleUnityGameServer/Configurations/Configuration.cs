using System;
using System.Collections.Generic;
using DarkSecurity.Enums;

namespace SampleUnityGameServer.Configurations
{
    public static class Configuration
    {
        public static CryptoKeySize AsymmetricKeySize = CryptoKeySize.Key1024;
        public static CryptoKeySize SymmetricKeySize = CryptoKeySize.Key128;
        public static int MessageTestLength = 32;

        public static List<Tuple<int, uint>> Channels = new List<Tuple<int, uint>>
        {
            new Tuple<int, uint>(3000, 100),
            new Tuple<int, uint>(3001, 100)
        };
    }
}
