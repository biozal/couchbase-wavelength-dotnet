﻿using System;
using System.IO;
using Couchbase.Lite;
using Couchbase.Lite.Sync;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Wavelength.Core.Models;

namespace Wavelength.Server.WebAPI.Services
{
    public class CouchbaseLiteService
    {
        private readonly CouchbaseConfig _couchbaseConfig;
        private readonly ILogger<CouchbaseLiteService> _logger;

        private Database? _db;
        public Database? AuctionDatabase 
	    {
            get => _db;
	    }    
        
        private Replicator? _replicator;
        public Replicator? DatabaseReplicator 
	    {
            get => _replicator;
	    }

        public CouchbaseLiteService(
            ILogger<CouchbaseLiteService> logger,
            IConfiguration configuration) 
	    {
            _logger = logger;
            //get config from JSON file configuration
            _couchbaseConfig = new CouchbaseConfig();
            configuration.GetSection(CouchbaseConfig.Section).Bind(_couchbaseConfig);
#if DEBUG
            Database.Log.Console.Domains = Couchbase.Lite.Logging.LogDomain.All;
            Database.Log.Console.Level = Couchbase.Lite.Logging.LogLevel.Verbose;
#endif
        }

        public void InitDatabase(string filePath)
        {
            try
            {
                var fullPath = $"{filePath}/databases";
                var databaseName = _couchbaseConfig?.DatabaseName;
                _db = new Database(databaseName, new DatabaseConfiguration { Directory = fullPath });

                //start replication
                InitReplication();
            }
            catch (Exception ex) 
	        {
                _logger.LogError(ex.Message, ex.StackTrace);
	        }

	    }

        private void InitReplication() 
	    {
            if (_db is not null 
		        && _couchbaseConfig.SyncGatewayUri is not null
		        && _couchbaseConfig.SyncGatewayUsername is not null 
		        && _couchbaseConfig.SyncGatewayPassword is not null)
            {
                var repConfig = new ReplicatorConfiguration(_db, new URLEndpoint(new Uri(_couchbaseConfig.SyncGatewayUri)));
                var repAuth = new BasicAuthenticator(_couchbaseConfig.SyncGatewayUsername, _couchbaseConfig.SyncGatewayPassword);

                repConfig.Authenticator = repAuth;
                repConfig.Continuous = true;
                repConfig.ReplicatorType = ReplicatorType.PushAndPull;
                
                _replicator = new Replicator(repConfig);
                _replicator.Start();
            }
            else 
	        {
                throw new Core.Exceptions.SyncGatewayConfigMissingException();
	        }

	    }

        public void StopReplication() 
	    { 
            if (_replicator is not null) 
	        {
                _replicator.Stop();
	        }
	    }

        public void Dispose() 
	    {
            if (_replicator is not null)
            {
                _replicator.Dispose();
            }
            if (_db is not null) 
	        {
                _db.Close();
                _db.Dispose();
	        }
            
	    }
    }
}