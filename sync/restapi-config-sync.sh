#!/bin/bash
curl -v -X PUT http://127.0.0.1:4985/wavelengthdb/ \  
-H 'Authorization: Basic c3luY19nYXRld2F5OnBhc3N3b3Jk' \
-H 'Content-Type: application/json' \
-d '{ "bucket": "wavelength", "num_index_replicas": 0 }'
