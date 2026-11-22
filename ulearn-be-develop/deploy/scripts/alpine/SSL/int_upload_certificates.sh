#!/bin/bash
SERVER_ADDRESS=yaspeak-int.westeurope.cloudapp.azure.com
USER_NAME=yaspeak_admin

function deploy_cert_if_exists(){
    cert_zip="${1}"
    trg_fld="${2}"
    echo "Uploading [${cert_zip}] to target certificate folder [${trg_fld}]"
    if test -f $cert_zip; then
        scp -i Yaspeak_Int.pem int.beapi.yaspeak.com.zip $USER_NAME@$SERVER_ADDRESS:.
        ssh -i Yaspeak_Int.pem $USER_NAME@$SERVER_ADDRESS 'bash -s' < ./install_certificate.sh $cert_zip $trg_fld
    fi
}


deploy_cert_if_exists "int.beapi.yaspeak.com.zip" "beapi"
deploy_cert_if_exists "int.yaspeak.com.zip" "fe"