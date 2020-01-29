using System.Linq;
using TLV.Decoder.Core.Common;
using TLV.Decoder.Core.Extensions;
using TLV.Decoder.Core.Models;

namespace TLV.Decoder.Core.Decoders
{
    public class TlvPacketDecoder
    {
        private readonly IDecoder<ushort> _ushortDecoder;
        private readonly IDecoder<string> _asciiStringDecoder;
        private readonly IDecoder<ushort[]> _softwareVersionDecoder;
        private readonly IDecoder<PowerConsumption> _powerConsumptionDecoder;
        private readonly IDecoder<DeviceTelemetry[]> _deviceTelemetriesDecoder;

        //can encapsulate all the decoders into an abstract factory or mediator pattern implementation as they all Implement the IDecoder interface
        //Fyi the reason no Mocking is being done here is because we can fully control the outcome in the tests as no HTTP calls or infrastructure
        //calls are being made that could cause slow or non-deterministic behaviour
        public TlvPacketDecoder(
            IDecoder<ushort> ushortDecoder,
            IDecoder<string> asciiStringDecoder,
            IDecoder<ushort[]> softwareVersionDecoder,
            IDecoder<PowerConsumption> powerConsumptionDecoder,
            IDecoder<DeviceTelemetry[]> deviceTelemetriesDecoder)
        {
            _ushortDecoder = ushortDecoder;
            _asciiStringDecoder = asciiStringDecoder;
            _softwareVersionDecoder = softwareVersionDecoder;
            _powerConsumptionDecoder = powerConsumptionDecoder;
            _deviceTelemetriesDecoder = deviceTelemetriesDecoder;
        }

        public Result<DecodedTlvPacket> Decode(string hexString) => Decode(hexString.HexStringToByteArray());

        public Result<DecodedTlvPacket> Decode(byte[] bytes)
        {
            var tlvPacket = TlvPacket.Create(bytes);

            var deviceIdResult = _ushortDecoder.Decode(tlvPacket.DeviceIdChunk);
            var companyIdResult = _asciiStringDecoder.Decode(tlvPacket.CompanyIdChunk);

            //TODO: can use the software version result to determine if other decoders are required because of firmware changes, etc, this can be done by the help of an abstract factory
            var softwareVersionResult = _softwareVersionDecoder.Decode(tlvPacket.SoftwareVersionChunk);
            var powerConsumptionResult = _powerConsumptionDecoder.Decode(tlvPacket.PowerConsumptionChunk);
            var deviceTelemetriesResult = _deviceTelemetriesDecoder.Decode(tlvPacket.DeviceTelemetriesChunk);

            var results = new Result[]
            {
                deviceIdResult, companyIdResult, softwareVersionResult, powerConsumptionResult, deviceTelemetriesResult
            };

            return results.All(r => r.IsSuccess) ? Result<DecodedTlvPacket>.Success(new DecodedTlvPacket
            {
                CompanyId = companyIdResult.Value,
                DeviceId = deviceIdResult.Value,
                SoftwareVersion = softwareVersionResult.Value,
                PowerConsumption = powerConsumptionResult.Value,
                DeviceTelemetries = deviceTelemetriesResult.Value
            }) : Result<DecodedTlvPacket>.Failure(results.Select(r => r.Errors));
        }
    }
}
