using System.Threading.Tasks;
using Wavelength.Models;

namespace Wavelength.Repository
{
    public interface IAuctionHttpRepository
    {
        Task<AuctionItems> GetItemsAsync(bool forceRefresh = false);
    }
}
