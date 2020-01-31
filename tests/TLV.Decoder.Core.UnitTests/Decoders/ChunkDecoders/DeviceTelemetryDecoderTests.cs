using System.Collections.Generic;
using FluentAssertions;
using TLV.Decoder.Core.Decoders.ChunkDecoders;
using TLV.Decoder.Core.Models;
using Xunit;

namespace TLV.Decoder.Core.UnitTests.Decoders.ChunkDecoders
{
    public class DeviceTelemetryDecoderTests
    {
        private DeviceTelemetriesTlvChunkDecoder _sut;

        public DeviceTelemetryDecoderTests()
        {
            _sut = new DeviceTelemetriesTlvChunkDecoder();
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void Decode_WhenDeviceTelemetries_ReturnsCorrectValues(string hexString, DeviceTelemetry[] expectedValue)
        {
            var result = _sut.Decode(TlvPacketChunk.Create(hexString).Value);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(expectedValue);
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[] { "0510FEFFFFFF2D000D001800000062005A00", new[]
            {
                new DeviceTelemetry { Temperature = -2, BatteryLevel = 45, SolarVoltage = 13 },
                new DeviceTelemetry { Temperature = 24, BatteryLevel = 98, SolarVoltage = 90 },
            }};
        }
    }
}