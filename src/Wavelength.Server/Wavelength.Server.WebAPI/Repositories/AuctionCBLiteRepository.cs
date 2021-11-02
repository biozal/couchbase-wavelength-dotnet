using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Wavelength.Core.DataAccessObjects;
using Wavelength.Core.DomainObjects;
using Wavelength.Core.DTO;
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

        public async Task<AuctionItems> GetAuctionItems(int limit = 50, int skip = 0)
        {
            if (_couchbaseLiteService.AuctionDatabase is not null)
            {
                var n1qlQuery = $"SELECT * FROM _ AS item WHERE documentType='auction' AND isActive = true LIMIT {limit} OFFSET {skip}";
                var query = _couchbaseLiteService.AuctionDatabase.CreateQuery(n1qlQuery);
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                var results = query.Execute().AllResults();
                stopWatch.Stop();
                if (results is not null) 
		        {
                    var items = new List<AuctionItem>();
                    foreach (var result in results) 
		            {
                        var json = result.ToJSON();
                        var dto = JsonConvert.DeserializeObject<CBLiteAuctionItemDTO>(json);
                        if (dto.Item is not null)
                        {
                            items.Add(dto.Item);
                        }
		            }
                    query.Dispose();
                    return new AuctionItems(items, $"{stopWatch.Elapsed.TotalMilliseconds}ms", $"{stopWatch.Elapsed.TotalMilliseconds}ms");
		        }
            }
            await Task.CompletedTask;
            return new AuctionItems(new List<AuctionItem>(), string.Empty, string.Empty);
        }
    }
}
