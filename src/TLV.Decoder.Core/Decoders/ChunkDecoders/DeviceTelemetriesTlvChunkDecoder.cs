using System;
using System.Collections.Generic;
using System.Linq;
using TLV.Decoder.Core.Common;
using TLV.Decoder.Core.Models;

namespace TLV.Decoder.Core.Decoders.ChunkDecoders
{
    public class DeviceTelemetriesTlvChunkDecoder : ITlvChunkDecoder<DeviceTelemetry[]>
    {
        public Result<DeviceTelemetry[]> Decode(TlvPacketChunk tlvPacketChunk)
        {
            var chunks = ChunkBy(tlvPacketChunk.PayloadBytes.ToList(), 8);

            var deviceTelemetries = chunks.Select(MapToDeviceTelemetry).ToArray();

            return Result<DeviceTelemetry[]>.Success(deviceTelemetries);
        }

        public static List<List<T>> ChunkBy<T>(List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        private DeviceTelemetry MapToDeviceTelemetry(List<byte> bytes)
        {
            var temperature = BitConverter.ToInt32(bytes.Take(4).ToArray(),0);
            ushort batteryLevel = bytes.Skip(4).Take(1).SingleOrDefault();
            ushort solarVoltage = bytes.Skip(6).Take(1).SingleOrDefault();

            return new DeviceTelemetry
            {
                BatteryLevel = batteryLevel,
                SolarVoltage = solarVoltage,
                Temperature = temperature
            };
        }
    }
}