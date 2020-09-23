using PacketDefinition.Definitions;

namespace PacketDefinition
{
    public class PacketReader
    {
        [PacketType(PacketCmd.PKT_C2S_EXIT)]
        public static ExitRequest ReadExitRequest(byte[] data)
        {
            return new ExitRequest();
        }

        [PacketType(PacketCmd.PKT_C2S_SWAP_ITEMS)]
        public static SwapItemsRequest ReadSwapItemsRequest(byte[] data)
        {
            var rq = new PacketDefinitions.C2S.SwapItemsRequest(data);
            return new SwapItemsRequest(rq.NetId, rq.SlotFrom, rq.SlotTo);
        }

        [PacketType(PacketCmd.PKT_C2S_ATTENTION_PING)]
        public static AttentionPingRequest ReadAttentionPingRequest(byte[] data)
        {
            var rq = new PacketDefinitions.C2S.AttentionPingRequest(data);
            return new AttentionPingRequest(rq.X, rq.Y, rq.TargetNetId, rq.Type);
        }

        [PacketType(PacketCmd.PKT_C2S_AUTO_ATTACK_OPTION)]
        public static AutoAttackOptionRequest ReadAutoAttackOptionRequest(byte[] data)
        {
            var rq = new AutoAttackOption(data);
            return new AutoAttackOptionRequest(rq.Netid, rq.Activated == 1);
        }

        [PacketType(PacketCmd.PKT_C2S_CAST_SPELL)]
        public static CastSpellRequest ReadCastSpellRequest(byte[] data)
        {
            var rq = new PacketDefinitions.C2S.CastSpellRequest(data);
            return new CastSpellRequest(rq.NetId, rq.SpellSlot, rq.X, rq.Y, rq.X2, rq.Y2, rq.TargetNetId);
        }

        [PacketType(PacketCmd.PKT_C2S_EMOTION)]
        public static EmotionPacketRequest ReadEmotionPacketRequest(byte[] data)
        {
            var rq = new PacketDefinitions.C2S.EmotionPacketRequest(data);

            // Convert packet emotion type to server emotion type
            // This is done so that when emotion ID changes in other packet versions, its functionality remains the same on server
            Emotions type;
            switch (rq.Id)
            {
                case EmotionType.DANCE:
                    type = Emotions.DANCE;
                    break;
                case EmotionType.TAUNT:
                    type = Emotions.TAUNT;
                    break;
                case EmotionType.LAUGH:
                    type = Emotions.LAUGH;
                    break;
                case EmotionType.JOKE:
                    type = Emotions.JOKE;
                    break;
                default:
                    type = Emotions.UNK;
                    break;
            }

            return new EmotionPacketRequest(rq.NetId, type);
        }
    }
}
