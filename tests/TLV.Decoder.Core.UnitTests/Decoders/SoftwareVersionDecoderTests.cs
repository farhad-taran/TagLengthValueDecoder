using System.Collections.Generic;
using FluentAssertions;
using TLV.Decoder.Core.Decoders;
using TLV.Decoder.Core.Models;
using Xunit;

namespace TLV.Decoder.Core.UnitTests.Decoders
{
    public class SoftwareVersionDecoderTests
    {
        private SoftwareVersionDecoder _sut;

        public SoftwareVersionDecoderTests()
        {
            _sut = new SoftwareVersionDecoder();
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void Decode_WhenSoftwareVersion_ReturnsCorrectValues(string hexString, int[] expectedValue)
        {
            var result = _sut.Decode(TlvPacketChunk.Create(hexString).Value);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(expectedValue);
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[] { "0303010203", new[] { 1, 2, 3 } };
            yield return new object[] { "0303060109", new[] { 6, 1, 9 } };
            yield return new object[] { "0303090507", new[] { 9, 5, 7 } };
        }
    }
}