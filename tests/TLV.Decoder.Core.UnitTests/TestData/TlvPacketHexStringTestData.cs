using TLV.Decoder.Core.Models;

namespace TLV.Decoder.Core.UnitTests.TestData
{
    public class TlvPacketHexStringTestData : TheoryData
    {
        public TlvPacketHexStringTestData()
        {
            AddRow( // original hex data

                "01024B0802064D2D4B4F504103030102030401020510FEFFFFFF2D000D001800000062005A00",
                2123,
                "M-KOPA",
                new short[] { 1, 2, 3 },
                PowerConsumption.NormalConsumption,
                new[]
                {
                    new DeviceTelemetry { Temperature = -2, BatteryLevel = 45, SolarVoltage = 13 },
                    new DeviceTelemetry { Temperature = 24, BatteryLevel = 98, SolarVoltage = 90 },
                }
            );

            AddRow( // out of order hex data

                "0510FEFFFFFF2D000D001800000062005A00040102030301020302064D2D4B4F504101024B08",
                2123,
                "M-KOPA",
                new short[] { 1, 2, 3 },
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