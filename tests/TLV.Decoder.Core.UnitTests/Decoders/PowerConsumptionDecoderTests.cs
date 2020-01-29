using FluentAssertions;
using TLV.Decoder.Core.Decoders;
using TLV.Decoder.Core.Models;
using Xunit;

namespace TLV.Decoder.Core.UnitTests.Decoders
{
    public class PowerConsumptionDecoderTests
    {
        private PowerConsumptionDecoder _sut;

        public PowerConsumptionDecoderTests()
        {
            _sut = new PowerConsumptionDecoder();
        }

        [Theory]
        [InlineData("040100", 0)]
        [InlineData("040101", 1)]
        [InlineData("040102", 2)]
        [InlineData("040103", 3)]
        public void Decode_WhenPowerConsumption_ReturnsCorrectValues(string hexString, int expectedValue)
        {
            var result = _sut.Decode(TlvPacketChunk.Create(hexString));

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be((PowerConsumption)expectedValue);
        }
    }
}