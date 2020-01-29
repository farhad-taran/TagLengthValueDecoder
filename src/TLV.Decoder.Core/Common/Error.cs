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
        public static Error InCorrectType(string typeHexStr) => new Error("error.incorrect.type", $"the type {typeHexStr} could not be determined");
        public static Error InCorrectLength(string lengthHexStr) => new Error("error.incorrect.length", $"the length {lengthHexStr} can not be parsed");
        public static Error NoCapableDecoderFound(ushort typeHexStr) => new Error("error.decoder.notfound", $"no capable decoder found for {typeHexStr}");
        public static Error InvalidInteger(string hex) => new Error("error.invalid.integer", $"the integer hex {hex} is not valid");
        public static Error PayloadIsEmpty => new Error("error.payload.empty","the payload is empty");
    }
}