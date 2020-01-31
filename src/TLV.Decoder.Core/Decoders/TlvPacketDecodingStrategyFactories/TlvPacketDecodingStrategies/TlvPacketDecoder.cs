using TLV.Decoder.Core.Common;
using TLV.Decoder.Core.Models;

namespace TLV.Decoder.Core.Decoders.TlvPacketDecodingStrategyFactories.TlvPacketDecodingStrategies
{
    public abstract class TlvPacketDecoder<T,TU> : ITlvPacketDecodingStrategy where T: TlvPacket where TU: DecodedTlvPacket
    {
        public bool CanDecode(TlvPacket packet)
        {
            return packet.GetType() == typeof(T);
        }

        public Result<DecodedTlvPacket> Decode(TlvPacket tlvPacket)
        {
            var result = DecodeSpecific((T) tlvPacket);

            return result.IsSuccess
                ? Result<DecodedTlvPacket>.Success(result.Value)
                : Result<DecodedTlvPacket>.Failure(result.Errors);
        }

        public abstract Result<TU> DecodeSpecific(T packet);
    }
}