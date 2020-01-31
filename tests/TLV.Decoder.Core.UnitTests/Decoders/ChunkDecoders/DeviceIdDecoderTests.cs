using FluentAssertions;
using TLV.Decoder.Core.Decoders.ChunkDecoders;
using TLV.Decoder.Core.Models;
using Xunit;

namespace TLV.Decoder.Core.UnitTests.Decoders.ChunkDecoders
{
    public class DeviceIdDecoderTests
    {
        private DeviceIdTlvChunkDecoder _sut;

        public DeviceIdDecoderTests()
        {
            _sut = new DeviceIdTlvChunkDecoder();
        }
        
        [Theory]
        [InlineData("01024B08", 2123)]
        public void Decode_WhenDeviceId_ReturnsCorrectValues(string hexString, ushort expectedValue)
        {
            var result = _sut.Decode(TlvPacketChunk.Create(hexString).Value);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(expectedValue);
        }
    }
}