using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wavelength.Core.DataAccessObjects;
using Wavelength.Core.DomainObjects;
using Wavelength.Server.WebAPI.Services;

namespace Wavelength.Server.WebAPI.Repositories
{
    public class AuctionCBLiteRepository
        : IAuctionRepository
    {
        private readonly CouchbaseLiteService _couchbaseLiteService;

        public AuctionCBLiteRepository(CouchbaseLiteService couchbaseLiteService)
        {
            _couchbaseLiteService = couchbaseLiteService;
        }

        public async Task<AuctionItems> GetAuctionItems(int limit, int skip)
        {
            if (_couchbaseLiteService.AuctionDatabase is not null)
            {
                var n1qlQuery = "SELECT * FROM _ AS item WHERE type='auction' AND isActive = true";
                var query = _couchbaseLiteService.AuctionDatabase.CreateQuery(n1qlQuery);
                var results = query.Execute();
                if (results is not null) 
		        {
                    foreach (var result in results) 
		            {
                        var json = result.ToJSON();
		            }
		        }
            }
            await Task.CompletedTask;
            return new AuctionItems(new List<AuctionItem>(), string.Empty, string.Empty);
        }
    }
}
