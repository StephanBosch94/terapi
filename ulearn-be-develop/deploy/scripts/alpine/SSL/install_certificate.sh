#!/bin/bash
ZIP_NAME=$1
CERTIFICATES_FOLDER=$2
TARGET_FOLDER="/etc/ssl/certs/yaspeak"
echo "  zip = [${ZIP_NAME}] to folder [$TARGET_FOLDER/$CERTIFICATES_FOLDER]"

rm -rf $CERTIFICATES_FOLDER cert.crt private.key certificate.crt ca_bundle.crt
unzip $ZIP_NAME
sudo cat certificate.crt ca_bundle.crt > cert.crt
sudo rm -rf $TARGET_FOLDER/$CERTIFICATES_FOLDER/*
sudo mkdir $TARGET_FOLDER/$CERTIFICATES_FOLDER
sudo chmod -x cert.crt private.key
sudo cp -rf cert.crt $TARGET_FOLDER/$CERTIFICATES_FOLDER
sudo cp -rf private.key $TARGET_FOLDER/$CERTIFICATES_FOLDER
rm $ZIP_NAME ca_bundle.crt certificate.crt private.key cert.crt
sudo rm -rf ca_bundle.crt certificate.crt
sudo nginx -s reload