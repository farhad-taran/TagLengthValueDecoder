using System;

namespace TLV.Decoder.Core.Models
{
    /// <summary>
    /// Marker class to allow for polymorphic behavior 
    /// </summary>
    public abstract class TlvPacket
    {

    }

    public class DefaultTlvPacket : TlvPacket
    {
        public TlvPacketChunk DeviceTelemetriesChunk { get; }
        public TlvPacketChunk PowerConsumptionChunk { get; }
        public TlvPacketChunk SoftwareVersionChunk { get; }
        public TlvPacketChunk CompanyIdChunk { get; }
        public TlvPacketChunk DeviceIdChunk { get; }

        public DefaultTlvPacket(
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
    }
}