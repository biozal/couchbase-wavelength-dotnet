using Xamarin.Forms;

namespace Wavelength.Repository
{
    public interface ICBLiteAuctionRepository
    {
        void RegisterAuctionCount(Command<int> updateAuctionCount);
        void DeregisterAuctionCount();

        void RegisterBidCount(Command<int> updateBidCount);
        void DeregisterBidCount();
    }
}