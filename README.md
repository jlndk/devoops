# MiniTwit DevOps project [![codecov](https://codecov.io/gh/jlndk/devoops/branch/master/graph/badge.svg)](https://codecov.io/gh/jlndk/devoops) ![Run SonarCloud on code](https://github.com/jlndk/devoops/workflows/Run%20SonarCloud%20on%20code/badge.svg?branch=master)

A Twitter clone made for the DevOps course at the IT University of Copenhagen. Over the course of the project, we took a barebones Python web application and both refactored it in a different language (C# with ASP.NET Core) and added many useful DevOps related features and practices. 

This project is:
 - Containerized using the Docker ecosystem, for easy deployment and scalability.
 - Continuously integrated using GitHub Actions, with automatic unit testing, system testing, code linting, static analysis and coverage reporting.
 - Continuously delivered with automatic GitHub releases.
 - Continuously deployed on DigitalOcean.
 - Fully logged using SeriLog, ElasticSearch and Kibana, ensuring scalable and easily accessible logging.
 - Performance monitored with Prometheus and Grafana, with a nice developer dashboard.
 
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
