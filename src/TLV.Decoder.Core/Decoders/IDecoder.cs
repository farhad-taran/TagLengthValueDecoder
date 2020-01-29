using TLV.Decoder.Core.Common;
using TLV.Decoder.Core.Models;

namespace TLV.Decoder.Core.Decoders
{
    public interface IDecoder<T>
    {
        Result<T> Decode(TlvPacketChunk tlvPacketChunk);
    }
}