using Couchbase.Lite;

namespace Wavelength.Services
{
    public interface ICBLiteDatabaseService
    {
	    //information display
        string DatacenterLocation { get; }
	    string ReplicationStatus { get; } 
		string IndexCount { get;  }
		
	    //database management
        bool IsDatabaseInitialized { get; }
        Database AuctionDatabase { get; }
        string DatabaseName { get; }
        string DatabaseDirectoryPath { get; }
        string SyncGatewayUri { get;  }

        void InitDatabase();
        void CloseDatabase();
        void DeleteDatabase();
    }
}