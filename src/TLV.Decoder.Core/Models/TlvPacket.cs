using System;
using System.Collections.Generic;
using System.Linq;
using TLV.Decoder.Core.Common;
using TLV.Decoder.Core.Extensions;

namespace TLV.Decoder.Core.Models
{
    public class TlvPacket
    {
        public TlvPacketChunk DeviceTelemetriesChunk { get; }
        public TlvPacketChunk PowerConsumptionChunk { get; }
        public TlvPacketChunk SoftwareVersionChunk { get; }
        public TlvPacketChunk CompanyIdChunk { get; }
        public TlvPacketChunk DeviceIdChunk { get; }

        private TlvPacket(
            TlvPacketChunk deviceIdChunk, 
            TlvPacketChunk companyIdChunk, 
            TlvPacketChunk softwareVersionChunk, 
            TlvPacketChunk powerConsumptionChunk, 
            TlvPacketChunk deviceTelemetriesChunk)
        {
            DeviceIdChunk = deviceIdChunk;
            CompanyIdChunk = companyIdChunk;
            SoftwareVersionChunk = softwareVersionChunk;
            PowerConsumptionChunk = powerConsumptionChunk;
            DeviceTelemetriesChunk = deviceTelemetriesChunk;
        }

        public static Result<TlvPacket> Create(string hexString) => Create(hexString.HexStringToByteArray());

        public static Result<TlvPacket> Create(byte[] bytes)
        {
            var remainingBytes = bytes.ToList();
            var chunks = new Dictionary<ChunkType, TlvPacketChunk>();

            while (remainingBytes.Any())
            {
                var chunkResult = TlvPacketChunk.Create(remainingBytes);

                if (chunkResult.IsSuccess == false)
                    return Result<TlvPacket>.Failure(chunkResult.Errors);

                if (chunks.ContainsKey(chunkResult.Value.Type))
                    return Result<TlvPacket>.Failure(Error.CorruptedDataDetected);

                chunks.Add(chunkResult.Value.Type, chunkResult.Value);
                remainingBytes.RemoveRange(0, chunkResult.Value.ChunkTotalSize);
            }

            var actualChunkTypes = chunks.Select(c => c.Key);
            var expectedChunkTypes = new[] { ChunkType.DeviceId, ChunkType.CompanyId, ChunkType.SoftwareVersion, ChunkType.PowerConsumption, ChunkType.DeviceTelemetries };

            var missingChunkTypes = expectedChunkTypes.Where(ct => actualChunkTypes.Contains(ct) == false).ToArray();

            return missingChunkTypes.Any() ? 
                Result<TlvPacket>.Failure(Error.MissingDataDetected(missingChunkTypes)) :
                Result<TlvPacket>.Success(
                    new TlvPacket(
                        chunks[ChunkType.DeviceId],
                        chunks[ChunkType.CompanyId],
                        chunks[ChunkType.SoftwareVersion],
                        chunks[ChunkType.PowerConsumption],
                        chunks[ChunkType.DeviceTelemetries]));
        }
    }
}