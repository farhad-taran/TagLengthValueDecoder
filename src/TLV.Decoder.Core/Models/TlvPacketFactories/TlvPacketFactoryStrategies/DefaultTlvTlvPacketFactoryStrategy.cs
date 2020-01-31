using System.Collections.Generic;
using System.Linq;
using TLV.Decoder.Core.Common;

namespace TLV.Decoder.Core.Models.TlvPacketFactories.TlvPacketFactoryStrategies
{
    public class DefaultTlvTlvPacketFactoryStrategy : ITlvPacketFactoryStrategy
    {
        public Result<TlvPacket> Create(byte[] bytes)
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
                    new DefaultTlvPacket(
                        chunks[ChunkType.DeviceId],
                        chunks[ChunkType.CompanyId],
                        chunks[ChunkType.SoftwareVersion],
                        chunks[ChunkType.PowerConsumption],
                        chunks[ChunkType.DeviceTelemetries]));
        }
    }
}