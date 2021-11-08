using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Wavelength.Core.DomainObjects;
using Wavelength.Core.Models;
using Wavelength.Server.WebAPI.Providers;

using Couchbase.KeyValue;

namespace Wavelength.Server.WebAPI.Repositories
{
    public class BidRepository
        : IBidRepository
    {
        private readonly IWavelengthBucketProvider _bucketProvider; 
        private readonly CouchbaseConfig _couchbaseConfig;
        public BidRepository(
            IWavelengthBucketProvider bucketProvider,
            IConfiguration configuration,
            ILogger<BidRepository> logger)
        {
            _bucketProvider = bucketProvider;
             //get config from JSON file configuration
             _couchbaseConfig = new CouchbaseConfig();
            configuration.GetSection(CouchbaseConfig.Section).Bind(_couchbaseConfig);
        }

        public async Task<(decimal DbExecutionTime, decimal DbElapsedTime)> CreateBid(
            string documentId, 
            Bid bid)
        {
            var stopWatch = new Stopwatch();
            var insertStopWatch = new Stopwatch();
            try
            {
                stopWatch.Start();
                var bucket = await _bucketProvider.GetBucketAsync();
                var collection = await bucket.DefaultCollectionAsync();
                //valdiate auction exists
                using (var auction = await collection.GetAsync(bid.AuctionId.ToString(), options => {
                    options.Timeout(TimeSpan.FromSeconds(2));
                }))
                {
                    if (auction is not null)
                    {
                        insertStopWatch.Start();
                        var result = await collection.InsertAsync<Bid>(documentId, bid);
                        insertStopWatch.Stop();
                        stopWatch.Stop();
                        return (
                            DbExecutionTime: new decimal(insertStopWatch.Elapsed.TotalMilliseconds), 
                            DbElapsedTime: new decimal(stopWatch.Elapsed.TotalMilliseconds));
                    }
                    insertStopWatch.Stop();
                    stopWatch.Stop();
                    throw new Exception("Couldn't find Auction based on auctionId passed in");
                }
            }
            catch (Exception)
            {
                stopWatch.Stop();
                insertStopWatch.Stop();
                throw;
            }
        }
    }
}
