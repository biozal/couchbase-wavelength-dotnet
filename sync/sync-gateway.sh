#!/bin/bash
echo "stopping sync-gateway  ..."
docker stop sync-gateway-3-beta02
echo "deleting sync-gateway  ..."
docker rm sync-gateway-3-beta02
echo "Running sync-gateway 3.0-beta02..."
docker run -p 4984-4985:4984-4985 --network demo --name sync-gateway-3-beta02 -d -v `pwd`/sync-gateway-config.json:/etc/sync_gateway/sync_gateway.json couchbase/sync-gateway:3.0.0-beta02-enterprise -adminInterface :4985 /etc/sync_gateway/sync_gateway.json

