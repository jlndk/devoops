#!/usr/bin/env bash

CURRENT_USER=$(whoami)
DOCKER_COMPOSE_VERSION=1.25.5
NODE_EXPORTER_VERSION=0.18.1

if [ "$UID" -ne 0 ]; then
    "This script requires root access (or sudo). Please run it as root or enter your password to continue."
    exec sudo "$0" "$@" 
    exit 0
fi

echo "###################################"
echo "     INSTALLING APT PACKAGES       "
echo "###################################"
apt-get update && apt-get install -y git curl wget tar systemd

echo "###################################"
echo "        INSTALLING DOCKER          "
echo "###################################"
if ! [ -x "$(command -v docker)" ]; then
    curl -fsSL https://get.docker.com | sh
else
    echo "System already has docker installed. Skipping..."
fi

echo "Adding current user to docker group"
usermod -aG docker $CURRENT_USER

echo "###################################"
echo "     INSTALLING DOCKER COMPOSE     "
echo "###################################"
if ! [ -x "$(command -v docker-compose)" ]; then
    echo "Downloading docker-compose..."
    curl -L "https://github.com/docker/compose/releases/download/$DOCKER_COMPOSE_VERSION/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
    echo "Setting permissions..."
    chmod +x /usr/local/bin/docker-compose
else
    echo "System already has docker-compose installed. Skipping..."
fi

echo "###################################"
echo "     SETTING UP NODE_EXPORTER      "
echo "###################################"
if ! [ -x "$(command -v node_exporter)" ]; then
    echo "Downloading node_exporter"
    wget -c "https://github.com/prometheus/node_exporter/releases/download/v$NODE_EXPORTER_VERSION/node_exporter-$NODE_EXPORTER_VERSION.linux-amd64.tar.gz" -O - | tar -xz /usr/local/bin/node_exporter
    echo "Setting permissions..."
    chmod +x /usr/local/bin/node_exporter
else
    echo "System already has node_exporter installed. Skipping..."
fi

NODE_EXPORTER_SERVICE_PATH='/etc/systemd/system/node_exporter.service'
if [ ! -f $NODE_EXPORTER_SERVICE_PATH ]; then
    echo "Registering node_exporter as a systemd service ($NODE_EXPORTER_SERVICE_PATH)"
    touch $NODE_EXPORTER_SERVICE_PATH
    {
        echo 'Description=Node Exporter'
        echo 'Wants=network-online.target'
        echo 'After=network-online.target'
        echo ''
        echo '[Service]'
        echo 'User=node_exporter'
        echo 'Group=node_exporter'
        echo 'Type=simple'
        echo 'ExecStart=/usr/local/bin/node_exporter'
        echo ''
        echo '[Install]'
        echo 'WantedBy=multi-user.target'
    } >> $NODE_EXPORTER_SERVICE_PATH
    echo "Reloading systemd"
    systemctl daemon-reload
else
    echo "System already has node_exporter registered as service. Skipping..."
fi

echo "Enable node_exporter on upstart"
systemctl enable node_exporter 