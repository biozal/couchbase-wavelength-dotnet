# Verizon Wavelength with Couchbase Demo  

## Couchbase Server

TODO - document how to setup Couchbase Server and indexes

### Local Debugging of ASP.NET WebAPI and Xamarin App 

TODO - explain process

## Sync Gateway

To run locally you need to use the scripts in the sync directory in this order:

- docker-create-bridge.sh
- docker-cb-server.sh
- docker-init-cluster.sh
- init-docker-env-data.sh
- sync-gateway.sh

You can use the logs-sync-gateway.sh to validate the configuration is working with Sync Gateway.
