#!/usr/bin/env bash

DOCKER_COMPOSE_VERSION=1.25.5
NODE_EXPORTER_VERSION=0.18.1

if [ "$UID" -ne 0 ]; then
    echo "This script requires root access (or sudo). Attemping to escalate privilegdes using sudo (you might need to enter your password to proceed)."
    exec sudo "$0" "$@" 
    exit 0
fi

if [ $SUDO_USER ]; then 
    CURRENT_USER=$SUDO_USER; 
else 
    CURRENT_USER=`whoami`; 
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

echo "Adding user '$CURRENT_USER' to docker group"
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
    wget -c "https://github.com/prometheus/node_exporter/releases/download/v$NODE_EXPORTER_VERSION/node_exporter-$NODE_EXPORTER_VERSION.linux-amd64.tar.gz" -O - | tar -xz --strip-components=1 -C /usr/local/bin node_exporter-$NODE_EXPORTER_VERSION.linux-amd64/node_exporter
    echo "Setting permissions..."
    chmod +x /usr/local/bin/node_exporter
else
    echo "System already has node_exporter installed. Skipping..."
fi

if ! id -u node_exporter > /dev/null 2>&1; then
    sudo useradd --no-create-home --shell /bin/false node_exporter
fi

NODE_EXPORTER_SERVICE_PATH='/etc/systemd/system/node_exporter.service'
if [ ! -f $NODE_EXPORTER_SERVICE_PATH ]; then
    echo "Registering node_exporter as a systemd service at path $NODE_EXPORTER_SERVICE_PATH"
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
echo "Starting node_exporter"
systemctl start node_exporter 

echo "###################################"
echo "           FINAL STEPS             "
echo "###################################"
# If 'MINITWIT_ENV' is not set, add it to bashrc
if [ -z ${MINITWIT_ENV+x} ]; then
    echo "Adding MINITWIT_ENV to bashrc"
    echo "export MINITWIT_ENV=production" >> ~/.bashrc
    source ~/.bashrc
else
    echo "MINITWIT_ENV is already set. Skipping..."
fi

echo ' (             (        )             )    (         (   (       )     ) '
echo ' )\ )     (    )\ )  ( /(    *   ) ( /(    )\ )      )\ ))\ ) ( /(  ( /( '
echo '(()/((    )\  (()/(  )\()) ` )  /( )\())  (()/(  (  (()/(()/( )\()) )\())'
echo ' /(_))\((((_)( /(_))((_)\   ( )(_)|(_)\    /(_)) )\  /(_))(_)|(_)\ ((_)\ '
echo '(_))((_))\ _ )(_))___ ((_) (_(_())  ((_)  (_))_ ((_)(_))(_))   ((_)_ ((_)'
echo '| _ \ __(_)_\(_)   \ \ / / |_   _| / _ \   |   \| __| _ \ |   / _ \ \ / /'
echo '|   / _| / _ \ | |) \ V /    | |  | (_) |  | |) | _||  _/ |__| (_) \ V / '
echo '|_|_\___/_/ \_\|___/ |_|     |_|   \___/   |___/|___|_| |____|\___/ |_|  '
echo "PLEASE log in and out (by reconnecting via SSH) for everything to work correctly!"

echo "###################################"
echo "              USAGE                "
echo "###################################"
echo " Run './deploy.sh' to deploy the app."
echo " Run './doc.sh' to interact with docker-compose."
echo " Note: You might need to  to use docker without sudo."