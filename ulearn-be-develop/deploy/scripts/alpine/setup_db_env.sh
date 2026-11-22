sudo sh -c 'echo "deb http://apt.postgresql.org/pub/repos/apt $(lsb_release -cs)-pgdg main" > /etc/apt/sources.list.d/pgdg.list'
wget --quiet -O - https://www.postgresql.org/media/keys/ACCC4CF8.asc | sudo apt-key add -
sudo apt-get update
sudo apt-get -y install postgresql
sudo service postgresql start
echo 'PATH=$PATH:/usr/lib/postgresql/15/bin' >> ~/.profile
sudo -u postgres psql -c "CREATE USER technical_admin WITH PASSWORD 'ox5bWk4G6Qob';"
sudo -u postgres psql -c "ALTER USER technical_admin CREATEDB; ALTER USER technical_admin CREATEROLE; ALTER USER technical_admin WITH SUPERUSER;"
sudo -u postgres psql -c "CREATE DATABASE yaspeak;"
sudo -u postgres psql -c "GRANT ALL PRIVILEGES ON DATABASE yaspeak to technical_admin;"
sudo nano /etc/postgresql/15/main/postgresql.conf #listen_addresses= '*' instead of localhost and don't forget to remove #
sudo nano /etc/postgresql/15/main/pg_hba.conf #0.0.0.0/0
sudo systemctl restart postgresql