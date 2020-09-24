using System;
using System.Collections.Generic;
using System.Text;

namespace DarkPacket.Interfaces
{
    public interface IPacketWriter : IDisposable
    {
        public void WriteBoolean(bool Value);
        public void WriteByte(byte Value);
        public void WriteBytes(byte[] Value);
        public void WriteBytes(byte[] Value, int Length);
        public void WriteShort(short Value);
        public void WriteUShort(ushort Value);
        public void WriteInt(int Value);
        public void WriteUInt(uint Value);
        public void WriteFloat(float Value);
        public void WriteLong(long Value);
        public void WriteULong(ulong Value);
        public void WriteDouble(double Value);
        public void WriteString(string Value);
        public void WriteString(string Value, int Length);
        public void WriteDateTime(DateTime DateTime);
        public byte[] GetPacketData();
    }
}
