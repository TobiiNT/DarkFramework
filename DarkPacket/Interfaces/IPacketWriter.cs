using System;

namespace DarkPacket.Interfaces
{
    public interface IPacketWriter : IDisposable
    {
        void WriteBoolean(bool Value);
        void WriteByte(byte Value);
        void WriteBytes(byte[] Value);
        void WriteBytes(byte[] Value, int Length);
        void WriteShort(short Value);
        void WriteUShort(ushort Value);
        void WriteInt(int Value);
        void WriteUInt(uint Value);
        void WriteFloat(float Value);
        void WriteLong(long Value);
        void WriteULong(ulong Value);
        void WriteDouble(double Value);
        void WriteString(string Value);
        void WriteString(string Value, int Length);
        void WriteDateTime(DateTime DateTime);
        byte[] GetPacketData();
    }
}
