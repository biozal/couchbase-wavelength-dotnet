using System.Threading.Tasks;
using Wavelength.Models;

namespace Wavelength.Repository
{
    public interface IAuctionItemRepository
    {
        Task<AuctionItems> GetItemsAsync(bool forceRefresh = false);
    }
}
