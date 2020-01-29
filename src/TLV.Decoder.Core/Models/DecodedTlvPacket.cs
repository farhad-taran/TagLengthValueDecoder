namespace TLV.Decoder.Core.Models
{
    public class DecodedTlvPacket
    {
        public int DeviceId { get; set; }
        public string CompanyId { get; set; }
        public ushort[] SoftwareVersion { get; set; }
        public PowerConsumption PowerConsumption { get; set; }
        public DeviceTelemetry[] DeviceTelemetries { get; set; }
    }
}