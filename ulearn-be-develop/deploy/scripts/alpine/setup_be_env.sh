#!/bin/bash
wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
sudo chmod +x ./dotnet-install.sh
./dotnet-install.sh --version latest
echo 'DOTNET_ROOT=$HOME/.dotnet' >> ~/.profile
echo 'PATH=$PATH:$HOME/.dotnet:$HOME/.dotnet/tools' >> ~/.profile
echo 'export ASPNETCORE_ENVIRONMENT=Int' >> ~/.profile
sudo apt install zip
sudo apt install unzip

#https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx?view=aspnetcore-6.0&tabs=linux-ubuntu
sudo apt install nginx
sudo service nginx start
sudo nginx -s reload
sudo nano /etc/systemd/system/yaspeak-beapi.service
sudo nano /etc/nginx/nginx.conf
sudo nano /etc/nginx/sites-available/default