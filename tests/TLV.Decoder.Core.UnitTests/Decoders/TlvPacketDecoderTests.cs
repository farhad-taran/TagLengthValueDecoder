using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using TLV.Decoder.Core.Common;
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
            yield return new object[] // original hex data
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

            yield return new object[] // out of order hex data
            {
                "0510FEFFFFFF2D000D001800000062005A00040102030301020302064D2D4B4F504101024B08",
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
            yield return new object[] // original bytes
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

            yield return new object[] // out of order bytes
            {
                new byte[] { 3, 3, 1, 2, 3, 2, 6, 77, 45, 75, 79, 80, 65, 4, 1, 2, 5, 16, 254, 255, 255, 255, 45, 0, 13, 0, 24, 0, 0, 0, 98, 0, 90, 0, 1, 2, 75, 8, },
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
        [MemberData(nameof(MissingChunksTestData))]
        public void Decode_WhenMissingChunk_ReturnsAppropriateError(byte[] bytes, Error expectedError)
        {
            var result = _sut.Decode(bytes);

            result.IsSuccess.Should().BeFalse();
            result.Errors.Single().Should().BeEquivalentTo(expectedError);
        }

        public static IEnumerable<object[]> MissingChunksTestData()
        {
            yield return new object[]
            {
                new byte[] {2, 6, 77, 45, 75, 79, 80, 65, 3, 3, 1, 2, 3, 4, 1, 2, 5, 16, 254, 255, 255, 255, 45, 0, 13, 0, 24, 0, 0, 0, 98, 0, 90, 0 },
                Error.MissingDataDetected(ChunkType.DeviceId)
            };

            yield return new object[]
            {
                new byte[] { 3, 3, 1, 2, 3, 4, 1, 2, 5, 16, 254, 255, 255, 255, 45, 0, 13, 0, 24, 0, 0, 0, 98, 0, 90, 0 },
                Error.MissingDataDetected(ChunkType.DeviceId, ChunkType.CompanyId)
            };

            yield return new object[]
            {
                new byte[] { 4, 1, 2, 5, 16, 254, 255, 255, 255, 45, 0, 13, 0, 24, 0, 0, 0, 98, 0, 90, 0 },
                Error.MissingDataDetected(ChunkType.DeviceId, ChunkType.CompanyId, ChunkType.SoftwareVersion)
            };

            yield return new object[]
            {
                new byte[] {5, 16, 254, 255, 255, 255, 45, 0, 13, 0, 24, 0, 0, 0, 98, 0, 90, 0 },
                Error.MissingDataDetected(ChunkType.DeviceId, ChunkType.CompanyId, ChunkType.SoftwareVersion, ChunkType.PowerConsumption)
            };

            yield return new object[]
            {
                new byte[] { 1, 2, 75, 8, 2, 6, 77, 45, 75, 79, 80, 65, 3, 3, 1, 2, 3, 4, 1, 2},
                Error.MissingDataDetected(ChunkType.DeviceTelemetries)
            };
        }

        [Theory]
        [MemberData(nameof(CorruptedChunksTestData))]
        public void Decode_WhenDataIsCorrupted_ReturnsAppropriateError(byte[] bytes)
        {
            var result = _sut.Decode(bytes);

            result.IsSuccess.Should().BeFalse();
            result.Errors.Single().Should().BeEquivalentTo(Error.CorruptedDataDetected);
        }

        public static IEnumerable<object[]> CorruptedChunksTestData()
        {
            yield return new object[]
            {
                new byte[] { 1, 2, 8, 2, 6, 77, 45, 75, 79, 80, 65, 3, 3, 1, 2, 3, 4, 1, 2, 5, 16, 254, 255, 255, 255, 45, 0, 13, 0, 24, 0, 0, 0, 98, 0, 90, 0 }
            };

            yield return new object[]
            {
                new byte[] { 1, 2, 75, 8, 2, 6, 77, 45, 79, 80, 65, 3, 3, 1, 2, 3, 4, 1, 2, 5, 16, 254, 255, 255, 255, 45, 0, 13, 0, 24, 0, 0, 0, 98, 0, 90, 0 }
            };

            yield return new object[]
            {
                new byte[] { 1, 2, 75, 8, 2, 6, 77, 45, 75, 79, 80, 65, 3, 3, 1, 2, 4, 1, 2, 5, 16, 254, 255, 255, 255, 45, 0, 13, 0, 24, 0, 0, 0, 98, 0, 90, 0 }
            };

            yield return new object[]
            {
                new byte[] { 1, 2, 75, 8, 2, 6, 77, 45, 75, 79, 80, 65, 3, 3, 1, 2, 3, 4, 1, 5, 16, 254, 255, 255, 255, 45, 0, 13, 0, 24, 0, 0, 0, 98, 0, 90, 0 }
            };

            yield return new object[]
            {
                new byte[] { 1, 2, 75, 8, 2, 6, 77, 45, 75, 79, 80, 65, 3, 3, 1, 2, 3, 4, 1, 2, 5, 16, 254, 255, 255, 255, 45, 0, 13, 0, 24, 0, 0, 0, 0, 90, 0 }
            };
        }
    }
}
