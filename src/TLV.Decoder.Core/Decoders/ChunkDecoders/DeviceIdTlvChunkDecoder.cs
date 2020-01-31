using System;
using TLV.Decoder.Core.Common;
using TLV.Decoder.Core.Models;

namespace TLV.Decoder.Core.Decoders.ChunkDecoders
{
    public class DeviceIdTlvChunkDecoder : ITlvChunkDecoder<ushort>
    {
        public Result<ushort> Decode(TlvPacketChunk tlvPacketChunk)
        {
            try
            {
                var deviceId = BitConverter.ToUInt16(tlvPacketChunk.PayloadBytes, 0);

                return Result<ushort>.Success(deviceId);
            }
            catch
            {
                return Result<ushort>.Failure(Error.InvalidDeviceId);
            }
        }
    }
}