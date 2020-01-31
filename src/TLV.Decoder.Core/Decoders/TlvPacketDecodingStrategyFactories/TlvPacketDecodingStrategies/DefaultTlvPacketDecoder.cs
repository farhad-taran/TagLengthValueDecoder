using System.Linq;
using TLV.Decoder.Core.Common;
using TLV.Decoder.Core.Decoders.ChunkDecoders;
using TLV.Decoder.Core.Models;

namespace TLV.Decoder.Core.Decoders.TlvPacketDecodingStrategyFactories.TlvPacketDecodingStrategies
{
    public class DefaultTlvPacketDecoder : TlvPacketDecoder<DefaultTlvPacket, DefaultDecodedTlvPacket>
    {
        private readonly ITlvChunkDecoder<ushort> _ushortTlvChunkDecoder;
        private readonly ITlvChunkDecoder<string> _asciiStringTlvChunkDecoder;
        private readonly ITlvChunkDecoder<ushort[]> _softwareVersionTlvChunkDecoder;
        private readonly ITlvChunkDecoder<PowerConsumption> _powerConsumptionTlvChunkDecoder;
        private readonly ITlvChunkDecoder<DeviceTelemetry[]> _deviceTelemetriesTlvChunkDecoder;

        public DefaultTlvPacketDecoder(
            ITlvChunkDecoder<ushort> ushortTlvChunkDecoder,
            ITlvChunkDecoder<string> asciiStringTlvChunkDecoder,
            ITlvChunkDecoder<ushort[]> softwareVersionTlvChunkDecoder,
            ITlvChunkDecoder<PowerConsumption> powerConsumptionTlvChunkDecoder,
            ITlvChunkDecoder<DeviceTelemetry[]> deviceTelemetriesTlvChunkDecoder)
        {
            _ushortTlvChunkDecoder = ushortTlvChunkDecoder;
            _asciiStringTlvChunkDecoder = asciiStringTlvChunkDecoder;
            _softwareVersionTlvChunkDecoder = softwareVersionTlvChunkDecoder;
            _powerConsumptionTlvChunkDecoder = powerConsumptionTlvChunkDecoder;
            _deviceTelemetriesTlvChunkDecoder = deviceTelemetriesTlvChunkDecoder;
        }
        
        public override Result<DefaultDecodedTlvPacket> DecodeSpecific(DefaultTlvPacket tlvPacket)
        {
            var deviceIdResult = _ushortTlvChunkDecoder.Decode(tlvPacket.DeviceIdChunk);
            var companyIdResult = _asciiStringTlvChunkDecoder.Decode(tlvPacket.CompanyIdChunk);
            var softwareVersionResult = _softwareVersionTlvChunkDecoder.Decode(tlvPacket.SoftwareVersionChunk);
            var powerConsumptionResult = _powerConsumptionTlvChunkDecoder.Decode(tlvPacket.PowerConsumptionChunk);
            var deviceTelemetriesResult = _deviceTelemetriesTlvChunkDecoder.Decode(tlvPacket.DeviceTelemetriesChunk);

            var results = new Result[]
            {
                deviceIdResult, companyIdResult, softwareVersionResult, powerConsumptionResult, deviceTelemetriesResult
            };

            return results.All(r => r.IsSuccess) ? Result<DefaultDecodedTlvPacket>.Success(new DefaultDecodedTlvPacket
            {
                CompanyId = companyIdResult.Value,
                DeviceId = deviceIdResult.Value,
                SoftwareVersion = softwareVersionResult.Value,
                PowerConsumption = powerConsumptionResult.Value,
                DeviceTelemetries = deviceTelemetriesResult.Value
            }) : Result<DefaultDecodedTlvPacket>.Failure(results.Select(r => r.Errors));
        }
    }
}