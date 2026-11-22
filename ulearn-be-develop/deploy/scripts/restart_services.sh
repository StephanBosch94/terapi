#!/bin/bash
function restart_service(){
    service_name="${1}"
    sudo systemctl restart $service_name
}


restart_service "yaspeak-beapi.service" 
restart_service "yaspeak-fs.service"
restart_service "yaspeak-scheduler.service"