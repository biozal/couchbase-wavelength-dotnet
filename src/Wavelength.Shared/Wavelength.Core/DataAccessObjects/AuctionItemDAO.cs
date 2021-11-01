using System;
namespace Wavelength.Core.DataAccessObjects
{
    public class AuctionItemDAO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset StopTime { get; set; }
    }
}
