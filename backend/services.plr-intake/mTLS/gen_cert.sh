#!/bin/bash
# # Generate a passphrase on mac, brew install pwgen
export PASSPHRASE=$(< /dev/urandom tr -dc _A-Z-a-z-0-9 | head -c${1:-32};echo;)
echo $PASSPHRASE > passphrase.txt
# export PASSPHRASE=$(head -c 500 /dev/urandom | tr -dc a-z0-9A-Z | head -c 128; echo)

# Certificate details; replace items in angle brackets with your own info
casubj="
C=CA
ST=British Columbia
O=MOH
localityName=Victoria
commonName=PIdP mTLS CA
organizationalUnitName=PIdP
emailAddress=primesupport@gov.bc.ca
"
subj="
C=CA
ST=British Columbia
O=MOH
localityName=Victoria
commonName=mauth.healthprovideridentityportal.gov.bc.ca
organizationalUnitName=PIdP
emailAddress=primesupport@gov.bc.ca
"

# Create a CA Private Key
openssl ecparam -name prime256v1 -genkey -noout -out pidp_ca.key

# Create a CA Certificate
openssl req \
    -new \
    -x509 \
    -days 1825 \
    -subj "$(echo -n "$casubj" | tr "\n" "/")" \
    -key pidp_ca.key \
    -passout env:PASSPHRASE \
    -out pidp_ca.crt

# Create the Client Certificate Private Key 
openssl ecparam \
        -name prime256v1 \
        -genkey \
        -noout \
        -out pidp_client.key

# Create the Client CSR
openssl req \
      -new \
      -sha256 \
      -subj "$(echo -n "$subj" | tr "\n" "/")" \
      -days 1825 \
      -passin env:PASSPHRASE \
      -key pidp_client.key \
      -out pidp_client.csr

# Create CA signed client cert
openssl x509 \
      -req \
      -sha256 \
      -in pidp_client.csr \
      -days 1825 \
      -CA pidp_ca.crt \
      -CAkey pidp_ca.key \
      -CAcreateserial \
      -passin env:PASSPHRASE \
      -out pidp_client.crt

cat pidp_client.key pidp_client.crt pidp_ca.crt> pidp_client.pem

# Convert to PKCS #12
openssl pkcs12 \
      -export \
      -in pidp_client.pem \
      -name "PRIME mTLS Certificate" \
      -caname "PRIME mTLS CA" \
      -password env:PASSPHRASE \
      -out pidp_client.pfx

echo -e "\n--- Signed Client PEM ---"
cat pidp_client.pem

echo -e "\n--- Signed Client PEM (base64 encoded)---"
base64 pidp_client.pem

echo -e "\n--- Key Password ---"
echo $PASSPHRASE

echo -e "\n--- Key Password  (base64 encoded) ---"
echo $PASSPHRASE | base64 

echo -e "\n--- Fingerprint ---"
openssl x509 -in pidp_client.crt -noout -fingerprint | cut -d "=" -f2 | awk '{ gsub(/:/,""); print }'

echo -e "\n--- Fingerprint (base64 encoded) ---"
openssl x509 -in pidp_client.crt -outform der | sha1sum | tr " " "-" | awk '{ gsub(/-/,""); print }' | awk '{ print toupper($0) }' | base64
