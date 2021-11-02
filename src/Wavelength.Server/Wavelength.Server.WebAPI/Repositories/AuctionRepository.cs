using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Couchbase.Extensions.DependencyInjection;
using Couchbase.Query;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Wavelength.Core.DataAccessObjects;
using Wavelength.Core.DomainObjects;
using Wavelength.Core.Models;

namespace Wavelength.Server.WebAPI.Repositories
{
    public class AuctionRepository 
	    : IAuctionRepository
    {
        private readonly IClusterProvider _clusterProvider;
        private readonly ILogger<AuctionRepository> _logger;
        private readonly CouchbaseConfig _couchbaseConfig;

        public AuctionRepository(
            IClusterProvider clusterProvider,
	        ILogger<AuctionRepository> logger,
	        IConfiguration configuration)
        {
            _logger = logger;
            _clusterProvider = clusterProvider;
            //get config from JSON file configuration
            _couchbaseConfig = new CouchbaseConfig();
            configuration.GetSection(CouchbaseConfig.Section).Bind(_couchbaseConfig);
        }

        public async Task<AuctionItems> GetAuctionItems(int limit = 25, int skip = 0)
        {
            //assume index is created on the server or this will go very poor
            //if running slow, check this index to make sure it's created
            //CREATE INDEX document_type_idx on `wavelength` (documentType, isActive)
            try
            {
                //validate we can see the proper config
                if (_clusterProvider is not null
                    && _couchbaseConfig is not null
                    && _couchbaseConfig.BucketName is not null)
                {
                    //query the database and get items back
                    var cluster = await _clusterProvider.GetClusterAsync();
                    var sb = new System.Text.StringBuilder("SELECT a.* FROM ");
                    sb.Append($"{_couchbaseConfig.BucketName} a ");
                    sb.Append("WHERE lower(a.documentType) = 'auction' ");
                    sb.Append("AND ");
                    sb.Append("a.isActive = true ");
                    sb.Append($"LIMIT {limit} ");
                    sb.Append($"OFFSET {skip} ");

                    //Query the database - see full documenation
		            //https://docs.couchbase.com/dotnet-sdk/current/howtos/n1ql-queries-with-sdk.html
                    var query = sb.ToString();
                    var queryOptions = new QueryOptions().Metrics(true);
                    queryOptions.ScanConsistency(QueryScanConsistency.RequestPlus);
                    queryOptions.Readonly(true);

                    var results = await cluster.QueryAsync<AuctionItem>(query, queryOptions).ConfigureAwait(false);
                    if (results is not null && results?.MetaData?.Status == QueryStatus.Success)
                    {
                        var listAuctions = await results.Rows.ToListAsync<AuctionItem>();
                        if (listAuctions is not null && listAuctions.Count > 0)
                        {
                            var auctionItems = new AuctionItems(
				                listAuctions, 
				                results.MetaData.Metrics.ExecutionTime,
                                results.MetaData.Metrics.ElapsedTime); 
                            return auctionItems; 
                        }
                    }
                }
            } 
            catch (Exception ex) 
	        {
                _logger.LogError(ex.Message, ex.StackTrace);
	        }
            return new AuctionItems(new List<AuctionItem>(), string.Empty, string.Empty); 
        }
    }
}
