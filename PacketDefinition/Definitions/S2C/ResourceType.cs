namespace PacketDefinition.Definitions.S2C
{
    public class ResourceType : BasePacket
    {
        public ResourceType(uint playernetid, byte resourceType)
            : base(PacketCmd.PKT_S2C_RESOURCE_TYPE, playernetid)
        {
            Write(resourceType);
        }
    }
}