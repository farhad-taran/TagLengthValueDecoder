using System.Collections.Generic;
using System.Linq;
using TLV.Decoder.Core.Extensions;

namespace TLV.Decoder.Core.Models
{
    public class TlvPacketChunk
    {
        public ushort Type { get; private set; }
        public ushort ExpectedLength { get; private set; }
        public byte[] PayloadBytes { get; private set; }

        public static TlvPacketChunk Create(ushort type, ushort expectedLength, byte[] payload)
        {
            return new TlvPacketChunk
            {
                Type = type,
                ExpectedLength = expectedLength,
                PayloadBytes = payload
            };
        }

        public static TlvPacketChunk Create(string hexString) => Create(hexString.HexStringToByteArray());

        //can change this to return Result<TlvPacketChunk> and do extra validation checks instead of throwing exception bombs
        public static TlvPacketChunk Create(IEnumerable<byte> bytes)
        {
            var chunkBytes = bytes.ToArray();
            ushort type = chunkBytes.First();
            ushort length = chunkBytes.Skip(1).First();
            var content = chunkBytes.Skip(2).ToArray();

            return new TlvPacketChunk
            {
                Type = type,
                ExpectedLength = length,
                PayloadBytes = content
            };
        }
    }
}