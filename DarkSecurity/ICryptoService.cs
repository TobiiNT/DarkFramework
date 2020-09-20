using System;
using System.Collections.Generic;
using System.Text;

namespace DarkSecurity
{
    public interface ICryptoService
    {
        byte[] Encrypt(byte[] Data);
        byte[] Decrypt(byte[] Data);
    }
}
