#!/usr/bin/env bash

# A simple helper script based on docker-compose that deploys our application.
# It uses the -f flag for docker-compose to tell it what docker-compose file
# to use. In our case we use the production config.
sysctl -w vm.max_map_count=262144
echo "###################################"
echo "        BUILDING CONTAINERS        "
echo "###################################"
# Build the source code into docker images, based on the production config
docker-compose -f docker-compose.prod.yml build

echo ""
echo "###################################"
echo "         STARTING SERVICE          "
echo "###################################"
# Start the app (or replace containers if already running) using the production config.
# The -d flag starts the containers in detached mode (as a daemon), which allows them
# to run in the background, even after the script is finished executing.
docker-compose -f docker-compose.prod.yml up -d

echo ""
echo "###################################"
echo "        DEPLOYMENT FINISHED        "
echo "###################################"
echo "See running containers: docker-compose -f docker-compose.prod.yml ps"
echo "See current logs: docker-compose -f docker-compose.prod.yml logs -f"
echo "Stop containers (THIS WILL CAUSE DOWNTIME!): docker-compose -f docker-compose.prod.yml down"
