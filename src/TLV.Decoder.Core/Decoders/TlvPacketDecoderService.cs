using TLV.Decoder.Core.Common;
using TLV.Decoder.Core.Decoders.TlvPacketDecodingStrategyFactories;
using TLV.Decoder.Core.Extensions;
using TLV.Decoder.Core.Models;
using TLV.Decoder.Core.Models.TlvPacketFactories;

namespace TLV.Decoder.Core.Decoders
{
    public class TlvPacketDecoderService
    {
        private readonly ITlvPacketFactory _tlvPacketFactory;
        private readonly ITlvPackDecodingStrategyFactory _tlvPackDecodingStrategyFactory;

        //can encapsulate all the decoders into an abstract factory or mediator pattern implementation as they all Implement the ITlvChunkDecoder interface
        //Fyi the reason no Mocking is being done here is because we can fully control the outcome in the tests as no HTTP calls or infrastructure
        //calls are being made that could cause slow or non-deterministic behaviour
        public TlvPacketDecoderService(
            ITlvPacketFactory tlvPacketFactory,
            ITlvPackDecodingStrategyFactory tlvPackDecodingStrategyFactory)
        {
            _tlvPacketFactory = tlvPacketFactory;
            _tlvPackDecodingStrategyFactory = tlvPackDecodingStrategyFactory;
        }

        public Result<DecodedTlvPacket> Decode(string hexString) => Decode(hexString.HexStringToByteArray());

        public Result<DecodedTlvPacket> Decode(byte[] bytes)
        {
            var tlvPacketResult = _tlvPacketFactory.Create(bytes);

            return tlvPacketResult.IsSuccess == false ? 
                Result<DecodedTlvPacket>.Failure(tlvPacketResult.Errors) : 
                _tlvPackDecodingStrategyFactory.Decode(tlvPacketResult.Value);
        }
    }
}
