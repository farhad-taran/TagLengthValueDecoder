using TLV.Decoder.Core.Models;

namespace TLV.Decoder.Core.UnitTests.TestData
{
    public class TlvPacketBytesTestData : TheoryData
    {
        public TlvPacketBytesTestData()
        {
            AddRow( // original bytes
            
                new byte[] { 1, 2, 75, 8, 2, 6, 77, 45, 75, 79, 80, 65, 3, 3, 1, 2, 3, 4, 1, 2, 5, 16, 254, 255, 255, 255, 45, 0, 13, 0, 24, 0, 0, 0, 98, 0, 90, 0 },
                2123,
                "M-KOPA",
                new short[]{1,2,3},
                PowerConsumption.NormalConsumption,
                new[]
                {
                    new DeviceTelemetry { Temperature = -2, BatteryLevel = 45, SolarVoltage = 13 },
                    new DeviceTelemetry { Temperature = 24, BatteryLevel = 98, SolarVoltage = 90 },
                }
            );

            AddRow( // out of order bytes
            
                new byte[] { 3, 3, 1, 2, 3, 2, 6, 77, 45, 75, 79, 80, 65, 4, 1, 2, 5, 16, 254, 255, 255, 255, 45, 0, 13, 0, 24, 0, 0, 0, 98, 0, 90, 0, 1, 2, 75, 8, },
                2123,
                "M-KOPA",
                new short[]{1,2,3},
                PowerConsumption.NormalConsumption,
                new[]
                {
                    new DeviceTelemetry { Temperature = -2, BatteryLevel = 45, SolarVoltage = 13 },
                    new DeviceTelemetry { Temperature = 24, BatteryLevel = 98, SolarVoltage = 90 },
                }
            );
        }
    }
}