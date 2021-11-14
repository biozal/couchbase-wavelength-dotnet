using Wavelength.Services;

namespace Wavelength.Repository
{
    public class CBLiteAuctionRepository 
	: ICBLiteAuctionRepository
    {
        private readonly ICBLiteDatabaseService _databaseService;

        public CBLiteAuctionRepository(ICBLiteDatabaseService databaseService)
        {
            _databaseService = databaseService;
            if (!_databaseService.IsDatabaseInitialized) 
	        {
                _databaseService.InitDatabase();
	        }
        }
    }
}