using System.Collections.Generic;
using FluentAssertions;
using TLV.Decoder.Core.Decoders;
using TLV.Decoder.Core.Models;
using Xunit;

namespace TLV.Decoder.Core.UnitTests.Decoders
{
    public class TlvPacketDecoderTests
    {
        private TlvPacketDecoder _sut;

        public TlvPacketDecoderTests()
        {
            _sut = new TlvPacketDecoder(new DeviceIdDecoder(), new CompanyIdDecoder(), new SoftwareVersionDecoder(), new PowerConsumptionDecoder(), new DeviceTelemetriesDecoder());
        }

        [Theory]
        [MemberData(nameof(HexStringTestData))]
        public void Decode_WhenDecodingWithHexString_DecodesCorrectly(string hexString, ushort deviceId, string companyId, short[] softwareVersion, PowerConsumption powerConsumption, DeviceTelemetry[] deviceTelemetries)
        {
            var result = _sut.Decode(hexString);

            result.IsSuccess.Should().BeTrue();
            result.Value.DeviceId.Should().Be(deviceId);
            result.Value.CompanyId.Should().Be(companyId);
            result.Value.SoftwareVersion.Should().BeEquivalentTo(softwareVersion);
            result.Value.PowerConsumption.Should().Be(powerConsumption);
            result.Value.DeviceTelemetries.Should().BeEquivalentTo(deviceTelemetries);
        }

        public static IEnumerable<object[]> HexStringTestData()
        {
            yield return new object[]
            {
                "01024B0802064D2D4B4F504103030102030401020510FEFFFFFF2D000D001800000062005A00",
                2123,
                "M-KOPA",
                new short[]{1,2,3},
                PowerConsumption.NormalConsumption,
                new[]
                {
                    new DeviceTelemetry { Temperature = -2, BatteryLevel = 45, SolarVoltage = 13 },
                    new DeviceTelemetry { Temperature = 24, BatteryLevel = 98, SolarVoltage = 90 },
                }
            };
        }

        [Theory]
        [MemberData(nameof(BytesTestData))]
        public void Decode_WhenDecodingWithBytesArray_DecodesCorrectly(byte[] bytes, ushort deviceId, string companyId, short[] softwareVersion, PowerConsumption powerConsumption, DeviceTelemetry[] deviceTelemetries)
        {
            var result = _sut.Decode(bytes);

            result.IsSuccess.Should().BeTrue();
            result.Value.DeviceId.Should().Be(deviceId);
            result.Value.CompanyId.Should().Be(companyId);
            result.Value.SoftwareVersion.Should().BeEquivalentTo(softwareVersion);
            result.Value.PowerConsumption.Should().Be(powerConsumption);
            result.Value.DeviceTelemetries.Should().BeEquivalentTo(deviceTelemetries);
        }

        public static IEnumerable<object[]> BytesTestData()
        {
            yield return new object[]
            {
                new byte[] { 1, 2, 75, 8, 2, 6, 77, 45, 75, 79, 80, 65, 3, 3, 1, 2, 3, 4, 1, 2, 5, 16, 254, 255, 255, 255, 45, 0, 13, 0, 24, 0, 0, 0, 98, 0, 90, 0 },
                2123,
                "M-KOPA",
                new short[]{1,2,3},
                PowerConsumption.NormalConsumption,
                new[]
                {
                    new DeviceTelemetry { Temperature = -2, BatteryLevel = 45, SolarVoltage = 13 },
                    new DeviceTelemetry { Temperature = 24, BatteryLevel = 98, SolarVoltage = 90 },
                }
            };
        }
    }
}
