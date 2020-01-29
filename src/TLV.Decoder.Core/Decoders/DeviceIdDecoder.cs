using System;
using TLV.Decoder.Core.Common;
using TLV.Decoder.Core.Models;

namespace TLV.Decoder.Core.Decoders
{
    public class DeviceIdDecoder : IDecoder<ushort>
    {
        public Result<ushort> Decode(TlvPacketChunk tlvPacketChunk)
        {
            var intValue = BitConverter.ToUInt16(tlvPacketChunk.PayloadBytes, 0);

            return Result<ushort>.Success(intValue);
        }
    }
}