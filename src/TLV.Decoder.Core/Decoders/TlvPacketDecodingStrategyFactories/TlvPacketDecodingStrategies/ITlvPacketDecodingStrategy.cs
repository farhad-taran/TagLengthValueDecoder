using TLV.Decoder.Core.Common;
using TLV.Decoder.Core.Models;

namespace TLV.Decoder.Core.Decoders.TlvPacketDecodingStrategyFactories.TlvPacketDecodingStrategies
{
    public interface ITlvPacketDecodingStrategy
    {
        bool CanDecode(TlvPacket packet);
        Result<DecodedTlvPacket> Decode(TlvPacket tlvPacket);
    }
}