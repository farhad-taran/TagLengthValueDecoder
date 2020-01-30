using FluentAssertions;
using TLV.Decoder.Core.Decoders;
using TLV.Decoder.Core.Models;
using Xunit;

namespace TLV.Decoder.Core.UnitTests.Decoders
{
    public class CompanyIdDecoderTests
    {
        private CompanyIdDecoder _sut;

        public CompanyIdDecoderTests()
        {
            _sut = new CompanyIdDecoder();
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