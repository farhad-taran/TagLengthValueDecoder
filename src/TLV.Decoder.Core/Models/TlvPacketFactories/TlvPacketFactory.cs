using System.Collections.Generic;
using System.Linq;
using TLV.Decoder.Core.Common;
using TLV.Decoder.Core.Extensions;
using TLV.Decoder.Core.Models.TlvPacketFactories.TlvPacketFactoryStrategies;

namespace TLV.Decoder.Core.Models.TlvPacketFactories
{
    public class TlvPacketFactory : ITlvPacketFactory
    {
        private readonly IEnumerable<ITlvPacketFactoryStrategy> _packetFactoryStrategies;

        public TlvPacketFactory(IEnumerable<ITlvPacketFactoryStrategy> packetFactoryStrategies)
        {
            _packetFactoryStrategies = packetFactoryStrategies;
        }

        public Result<TlvPacket> Create(string hexString) => Create(hexString.HexStringToByteArray());

        public Result<TlvPacket> Create(byte[] bytes)
        {
            var results = _packetFactoryStrategies
                .Select(pfs => pfs.Create(bytes))
                .ToArray();

            var result = results.FirstOrDefault(tlvPacket => tlvPacket.IsSuccess);

            return result ?? Result<TlvPacket>.Failure(results.Select(r => r.Errors));
        }
    }
}