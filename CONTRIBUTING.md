## Branching Model

We are using a branching model of feature branches and a master. The master does not allow commits, so all features should come through small feature branches that passes all tests and checks. This also means that all new features should have associated tests. Feature branches should be of limited scope and be done in a short period of time, so as to limit the branch diverting too far from master.

Later in the project, a release branch or development branch may be added. But this does not make sense before a staging server is available to test on. 

## Git Workflow

We are using the [Centralized workflow](https://git-scm.com/book/en/v2/Distributed-Git-Distributed-Workflows) model, given the limited scope and size of the team on this project.

## Pull Requests

Pull requests should not contain install or build dependencies.

If you change the interface of any program, update the readme to reflect the change.

The pull request may be merged after being approved by 1 reviewer for small branches. Major changes should be signed approved by all team members to ensure that learning outcomes are met by all team members.

## Code of Conduct

Having fun is a requirement.