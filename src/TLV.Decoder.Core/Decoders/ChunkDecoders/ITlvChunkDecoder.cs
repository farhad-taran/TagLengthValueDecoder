using TLV.Decoder.Core.Common;
using TLV.Decoder.Core.Models;

namespace TLV.Decoder.Core.Decoders.ChunkDecoders
{
    public interface ITlvChunkDecoder<T>
    {
        Result<T> Decode(TlvPacketChunk tlvPacketChunk);
    }
}