using System;
using System.Threading.Tasks;
using Couchbase.Lite;
using Couchbase.Lite.Query;
using Couchbase.Lite.Sync;
using Wavelength.Constants;
using Wavelength.Models;
using Xamarin.Essentials;

namespace Wavelength.Services
{
    public class CBLiteDatabaseService
        : ICBLiteDatabaseService
    {
        private readonly IConnectivityService _connectivityService;
        private Replicator _auctionReplicator;
        private ListenerToken _replicatonChangeToken;
        
        private readonly string _databaseName;
        public string DatabaseName  => _databaseName; 

        private readonly string _directoryPath;
        public string DatabaseDirectoryPath => _directoryPath;
        
	    private readonly string _deviceId;
	    public string DeviceId => _deviceId;
	    
		public string RestApiUri { get; private set; }
        public string SyncGatewayUri { get; private set; } 
        public string DatacenterLocation { get; private set; }
        
        public string LastReplicatorStatus { get; private set; }
	    public string IndexCount { get; private set; }
        public bool IsDatabaseInitialized { get; private set; }
        public Database AuctionDatabase { get; private set; }
		
        public CBLiteDatabaseService(IConnectivityService connectivityService)
        {
            _connectivityService = connectivityService;
            _databaseName = "Auctions";
            _directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _deviceId = Xamarin.Essentials.Preferences.Get(Constants.Preferences.DeviceIdKey, Guid.NewGuid().ToString());
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
		        _auctionReplicator.RemoveChangeListener(_replicatonChangeToken);
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
          
                _replicatonChangeToken = _auctionReplicator.AddChangeListener((o, e) =>
                {
	                if (e.Status.Error != null)
	                {
		                Messaging.Instance.Publish(Constants.Messages.ReplicationError, e.Status.Error);
	                }

	                if (e.Status.Progress.Completed > 0 || e.Status.Progress.Total > 0)
	                {
		                Messaging.Instance.Publish(Constants.Messages.ReplicationProgressUpdate, new ProgressStatus
									{
										Total = e.Status.Progress.Total, 
										Completed = e.Status.Progress.Completed
									});
	                }
	                switch (e.Status.Activity)
	                {
						case ReplicatorActivityLevel.Busy:
							LastReplicatorStatus = Messages.ReplicationStatus.Busy;
							Messaging.Instance.Publish(Constants.Messages.ReplicationChangeStatus, Constants.Messages.ReplicationStatus.Busy);
							break;
						case ReplicatorActivityLevel.Connecting:
							LastReplicatorStatus = Messages.ReplicationStatus.Connecting;
							Messaging.Instance.Publish(Constants.Messages.ReplicationChangeStatus, Constants.Messages.ReplicationStatus.Connecting);
							break;
						case ReplicatorActivityLevel.Offline:
							LastReplicatorStatus = Messages.ReplicationStatus.Offline;
							Messaging.Instance.Publish(Constants.Messages.ReplicationChangeStatus, Constants.Messages.ReplicationStatus.Offline);
							break;
						case ReplicatorActivityLevel.Stopped:
							LastReplicatorStatus = Messages.ReplicationStatus.Stopped;
							Messaging.Instance.Publish(Constants.Messages.ReplicationChangeStatus, Constants.Messages.ReplicationStatus.Stopped);
							break;
						default:
							LastReplicatorStatus = Messages.ReplicationStatus.Idle;
							Messaging.Instance.Publish(Constants.Messages.ReplicationChangeStatus, Constants.Messages.ReplicationStatus.Idle);
							break;
	                }     
                });
                _auctionReplicator.Start();
            }
	    }

        private async Task CalculateSyncGatewayEndpoint() 
	    {
            var isWavelengthApiAvailable = await _connectivityService.IsRemoteReachable(
		                                    Constants.RestUri.WavelengthServerBaseUrl, 
					                        Constants.RestUri.WavelengthServerPort);

            var isWavelengthSyncGatewayAvailable = await _connectivityService.IsRemoteReachable(
		                                    Constants.RestUri.WavelengthSyncGatewayUrl, 
					                        Constants.RestUri.WavelengthSyncGatewayPort);

            if (isWavelengthApiAvailable && isWavelengthSyncGatewayAvailable) 
	        {
		        DatacenterLocation = Constants.Labels.DatacenterLocationWavelength;
		        SyncGatewayUri = $@"{Constants.RestUri.WavelengthSyncGatewayProtocol}://{Constants.RestUri.WavelengthSyncGatewayUrl}:{Constants.RestUri.WavelengthSyncGatewayPort}/{Constants.RestUri.WavelengthSyncGatewayEndpoint}";
		        RestApiUri = $@"{Constants.RestUri.WavelengthServerProtocol}://{Constants.RestUri.WavelengthServerBaseUrl}:{Constants.RestUri.WavelengthServerPort}";
	        } else 
	        { 
		        DatacenterLocation = Constants.Labels.DatacenterLocationCloud;
		        SyncGatewayUri = $@"{Constants.RestUri.CloudSyncGatewayProtocol}://{Constants.RestUri.CloudSyncGatewayUrl}:{Constants.RestUri.CloudSyncGatewayPort}/{Constants.RestUri.CloudSyncGatewayEndpoint}";
		        RestApiUri = $@"{Constants.RestUri.CloudServerProtocol}://{Constants.RestUri.CloudServerBaseUrl}:{Constants.RestUri.CloudServerPort}";
	        }
	    }
    }
}