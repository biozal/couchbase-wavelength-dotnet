using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wavelength.Server.WebAPI.Repositories
{
    public interface IAuctionRepository
    {
        Task<Core.DataAccessObjects.AuctionItems> GetAuctionItems(int limit, int skip);
    }
}
