using TLV.Decoder.Core.Models;

namespace TLV.Decoder.Core.Common
{
    public class Error
    {
        public string Code { get; }
        public string Message { get; }

        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        //can use these error types when validating and returning results instead of throwing exception bombs
        public static Error InvalidDeviceId => new Error("error.invalid.deviceid", "the payload for deviceId is not valid");
        public static Error InvalidCompanyId => new Error("error.invalid.companyid", "the payload for companyId is not valid");
        public static Error InvalidPowerConsumption => new Error("error.invalid.powerconsumption", "the payload for powerConsumption is not valid");
        public static Error CorruptedDataDetected => new Error("error.packet.corrupted", "the packet is corrupted");
        public static Error PacketFactoryNotFound => new Error("error.packet.unknown", "the packet is in an unknown format");
        public static Error DecodingStrategyNotFound => new Error("error.decoding.not.implemented", "there is currently no decoding strategy implemented for this packet");

        public static Error MissingDataDetected(params ChunkType[] chunks) => new Error("error.packet.missing.chunk", $"the packet is missing {string.Join(",", chunks)} required chunks");
    }
}