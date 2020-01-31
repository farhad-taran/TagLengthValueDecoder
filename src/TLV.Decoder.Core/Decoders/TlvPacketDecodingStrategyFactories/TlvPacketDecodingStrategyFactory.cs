using System.Collections.Generic;
using System.Linq;
using TLV.Decoder.Core.Common;
using TLV.Decoder.Core.Decoders.TlvPacketDecodingStrategyFactories.TlvPacketDecodingStrategies;
using TLV.Decoder.Core.Models;

namespace TLV.Decoder.Core.Decoders.TlvPacketDecodingStrategyFactories
{
    public class TlvPacketDecodingStrategyFactory : ITlvPackDecodingStrategyFactory
    {
        private readonly IEnumerable<ITlvPacketDecodingStrategy> _tlvPacketDecodingStrategies;

        public TlvPacketDecodingStrategyFactory(IEnumerable<ITlvPacketDecodingStrategy> tlvPacketDecodingStrategies)
        {
            _tlvPacketDecodingStrategies = tlvPacketDecodingStrategies;
        }

        public Result<DecodedTlvPacket> Decode(TlvPacket tlvPacket)
        {
            var strategy = _tlvPacketDecodingStrategies.FirstOrDefault(s => s.CanDecode(tlvPacket));

            return strategy == null ? 
                Result<DecodedTlvPacket>.Failure(Error.DecodingStrategyNotFound) : 
                strategy.Decode(tlvPacket);
        }
    }
}