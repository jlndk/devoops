### Applied development process and tools supporting it

Throughout this course, two tools, Github and Discord, has been used to manage the project and communication.
Discord is used to communicate directly and informally to allow for fast and efficient communication,
whereas GitHub is used to organize our work in a way that can easily be searched through and understood at a later date.

GitHub has been the ideal tool for the majority of our project management, because both the code, git commits, and corresponding issues coexist and can easily be cross-referenced.
Furthermore, GitHub has the concept of "Pull requests" (PR), which introduces a way to do formal review policy for new code, before merging it with the master branch.
These pull requests can also be linked with the aforementioned issues, which makes it easy to see which code-changes fixes a given issue.
Besides GitHub automatically marking the issue as solved when the PR is merged, this is also immensely useful for documentation purposes, since it is easy to track which is pull requests fixes a given issue.

The Github workflow starts by creating the issues. The actual content of the issues is often agreed upon verbally or dictated by the assignments from the course. The issues are then augmented with relevant metadata, such as description, tags, milestones, assignees, and comments. Most importantly, a tag is used for all mandatory work, so that all tasks that needs to be completed can be found instantly.
Occasionally, when many tasks need to be solved in a certain timeframe, such as when writing this report,a "milestone" is created in GitHub.
In this, it is possible to attach all relevant issues, set a due date, and afterward track the combined progress for all tasks.
Before working on a task, the developer that is supposed to solve it, starts by assigning themself to the issue if they were not already assigned when the issue was created.
Afterward, they make a branch and make the changes to the code, as described in (TODO: Refer to GitHub flow chapter). When the code change is complete, they create a new PR from their branch to master, and augment it with the relevant data, such as a descriptive title, description, and linked issues. By creating this process, it signals to the other developers that the code is ready for review. Furthermore, the developer also posts a link to the PR on discord to notify the rest of the team.

For a pull request to be merged it must comply with our policies.
First of all, all automatic tests and checks must complete successfully. This includes unit tests, end-to-end tests, static analysis, and more.
Secondly, the PR must be approved by at least one other developer. Github supports code reviews as a part of the pull requests, so that other team members can, approve, reject, and suggest changes to a PR.
The automatic tests help catch bugs and regressions the human reviewer would otherwise miss, and the human usually catches things the automatic tests are not configured or able to detect.
Even though it is essentially impossible to eliminate bugs completely, the combination of automatic tools, combined with human common sense, is the best way of attempting to prevent them.
If a pull request is rejected, the author can make changes and re-request reviews.
When a pull request is merged, it will automatically be deployed to our production environment (TODO: Refer to CI/CD chapter), and mark all linked issues as solved.

Discord has been used where GitHub falls short, namely for instant communication.
Despite the "gamer-branding", Discord was used instead of other popular team communication tools, such as messenger or slack, for various reasons.
Besides already using it extensively in our daily lives, Discord has many powerful features that make it incredibly well suited for distributed teamwork.
One of the biggest advantages is that a discord server supports multiple text and voice channels.
This makes it easy to categorize and organize the chat. The voice chats also allow us to enter and leave at will, making it ideal for long-term group work, where people can join and leave as they please.
Another big advantage is that discord has an easy-to-use API, with support for webhooks. This has been utilized by hooking up GitHub and our uptime-monitoring tool, OhDear, to separate channels, so that notifications are automatically posted when new updates occur. This has allowed us to act very fast on code changes and downtime.

**TODO: Describe how we used devops practices when writing this report**
