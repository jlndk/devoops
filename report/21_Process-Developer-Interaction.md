### Developer interaction
Interaction throughout the project occurred through three main avenues:
- A weekly meeting right after the lecture
- The Teams' Discord server
- Issues and pull requests on the GitHub repository of the project

The meeting was mainly used to organize the team, and will be described closer in the next section about team organization. It is worth noting however, that after  the lockdown caused be Covid-19, the weekly meeting started being held over Discord as well, but no other major changes were made.

#### Tools used

Throughout this course, two existing tools, Github and Discord, has been used to manage the project and communication.

GitHub has been used to track work throughout the project, mainly via Issues assigned to developers. Issues were the main way of tracking what work needed to be done, and by who. Some communication was also done via GitHub in the form of comments on pull requests, to keep the communication about a given pull request contained. 

Github issues worked well as the repository was already used for hosting the code, and the extra features for communication fit well for already quite asynchronous workflow. 

Furthermore, GitHub has the concept of "Pull requests" (PR), which introduces a way to do formal review policy for new code, before merging it with the master branch.
These pull requests can also be linked with the aforementioned issues, which makes it easy to see which code-changes fixes a given issue.
Besides GitHub automatically marking the issue as solved when the PR is merged, this is also immensely useful for documentation purposes, since it is easy to track which is pull requests fixes a given issue.

A feature that worked less well was the GitHub Milestones, which was used for a short while tried using to keep track of what issues were being worked on in a given week, but it was decided after a few weeks to stop using this, as it did not provide enough value compared to the work spent maintaining them. 

Discord has been used where GitHub falls short, namely for instant communication. It has been used for both text-channels for quick questions and requests, and through voice channels for meetings or collaborative work. 

Discord is an easy to use and powerful application that is highly customizable, allowing for separate channels dedicated to different kinds of communication. Such channels are reviews, important links, and so on. Discord also allowed for channels not meant for communication, but for bots that provided monitoring functionality like [Oh Dear](https://ohdear.app/). This acted as a alert channel and made it possible to act fast on any possible problems.

#### Workflow 

The Github workflow starts by creating the issues. The actual content of the issues is agreed upon verbally or dictated by the assignments from the course. The issues are then augmented with relevant metadata, such as description, tags, milestones, assignees, and comments. Most importantly, a tag is used for all mandatory work, so that all tasks that need to be completed can be found instantly.
Occasionally, when many tasks need to be solved in a certain time frame, such as when writing this report, a "milestone" is created in GitHub, where it is possible to attach all relevant issues, set a due date, and afterward track the combined progress for all tasks.

Before working on a task, the developer that is supposed to solve it, starts by assigning themselves to the issue if they were not already assigned when the issue was created.
Afterward, they make a branch and make the changes to the code, as described in (TODO: Refer to GitHub flow chapter). When the task is complete, the developer creates a PR from their branch to master, and augments it with relevant data. By creating this PR, it signals to the other developers that the code is ready for review. Furthermore, the developer also posts a link to the `#review` channel on discord to notify the rest of the team.

Pull requests have to go through the process detailed below before getting merged.
1. All automatic tests and checks must complete successfully. This includes unit tests, end-to-end tests, static analysis, and more
2. The PR must be approved by at least one other developer. Github supports code reviews as a part of the pull requests, so that other team members can, approve, reject, and suggest changes to a PR
3. All requested changes has to be implemented or given a reason for not being changed

The automatic tests help catch bugs and regressions the human reviewer would otherwise miss, and the human usually catches things the automatic tests are not configured or able to detect.
Even though it is essentially impossible to eliminate bugs completely, the combination of automatic tools, combined with human common sense, is the best way of attempting to prevent them.
If a pull request is rejected, the author can make changes and re-request reviews.
When a pull request is merged, it will automatically be deployed to the production environment (TODO: Refer to CI/CD chapter), and mark all linked issues as solved.


