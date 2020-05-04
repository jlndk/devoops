# MiniTwit DevOps project [![codecov](https://codecov.io/gh/jlndk/devoops/branch/master/graph/badge.svg)](https://codecov.io/gh/jlndk/devoops) ![Run SonarCloud on code](https://github.com/jlndk/devoops/workflows/Run%20SonarCloud%20on%20code/badge.svg?branch=master)
> ~~Check our uptime and maintenance status here: https://status.minitwit.tk~~

A Twitter clone made a long time ago, in a galaxy far, far away.
Over the course of the semester, we brought this application up to date and deployed it using all the DevOps skills we have learned. 

The project was developed with Continuous Integration, Delivery and Deployment

This project is:
 - Containerized/Clusterized
 - Monitored
 - Cloud-Powered
 - Data Visualization
 - DevOps Focused

Technologies used:
 - Kibana
 - Grafana
 - Logstash
 - ElasticSearch
 - Docker Swarm
 - ASP.NET
 - .NET Core 3.1

Static analysis tools, CI pipeline, E2E tests, it's all here.

## Quick-start

To deploy on a production node, simply clone this repository to the node and run the following two commands:

```
sudo ./install.sh
./deploy.sh
```

The scripts should install all dependencies

## Setup

This system works on Ubuntu 18.04 LTS, with `docker` and `docker-compose` as direct dependencies, and `node_exporter` as an optional dependency.

On a production environment, the environment variable `MINITWIT_ENV` has to be set to `production` to ensure the scripts run with the correct production variables.

## Usage

On the production environment, simply run

```
./deploy.sh
```

to automatically spin up the docker containers for every part.
This uses a special docker-compose script (`./doc.sh`) to inject proper environment variables and run the docker containers properly.
