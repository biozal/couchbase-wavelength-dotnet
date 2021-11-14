using System;
using System.Diagnostics;
using System.Windows.Input;
using Couchbase.Lite;
using Couchbase.Lite.Query;
using Wavelength.Services;
using Xamarin.Forms;

namespace Wavelength.Repository
{
    public class CBLiteAuctionRepository 
	: ICBLiteAuctionRepository
    {
        private readonly ICBLiteDatabaseService _databaseService;
        private IQuery _queryAuctionCount;
        private ListenerToken _queryAuctionCountToken;
        private IQuery _queryBidCount; 
        private ListenerToken _queryBidCountToken;
        public CBLiteAuctionRepository(ICBLiteDatabaseService databaseService)
        {
            _databaseService = databaseService;
            if (!_databaseService.IsDatabaseInitialized) 
	        {
                _databaseService.InitDatabase();
	        }
            _queryAuctionCount = _databaseService.AuctionDatabase.CreateQuery(@"SELECT COUNT(*) FROM _ as Total WHERE documentType='auction'");
            _queryBidCount = _databaseService.AuctionDatabase.CreateQuery(@"SELECT COUNT(*) FROM _ as Total WHERE documentType='bid'");
        }

        public void RegisterAuctionCount(Command<int> updateAuctionCount)
        {
            _queryAuctionCountToken = _queryAuctionCount.AddChangeListener((sender, args) =>
            {
                var allResults = args.Results.AllResults();
                foreach (var result in allResults)
                {
                    updateAuctionCount.Execute(result[0].Int);
                }
            });
        }

        public void DeregisterAuctionCount()
        {
            _queryAuctionCount.RemoveChangeListener(_queryAuctionCountToken);
        }
        
        public void RegisterBidCount(Command<int> updateBidCount)
        {
            _queryBidCountToken = _queryBidCount.AddChangeListener((sender, args) =>
            {
                var allResults = args.Results.AllResults();
                foreach (var result in allResults)
                {
                    updateBidCount.Execute(result[0].Int);
                }
            });
        }

        public void DeregisterBidCount()
        {
            _queryAuctionCount.RemoveChangeListener(_queryBidCountToken);
        }
    }
}