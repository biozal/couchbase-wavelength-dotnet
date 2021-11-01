#!/bin/bash

docker exec -it cb-server-7 couchbase-cli cluster-init -c 127.0.0.1 \
--cluster-username Administrator \
--cluster-password password \
--services data,index,query \
--cluster-ramsize 1024 \
--cluster-index-ramsize 256 

sleep(5)
curl -v -X POST http://localhost:8091/pools/default/buckets \
-u Administrator:password \
-d name=wavelength \
-d ramQuotaMB=512

sleep(10)
curl -v http://localhost:8093/query/service \
  -u Administrator:password \
  -d 'statement=CREATE INDEX idx_wavelength_document_type on wavelength(documentType)'

sleep(5)
curl -v http://localhost:8093/query/service \
  -u Administrator:password \
  -d 'statement=CREATE INDEX idx_wavelength_auction_active on wavelength(isActive)'

docker cp sample-documents-server.json \
cb-server-7:/sample-documents-server.json

docker exec -it cb-server-7 /opt/couchbase/bin/cbimport json --format list \
  -c http://localhost:8091 -u Administrator -p password \
  -d "file:///sample-documents-server.json" -b 'wavelength' -g %documentId%