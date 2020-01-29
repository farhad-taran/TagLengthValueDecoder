using System.Collections.Generic;
using System.Linq;
using TLV.Decoder.Core.Extensions;

namespace TLV.Decoder.Core.Models
{
    public class TlvPacket
    {
        public TlvPacketChunk DeviceTelemetriesChunk { get; private set; }
        public TlvPacketChunk PowerConsumptionChunk { get; private set; }
        public TlvPacketChunk SoftwareVersionChunk { get; private set; }
        public TlvPacketChunk CompanyIdChunk { get; private set; }
        public TlvPacketChunk DeviceIdChunk { get; private set; }

        private TlvPacket() { }

        public static TlvPacket Create(string hexString) => Create(hexString.HexStringToByteArray());

        //can change this to return Result<TlvPacket> and do extra validation checks instead of throwing exception bombs
        public static TlvPacket Create(byte[] bytes)
        {
            var remainingBytes = bytes.ToList();
            var chunks = new List<TlvPacketChunk>();

            while (remainingBytes.Any())
            {
                var type = remainingBytes.First();
                var sizeToTake = (ushort)remainingBytes.Skip(1).First();
                var payload = remainingBytes.Skip(2).Take(sizeToTake).ToArray();
                chunks.Add(TlvPacketChunk.Create(type, sizeToTake, payload));
                remainingBytes.RemoveRange(0, 2 + sizeToTake);
            }

            return new TlvPacket
            {
                DeviceIdChunk = chunks.ElementAt(0),
                CompanyIdChunk = chunks.ElementAt(1),
                SoftwareVersionChunk = chunks.ElementAt(2),
                PowerConsumptionChunk = chunks.ElementAt(3),
                DeviceTelemetriesChunk = chunks.ElementAt(4)
            };
        }
    }
}