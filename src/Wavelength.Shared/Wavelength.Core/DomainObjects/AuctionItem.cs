using System;
namespace Wavelength.Core.DomainObjects
{
    public class AuctionItem 
        : DomainBase
    {
        public override string? DocumentType { get; set; } = "Auction";

        public string? Title { get; set; }
        public string? ImageUrl { get; set; }
        public DateTimeOffset? StartTime { get; set; }
        public DateTimeOffset? StopTime { get; set; }
        
        public DataAccessObjects.AuctionItemDAO ToAuctionItemDAO()
        {
            return new DataAccessObjects.AuctionItemDAO
            {
                Id = DocumentId.ToString() ?? Guid.NewGuid().ToString(),
                ImageUrl = ImageUrl ?? "",
                Title = Title ?? "",
                StartTime = StartTime ?? DateTime.Now,
                StopTime = StopTime ?? DateTime.Now
            };
        }
    }
}
