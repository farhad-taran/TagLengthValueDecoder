using FluentAssertions;
using TLV.Decoder.Core.Decoders.ChunkDecoders;
using TLV.Decoder.Core.Models;
using Xunit;

namespace TLV.Decoder.Core.UnitTests.Decoders.ChunkDecoders
{
    public class CompanyIdDecoderTests
    {
        private CompanyIdTlvChunkDecoder _sut;

        public CompanyIdDecoderTests()
        {
            _sut = new CompanyIdTlvChunkDecoder();
        }

        [Theory]
        [InlineData("02064D2D4B4F5041", "M-KOPA")]
        public void Decode_WhenCompanyId_ReturnsCorrectValues(string hexString, string expectedValue)
        {
            var result = _sut.Decode(TlvPacketChunk.Create(hexString).Value);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(expectedValue);
        }
    }
}