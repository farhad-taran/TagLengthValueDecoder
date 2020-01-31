using TLV.Decoder.Core.Common;

namespace TLV.Decoder.Core.Models.TlvPacketFactories.TlvPacketFactoryStrategies
{
    public interface ITlvPacketFactoryStrategy
    {
        Result<TlvPacket> Create(byte[] bytes);
    }
}