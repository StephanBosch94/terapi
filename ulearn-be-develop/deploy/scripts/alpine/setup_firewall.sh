sudo apt-get install ufw
sudo ufw default allow incoming
sudo ufw allow 5432
sudo ufw allow 80
sudo ufw allow 22/tcp
sudo ufw allow 80/tcp
sudo ufw allow 443/tcp
sudo ufw allow http
sudo ufw allow https
sudo ufw enable
sudo ufw status
sudo ufw allow 'Nginx HTTP'
sudo ufw allow 'Nginx HTTPS'