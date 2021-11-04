using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wavelength.Core.DomainObjects
{
    public class Bid
        : DomainBase
    {
        public override string? DocumentType { get; set; } = "Bid";
        public Guid DeviceId { get; set; }
        public Guid BidId { get; set; }
        public Guid AuctionId { get; set; }
        public DateTimeOffset Sent { get; set; }
        public DateTimeOffset Received { get; set; }
        public double NetworkLatency { get; set; } 
        public string LocationName { get; set; } = string.Empty;

        public DataAccessObjects.BidDAO ToBidDAO()
        {
            return new DataAccessObjects.BidDAO()
            {
                DeviceId = DeviceId,
                DocumentId = DocumentId ?? Guid.NewGuid(),
                BidId = BidId,
                AuctionId = AuctionId,
                Sent = Sent,
                Received = Received,
                LocationName = LocationName
            };
        }
    }
}
