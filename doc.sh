#!/usr/bin/env bash

shopt -s nocasematch

if [ "${MINITWIT_ENV,,}" = "production" ]; then
    echo "Prod"
    docker-compose -f docker-compose.base.yml -f docker-compose.prod.yml "$@"
elif [ "${MINITWIT_ENV,,}" = "ci" ]; then
    echo "CI"
    docker-compose -f docker-compose.base.yml -f docker-compose.ci.yml "$@"
else
    echo "Dev"
    docker-compose -f docker-compose.base.yml -f docker-compose.dev.yml "$@"
fi