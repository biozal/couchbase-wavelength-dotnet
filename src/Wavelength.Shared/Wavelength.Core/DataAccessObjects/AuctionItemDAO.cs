using System;
namespace Wavelength.Core.DataAccessObjects
{
    public class AuctionItemDAO
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset StopTime { get; set; }
    }
}
