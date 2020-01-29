
namespace TLV.Decoder.Core.Models
{
    public class DeviceTelemetry
    {
        public int Temperature { get; set; }

        public ushort BatteryLevel { get; set; }

        public ushort SolarVoltage { get; set; }
    }
}