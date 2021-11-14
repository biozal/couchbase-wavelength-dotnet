using System;
using System.Threading.Tasks;
using Couchbase.Lite;
using Couchbase.Lite.Query;
using Couchbase.Lite.Sync;

namespace Wavelength.Services
{
    public class CBLiteDatabaseService
        : ICBLiteDatabaseService
    {
        private readonly IConnectivityService _connectivityService;
        private Replicator _auctionReplicator;

        private readonly string _databaseName;
        public string DatabaseName  => _databaseName; 

        private readonly string _directoryPath;
        public string DatabaseDirectoryPath => _directoryPath;

        public string SyncGatewayUri { get; private set; } 
        public string DatacenterLocation { get; private set; }
	    public string ReplicationStatus { get; private set; }
	    public string IndexCount { get; private set; }
        public bool IsDatabaseInitialized { get; private set; }
        public Database AuctionDatabase { get; private set; }

        public CBLiteDatabaseService(IConnectivityService connectivityService)
        {
            _connectivityService = connectivityService;
            _databaseName = "Auctions";
            _directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            IsDatabaseInitialized = false;
        }

        public void InitDatabase()
        {
            Database.Log.Console.Domains = Couchbase.Lite.Logging.LogDomain.All;
            Database.Log.Console.Level = Couchbase.Lite.Logging.LogLevel.Verbose;

            if (AuctionDatabase is null)
            {
                var databaseConfig = new DatabaseConfiguration()
                {
                    Directory = _directoryPath 
                };
                AuctionDatabase = new Database(_databaseName, databaseConfig);

                //setup replication
                Task.Run(async () =>
                {
	                await CreateIndexes();
                    await SetupReplication();
                    IsDatabaseInitialized = true;
                });
            }
        }

        public void CloseDatabase()
        {
            if (AuctionDatabase is not null)
            {
                _auctionReplicator.Stop();

                AuctionDatabase.Close();
                AuctionDatabase.Dispose();
            }
        }

        public void DeleteDatabase() 
	    { 
            if (Database.Exists(_databaseName, _directoryPath)) 
	        {
                //remove replicator
                _auctionReplicator.Stop();
		        _auctionReplicator.Dispose();

                //validate the database is closed first
                AuctionDatabase.Close();
                AuctionDatabase.Dispose();
                Database.Delete(_databaseName, _directoryPath); 
	        }
	    }

        private async Task CreateIndexes()
        {
	        if (AuctionDatabase is not null)
	        {
		        var indexes = AuctionDatabase.GetIndexes();
		        if (!indexes.Contains("documentType"))
		        {
			        AuctionDatabase.CreateIndex(
				        "typeIndex",
						IndexBuilder.ValueIndex(ValueIndexItem.Expression(Expression.Property("documentType")))
				        );
		        }

		        IndexCount = AuctionDatabase.GetIndexes().Count.ToString();
	        }
	        await Task.CompletedTask;
        }

        private async Task SetupReplication() 
	    {
            if (_auctionReplicator is null)
            {
                await CalculateSyncGatewayEndpoint();
                var urlEndpoint = new URLEndpoint(new Uri(SyncGatewayUri));

                var authenticator = new  BasicAuthenticator(Constants.RestUri.SyncGatewayUsername, Constants.RestUri.SyncGatewayPassword);
                var replicationConfig = new ReplicatorConfiguration(AuctionDatabase, urlEndpoint);
                replicationConfig.ReplicatorType = ReplicatorType.Pull;
                replicationConfig.Continuous = true;
                replicationConfig.Authenticator = authenticator;

                _auctionReplicator = new Replicator(replicationConfig);
                _auctionReplicator.Start();
            }
	    }

        private async Task CalculateSyncGatewayEndpoint() 
	    {
            var isWavelengthAPIAvailable = await _connectivityService.IsRemoteReachable(
		                                    Constants.RestUri.WavelengthServerBaseUrl, 
					                        Constants.RestUri.WavelengthServerPort);

            var isWavelengthSyncGatewayAvailable = await _connectivityService.IsRemoteReachable(
		                                    Constants.RestUri.WavelengthSyncGatewayUrl, 
					                        Constants.RestUri.WavelengthSyncGatewayPort);

            if (isWavelengthAPIAvailable && isWavelengthSyncGatewayAvailable) 
	        {
		        DatacenterLocation = Constants.Labels.DatacenterLocationWavelength;
		        SyncGatewayUri = $@"{Constants.RestUri.WavelengthSyncGatewayProtocol}://{Constants.RestUri.WavelengthSyncGatewayUrl}:{Constants.RestUri.WavelengthSyncGatewayPort}/{Constants.RestUri.WavelengthSyncGatewayEndpoint}";
	        } else 
	        { 
		        DatacenterLocation = Constants.Labels.DatacenterLocationCloud;
		        SyncGatewayUri = $@"{Constants.RestUri.CloudSyncGatewayProtocol}://{Constants.RestUri.CloudSyncGatewayUrl}:{Constants.RestUri.CloudSyncGatewayPort}/{Constants.RestUri.CloudSyncGatewayEndpoint}";
	        }
	    }
    }
}