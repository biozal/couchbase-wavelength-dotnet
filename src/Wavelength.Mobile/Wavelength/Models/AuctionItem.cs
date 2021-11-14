using System;

namespace Wavelength.Models
{
    public class AuctionItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public DateTimeOffset StopTime { get; set; }
        public bool IsWinnerCalculated { get; set; }
        public Guid? WinnerDeviceId { get; set; }
    }
}
