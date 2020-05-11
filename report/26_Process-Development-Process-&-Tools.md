### Applied development process and tools supporting it

Throughout this course, two tools, Github and Discord, has been used to manage the project and communication.
Two tools have been used, to differentiate between synchronous and asynchronous communication.
That is, Discord is used to communicate directly and informally to allow for fast and efficient communication,
whereas GitHub is used to organize our work in a way that can easily be searched through and understood at a later date.

GitHub has been the ideal tool for the majority of our project management, because both the code, git commits, and corresponding issues coexist and can easily be cross-referenced.
Furthermore, GitHub has the concept of "Pull requests" (PR), which introduces a way to do formal review policy for new code, before merging it with the master branch.
These pull requests can also be linked with the aforementioned issues, which makes it easy to see which code-changes fixes a given issue.

The Github workflow starts by creating the issues. The actual content of the issues is often agreed upon verbally or dictated by the homework for the course. The issues are then augmented with relevant metadata, such as description, tags, milestones, assignees, and comments. Most importantly, we use a tag for all mandatory work, so that we know what needs to be completed.
Occasionally, when many tasks need to be solved in a certain timeframe, such as when writing this report, we make a "milestone" in GitHub.
In this, we can attach all relevant issues, set a due date and afterward track the combined progress for all tasks.
When a developer is ready to solve the issue, they first assign themself if they were not assigned when the issue was created. Afterward, they make a branch and make the changes to the code, as described in (TODO: Refer to GitHub flow chapter). When the code change is complete, they create a new PR from their branch to master, and augment it with the relevant data, such as a descriptive title, description, and linked issues. By creating this process, it signals to the other developers that the code is ready for review. Furthermore, the developer also posts a link to the PR on discord to notify the rest of the team.

For a pull request to be merged it must comply with our policies.
First of all, all automatic tests and checks must complete successfully. This includes unit tests, end-to-end tests, static analysis, and more.
Secondly, the PR must be approved by at least one other developer. Github supports code reviews as a part of the pull requests, so that other team members can, approve, reject, and suggest changes to a PR.
The automatic tests help catch bugs and regressions the human reviewer would otherwise miss, and visa versa.
Even though we cannot eliminate bugs completely, but the combination of automatic tools, combined with human common sense, is the best way of attempting to prevent them.
If a pull request is rejected, the author can make changes and re-request reviews.
When a pull request is merged, it will automatically be deployed to our production environment (TODO: Refer to CI/CD chapter), and mark all linked issues as solved.
<!-- 
**TODO GitHub**:
* Issues
* PR's
* Reviews
* Labels
* Milestones 
-->

Discord has been used for the
**TODO Discord**

* Channels
* Voice chats
* Bots

**TODO: Descripe how we where devops when writing report**

<!-- For example, how did you use issues, Kanban boards, etc. to organize open tasks -->
