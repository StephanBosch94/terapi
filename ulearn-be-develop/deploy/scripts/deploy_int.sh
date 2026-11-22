#!/bin/bash
function deploy_archive(){
    service_name="${1}"
    zip_name="${2}"
    dest_folder="${3}"
    echo "Deploying ${service_name} | ${zip_name} | ${dest_folder}" 
    unzipped_folder=$(echo $zip_name | sed "s|.zip||g")
    unzip -qq $zip_name
    rm $zip_name
    sudo systemctl stop $service_name 
    sudo systemctl disable $service_name 
    sudo rm -rf $dest_folder
    sudo cp -r $unzipped_folder/. $dest_folder
    sudo rm -rf $unzipped_folder
    sudo systemctl daemon-reload
    sudo systemctl enable $service_name 
    sudo systemctl start $service_name  
}

deploy_archive "yaspeak-beapi.service" "dist-beapi.zip" "/var/www/beapi"
deploy_archive "yaspeak-fs.service" "dist-fs.zip" "/var/www/fs"
deploy_archive "yaspeak-scheduler.service" "dist-scheduler.zip" "/var/www/scheduler"