using System.Collections.Generic;
using System.Threading.Tasks;
using Wavelength.Core.DataAccessObjects;

namespace Wavelength.Server.WebAPI.Repositories
{
    public interface IAuctionRepository
    {
        Task<AuctionItems> GetAuctionItems(int limit, int skip);
        Task<IEnumerable<string>> CloseEndedAuctions();
    }
}
