### DevOps Style

Following DevOps practices had a significant impact on how implementations were handled in this project. When doing a project just from a developer's perspective, it is much easier to not think about ease of operation and maintenance, but by doing it with a DevOps perspective it is clear how large part it is in a production system.

It makes the value in reproducibility very apparent, as you never know when you come back to a system that has not been touched for ten years. Having everything centralized to a single location, like a git repo, and configuration and dependencies explicitly defined in code, makes it easy to spin up a new server many years later.
By having unit and system tests it also possible to verify that the system is in working order. Both after many years or just after changes are made.

Having a focus on automation whenever possible decreases the overhead for developers. They don't have to worry about verifying to as large of a degree and deployment is a non-issue. This decreases lead-time for fixing issues and implementing features, and therefore increases customer value. It should also increase the effectiveness of developers as they can focus more on their job and worry less.
