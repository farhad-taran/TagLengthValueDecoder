using System.Linq;
using TLV.Decoder.Core.Common;
using TLV.Decoder.Core.Models;

namespace TLV.Decoder.Core.Decoders.ChunkDecoders
{
    public class PowerConsumptionTlvChunkDecoder : ITlvChunkDecoder<PowerConsumption>
    {
        public Result<PowerConsumption> Decode(TlvPacketChunk tlvPacketChunk)
        {
            try
            {
                int intValue = tlvPacketChunk.PayloadBytes.Single();

                return Result<PowerConsumption>.Success((PowerConsumption)intValue);
            }
            catch
            {
                return Result<PowerConsumption>.Failure(Error.InvalidPowerConsumption);
            }
        }
    }
}