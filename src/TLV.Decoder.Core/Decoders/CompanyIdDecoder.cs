using System;
using TLV.Decoder.Core.Common;
using TLV.Decoder.Core.Models;

namespace TLV.Decoder.Core.Decoders
{
    public class CompanyIdDecoder : IDecoder<string>
    {
        public Result<string> Decode(TlvPacketChunk tlvPacketChunk)
        {
            try
            {
                return Result<string>.Success(System.Text.Encoding.ASCII.GetString(tlvPacketChunk.PayloadBytes));
            }
            catch
            {
                return Result<string>.Failure(Error.InvalidCompanyId);
            }
        }
    }
}