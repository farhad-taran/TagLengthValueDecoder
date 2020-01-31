using TLV.Decoder.Core.Common;

namespace TLV.Decoder.Core.Models.TlvPacketFactories
{
    public interface ITlvPacketFactory
    {
        Result<TlvPacket> Create(string hexString);
        Result<TlvPacket> Create(byte[] bytes);
    }
}