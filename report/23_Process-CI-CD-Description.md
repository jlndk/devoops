## CI/CD Description

All CI/CD in the project is achieved using GitHub actions. The different steps of the CI/CD process have been split into workflow files with separate responsibilities:

- `dotnetcore.yml` runs unit tests on the project whenever code is pushed.
- `e2e.yml` runs end-to-end tests when a pull request to master is made.
- `sonarPR.yml` provides a report with code analysis from SonarCloud whenever a pull request to master is made. This report is put on the pull request for easy access.
- `sonarMaster.yml` is triggered when code is pushed to master, and updates the master [SonarCloud report](https://sonarcloud.io/dashboard?id=jlndk_devoops) for the project. This is used by SonarCloud to compare reports between master and a branch.
- `deploy.yml` deploys the code on the Digital Ocean server whenever code is pushed to master.
- `release.yml` makes an automatic release of the code on GitHub, including automatic versioning and a copy of the compiled report, whenever code is pushed to master.

Continuous Integration is accomplished through `dotnetcore.yml` and `e2e.yml` by testing code to make sure that it doesn't break when the project changes. `dotnetcore.yml` is triggered on all pushes to speed up testing, and so a developer can check that their code still works before they make a pull request. Ideally `e2e.yml` would also be run on pushes, but it can only make its report if there is a pull request to comment on. Besides that, it would also clutter the actions and takes a long time to run. The same is the case for the `sonarPR.yml`.

Continuous delivery and deployment are accomplished through `deploy.yml`, which on accepted changes to master will roll out these changes. Currently, this is accomplished by connecting to the server from the CI pipeline using SSH, let the server use Git to pull the repo, and then run a script, `deploy.sh`, that uses docker-compose to build and start the docker images based on the project. This has the problem that the server needs to contain all the source code, and some development dependencies, rather than the compiled code of the project. Ideally, a workflow script would instead publish images to Dockerhub which the server would simply pull from and then use an appropriate deployment strategy to replace the outdated containers. This was attempted, but not successfully implemented before the end of the project.

While not part of the CI/CD flow itself, there is also the workflow script `release.yml`, where a release is delivered with every accepted pull request using the [create-release](https://github.com/actions/create-release) action. This is arguably of questionable value in a system that is deployed on a server, but since releases were required for the project, they have been included anyway.
