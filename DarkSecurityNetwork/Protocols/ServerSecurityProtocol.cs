using DarkSecurity.Enums;
using DarkSecurity.Interfaces.Keys;
using DarkSecurity.Services.AES;
using DarkSecurity.Services.RSA;
using DarkSecurityNetwork.Events;
using DarkSecurityNetwork.Exceptions;
using DarkSecurityNetwork.Interfaces;
using System;

namespace DarkSecurityNetwork.Protocols
{
    public class ServerSecurityProtocol : SecurityNetworkEvent, ISecurityProtocol
    {
        public AESService SymmetricService { private set; get; }
        public RSAService AsymmetricService { private set; get; }

        public ServerSecurityProtocol()
        {
            try
            {
                this.AsymmetricService = new RSAService();
            }
            catch (Exception Exception)
            {
                throw new ServiceNotInitializedException(this.AsymmetricService, Exception);
            }
            try
            {
                this.SymmetricService = new AESService();
            }
            catch (Exception Exception)
            {
                throw new ServiceNotInitializedException(this.SymmetricService, Exception);
            }
        }

        public void ImportAsymmetricPublicKey(string RawPublicString) => throw new ServiceNotSupportedException("ImportAsymmetricKey", this);
        public void ImportSymmetricKey(int KeySize, byte[] Key, byte[] IV)
        {
            try
            {
                if (this.SymmetricService == null)
                {
                    throw new ServiceNotInitializedException(this.SymmetricService, new Exception("Service is null"));
                }
                else
                {
                    ICryptoKey CryptoKey = new AESKey(KeySize, Key, IV);

                    this.SymmetricService.ImportKey(CryptoKey, true);
                }
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public void GenerateNewAsymmetricKey(CryptoKeySize CryptoKeySize)
        {
            try
            {
                if (this.AsymmetricService == null)
                {
                    throw new ServiceNotInitializedException(this.AsymmetricService, new Exception("Service is null"));
                }
                else
                {
                    this.AsymmetricService.GenerateKey(CryptoKeySize, true);
                }
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public void GenerateNewSymmetricKey(CryptoKeySize CryptoKeySize) => throw new ServiceNotSupportedException("GenerateSymmetricKey", this);
        

        public void EncryptDataWithAsymmetricPublicKey(ref byte[] Data)
        {
            try
            {
                if (this.AsymmetricService == null)
                {
                    throw new ServiceNotInitializedException(this.AsymmetricService, new Exception("Service is null"));
                }
                else
                {
                    this.AsymmetricService.Encrypt(ref Data);
                }
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public void DecryptDataWithAsymmetricPrivateKey(ref byte[] Data)
        {
            try
            {
                if (this.AsymmetricService == null)
                {
                    throw new ServiceNotInitializedException(this.AsymmetricService, new Exception("Service is null"));
                }
                else
                {
                    this.AsymmetricService.Decrypt(ref Data);
                }
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public void EncryptDataWithSymmetricAlgorithm(ref byte[] Data)
        {
            try
            {
                if (this.SymmetricService == null)
                {
                    throw new ServiceNotInitializedException(this.SymmetricService, new Exception("Service is null"));
                }
                else
                {
                    this.SymmetricService.Encrypt(ref Data);
                }
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public void DecryptDataWithSymmetricAlgorithm(ref byte[] Data)
        {
            try
            {
                if (this.SymmetricService == null)
                {
                    throw new ServiceNotInitializedException(this.SymmetricService, new Exception("Service is null"));
                }
                else
                {
                    this.SymmetricService.Decrypt(ref Data);
                }
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
    }
}
