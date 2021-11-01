#!/bin/bash
curl --location --request PUT 'http://127.0.0.1:4985/wavelengthdb/' \  
--header 'Authorization: Basic c3luY19nYXRld2F5OnBhc3N3b3Jk' \
--header 'Content-Type: application/json' \
--data-raw '{
    "bucket": "wavelength", 
    "num_index_replicas": 0 
    }'