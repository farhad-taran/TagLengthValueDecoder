using System;
using System.Collections.Generic;
using System.Linq;
using TLV.Decoder.Core.Common;
using TLV.Decoder.Core.Extensions;

namespace TLV.Decoder.Core.Models
{
    public class TlvPacketChunk
    {
        public ChunkType Type { get; }
        public ushort ExpectedLength { get; }
        public byte[] PayloadBytes { get; }
        public int ChunkTotalSize => PayloadBytes.Length + 2;

        private TlvPacketChunk(ChunkType type, ushort expectedLength, byte[] payloadBytes)
        {
            Type = type;
            ExpectedLength = expectedLength;
            PayloadBytes = payloadBytes;
        }

        public static Result<TlvPacketChunk> Create(string hexString) => Create(hexString.HexStringToByteArray());

        public static Result<TlvPacketChunk> Create(IEnumerable<byte> bytes)
        {
            try
            {
                var chunkBytes = bytes.ToArray();
                ChunkType type = (ChunkType)chunkBytes.First();
                ushort expectedLength = chunkBytes.Skip(1).First();
                var payloadBytes = chunkBytes.Skip(2).Take(expectedLength).ToArray();

                return payloadBytes.Length != expectedLength ?
                    Result<TlvPacketChunk>.Failure(Error.CorruptedDataDetected) :
                    Result<TlvPacketChunk>.Success(new TlvPacketChunk(type, expectedLength, payloadBytes));
            }
            catch
            {
                return Result<TlvPacketChunk>.Failure(Error.CorruptedDataDetected);
            }
        }
    }
}