﻿using DarkGamePacket.Attributes;
using DarkGamePacket.Definitions;
using DarkGamePacket.Definitions.C2S;
using DarkPacket.Interfaces;
using DarkPacket.Readers;
using DarkPacket.Writers;

namespace DarkGamePacket.Packets
{
    public class ListPacketClient
    {
        [PacketType(PacketDirection.IN, PacketID.CHAT_MESSAGE)]
        public static C2S_ChatMessage ChatMessageRequest(byte[] Data) => PacketDeserializer.Deserialize<C2S_ChatMessage>(Data);

        [PacketType(PacketDirection.OUT, PacketID.CHAT_MESSAGE)]
        public static byte[] ChatMessageRequest(ICoreMessage Response) => PacketSerializer.Serialize(Response as C2S_ChatMessage);
    }
}