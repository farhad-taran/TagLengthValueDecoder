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
    }
}