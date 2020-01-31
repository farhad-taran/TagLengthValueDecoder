using System.Linq;
using TLV.Decoder.Core.Common;
using TLV.Decoder.Core.Models;

namespace TLV.Decoder.Core.Decoders.ChunkDecoders
{
    public class SoftwareVersionTlvChunkDecoder : ITlvChunkDecoder<ushort[]>
    {
        public Result<ushort[]> Decode(TlvPacketChunk tlvPacketChunk)
        {
            var conversionResults = tlvPacketChunk.PayloadBytes.Select(s => (ushort)s).ToArray();

            return Result<ushort[]>.Success(conversionResults);
        }
    }
}