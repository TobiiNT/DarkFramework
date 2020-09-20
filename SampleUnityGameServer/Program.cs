using DarkPacket.Packets;
using DarkSecurity;
using DarkSecurity.Securities.AES;
using DarkSecurity.Securities.RSA;
using SampleUnityGameServer.Networks;
using System;
using System.Linq;
using System.Text;

namespace SampleUnityGameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            SocketListener Listener = new SocketListener();
            Listener.StartListening(3333);

            //byte[] RawData = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 };

            //AESKeyGenerator KeyGenerator = new AESKeyGenerator();
            //
            //AESKeyPair KeyPair = KeyGenerator.GenerateKey(AESKeySize.Key192);
            //AESService AESService = new AESService(KeyPair);
            //
            //byte[] AESEncryptedData = AESService.Encrypt(RawData);
            //Console.WriteLine($"{BitConverter.ToString(AESEncryptedData)}");
            //byte[] AESDecryptedData = AESService.Decrypt(AESEncryptedData);
            //Console.WriteLine($"{BitConverter.ToString(AESDecryptedData)}");

            //RSAKeyGenerator KeyGenrator = new RSAKeyGenerator();
            //
            //RSAKeyPair KeyPair = KeyGenrator.GenerateKey(RSAKeySize.Key1024);
            //RSAService RSAService = new RSAService(KeyPair);
            //
            //byte[] RSAEncryptedData = RSAService.Encrypt(RawData);
            //Console.WriteLine($"{BitConverter.ToString(RSAEncryptedData)}");
            //byte[] RSADecryptedData = RSAService.Decrypt(RSAEncryptedData);
            //Console.WriteLine($"{BitConverter.ToString(RSADecryptedData)}");

            while (true)
            {
                string Content = Console.ReadLine();
                if (Content == "exit")
                    break;

                Logging.WriteLine($"Total Connections : {World.Connections.Count}");
                foreach (var Connection in World.Connections.ToList())
                {
                    using (PacketWriter Packet = new PacketWriter())
                    {
                        Packet.WriteString(Content);

                        byte[] Data = Packet.GetPacketData();
                        Connection.Send(Data, Data.Length);

                        using (PacketReader Reader = new PacketReader(Data))
                        {
                            Console.WriteLine(Reader.ReadString());
                        }
                    }
                }
            }

            Console.ReadKey();
        }
    }
}
