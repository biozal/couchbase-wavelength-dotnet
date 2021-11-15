using System.Threading.Tasks;
using Wavelength.Models;

namespace Wavelength.Repository
{
    public interface IAuctionHttpRepository
    {
        Task<AuctionItemsDao> GetItemsAsync(bool forceRefresh = false);
    }
}
