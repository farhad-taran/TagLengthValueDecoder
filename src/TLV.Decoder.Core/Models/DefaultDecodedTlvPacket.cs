namespace TLV.Decoder.Core.Models
{
    /// <summary>
    /// Marker class to allow for polymorphism
    /// </summary>
    public abstract class DecodedTlvPacket
    {

    }

    public class DefaultDecodedTlvPacket : DecodedTlvPacket
    {
        public int DeviceId { get; set; }
        public string CompanyId { get; set; }
        public ushort[] SoftwareVersion { get; set; }
        public PowerConsumption PowerConsumption { get; set; }
        public DeviceTelemetry[] DeviceTelemetries { get; set; }
    }
}