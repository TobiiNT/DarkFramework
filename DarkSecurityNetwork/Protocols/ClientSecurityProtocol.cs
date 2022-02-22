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
    public class ClientSecurityProtocol : SecurityNetworkEvent, ISecurityProtocol
    {
        public AESService SymmetricService { get; }
        public RSAService AsymmetricService { get; }

        public ClientSecurityProtocol()
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

        public void ImportAsymmetricPublicKey(string RawPublicKey)
        {
            try
            {
                if (this.AsymmetricService == null)
                {
                    throw new ServiceNotInitializedException(this.AsymmetricService, new Exception("Service is null"));
                }
                else
                {
                    ICryptoKey CryptoKey = new RSAKey(RawPublicKey);

                    this.AsymmetricService.ImportKey(CryptoKey, true);
                }
            }
            catch
            {
                throw;
            }
        }
        public void ImportSymmetricKey(int KeySize, byte[] Key, byte[] IV) => throw new ServiceNotSupportedException("ImportAsymmetricKey", this);

        public void GenerateNewAsymmetricKey(CryptoKeySize CryptoKeySize) => throw new ServiceNotSupportedException("GenerateNewAsymmetricKey", this);
        public void GenerateNewSymmetricKey(CryptoKeySize CryptoKeySize)
        {
            try
            {
                if (this.SymmetricService == null)
                {
                    throw new ServiceNotInitializedException(this.SymmetricService, new Exception("Service is null"));
                }
                else
                {
                    this.SymmetricService.GenerateKey(CryptoKeySize, true);
                }
            }
            catch
            {
                throw;
            }
        }

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
            catch
            {
                throw;
            }
        }
        public void DecryptDataWithAsymmetricPrivateKey(ref byte[] Data) => throw new ServiceNotSupportedException("DecryptDataWithAsymmetricPublicKey", this);

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
            catch
            {
                throw;
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
            catch
            {
                throw;
            }
        }
    }
}
