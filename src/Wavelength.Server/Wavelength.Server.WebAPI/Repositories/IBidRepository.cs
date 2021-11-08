using System;
using System.Threading.Tasks;

namespace Wavelength.Server.WebAPI.Repositories
{
    public interface IBidRepository
    {
        public Task<(decimal DbExecutionTime, decimal DbElapsedTime)> CreateBid(string documentId, Core.DomainObjects.Bid bid);
    }
}
