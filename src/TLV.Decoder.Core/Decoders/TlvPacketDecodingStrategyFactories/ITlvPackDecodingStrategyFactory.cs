using TLV.Decoder.Core.Common;
using TLV.Decoder.Core.Models;

namespace TLV.Decoder.Core.Decoders.TlvPacketDecodingStrategyFactories
{
    public interface ITlvPackDecodingStrategyFactory
    {
        Result<DecodedTlvPacket> Decode(TlvPacket tlvPacket);
    }
}