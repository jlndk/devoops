### <a name="41"></a>Refactoring

The first task in this project was to refactor the provided system twice; first to Python 3, then a tech stack of the team's own choice.

#### Python 2 to Python 3

The conversion from Python 2 to 3 was almost symbolic of easy wins in DevOps.
To do the conversion, a tool called [2to3](http://python3porting.com/2to3.html) was used; feed Python 2 in, get Python 3 out.

It worked almost completely right away, which was surprising as the introduction to the tool in-class said to be careful that nothing broke.
The only thing needing a fix was the tests, which were previously dependent on the Unicode handling of Python 2, but that was not hard to fix (See [This PR](https://github.com/jlndk/devoops/pull/6) for more).

#### Python 3 with SQLite to ASP.NET with PostgreSQL

The following conversion was much larger than the previous task.
It was decided to rewrite the python program to an ASP.NET Core 3 application with PostgreSQL as the DBMS, as the language was familiar to some members, while being something desirable to learn for the rest of the team.

This rewrite turned out to be quite monolithic, due to everything being changed at once.

The first part was difficult in itself, as ASP.NET provides at least 6 different ways to make a server-side rendered web page.
The template chosen was the Model-View-Controller architecture, along with using ASP.NET Razor template pages for providing the HTML templates.

The next part was to convert the Flask template pages to a format that Razor understands, which was simple enough, though tedious.
Learning the syntax of Razor and Flask at the same time provided somewhat of a challenge, and porting to an MVC architecture at the same time didn't make this transition easier.

The arguably biggest part was the implementation of Entity Framework (EF) and porting to PostgreSQL.
For each part of the model, an entity would have to be established in code, and a context and repository layer for accessing and changing that entity would have to be written.
Then, on top of that, an API layer was necessary to access and mutate this state.
On the bright side, this completely eliminated plain text SQL queries in the code itself.

If such a large rewrite was to be done some other time, then a more incremental approach might be better.
If the database was first changed to rely on another DBMS, and _then_ changed to work with EF, then the refactor could become more incremental and thus less monolithic.
The only problem here is that it would equate to doing double the work, and when a rewrite has to be done in a small timeframe, then double work doesn't seem practical.

The positives of having done all this work upfront were that there were several tasks that were handed out later which did not need to be implemented, as it had already been taken care of here.

#### Post refactor verification

To ensure that the rewrite did not break anything, tests could be useful.
The only problem is that the previous tests were an API test script along with Python unit tests.

The immediate action was to create a Github Action to automate API tests.
This turned out to not be enough for validating the rewrite, as real messages were not being simulated.
The action was then changed to run the API simulator instead.

Implementation of a proper test suite came later down the line, with unit tests for the `dotnet` runtime, as well as several end-to-end tests.
Ideally, we would also have written integration tests, to test the controller and how the subcomponents of the system interacted, but was prevented from this due to time constraints.
There was always some awareness of the fact that there should be more tests, and reminders came up when things broke in production (like when the register page broke completely).
Though it is a lesson already learned in previous projects, it was once again learned here.

### Evolution

Once the necessary refactoring steps were done, expansion of the implementation could proceed.

#### Automation and its benefits

Early on in the development process, not many automatic steps were set up in the project pipeline.
This made evolution much harder than it had to be, as tests would have to be manually run all the time to verify builds, and releases would have to be done manually.

In the pipeline, there was a focus on having many actions done automatically, as mentioned in the section CI/CD Description.
This was, at first, because the course demanded it, but once automatic steps saw usage, the value was apparent.
The pipeline was expanded repeatedly, with the steps themselves even being expanded, as is the case with `e2e.yml`.

The takeaway here is that automation is very useful, as these manual, menial tasks could now be carried out automatically.
It is therefore now a focus moving forward to set up robust automation steps in future projects as soon as possible, as it would remove the need for doing menial tasks.

#### Choose scalable technologies early

It was already known that choosing scalable technologies early was important.
If a choice to switch from a monolithic server to a distributed system comes too late, then the changes that will have to be made could become quite big, resulting in a lot of technical debt.

This was why dockerization came early for the project, with separation between components for easier clusterization.
Because the considerations were done early, Docker Swarm would be easier to set up, instead of being buried in debt from not having a RESTful API, for example.
the Docker Swarm refactor unfortunately failed, though the work put into trying can be found on the `swarm` branch.

#### Graphs are motivational

After the simulator had been live for a few weeks, the statistics showed that this project lacked behind the other participating projects by quite a big margin.
Having this as a motivator to improve was vital to finding some of the bugs in the system.

First, it was discovered that the project lacked behind because response times were highly influential for the graph trend lines.
It was then considered whether HTTPS and the Traefik reverse-proxy were the things slowing down the simulator, but removing those things only made very slight improvements, so they were re-enabled.
The actual reason turned out to be the server location, as will be described in the following section Operation.

Without the graph, statistics, and the competitive nature of the participating programmers as motivators, these things would never have been discovered.
Replication of this feeling could be done by collecting metrics from competitors, and gamifying these.
Gamification could lead teams to improve metrics, for example through having a big screen with several desired trend lines/thresholds along with actual statistics.
The more prominent the gamification, the more this competitive nature could drive participants to do better.

#### Proper communication

A problem that never got improved enough was communication, especially of the asynchronous kind.
When co-located, the team managed to get lots of work done and reviewed.
At the times between co-located sessions and after the COVID-19 pandemic hit the world, the reviews were slow and impractical.

A possible reason could be that reminding people to review is easier and more direct when working next to each other.
Another could be that the other courses and work got in the way.

A third possibility could be that there were no channels for politely nudging towards a review.
An attempt to alleviate this was done with the `#review` channel on Discord, where anyone could push for a review.
This helped in some ways, as Discord is a more prominently used communication tool among the developers.

This theme is expanded upon a bit more in the section Maintenance.

This doesn't mean that all communication was bad, far from it.
Discord and GitHub were great places for collecting information, the problems were mainly with assigning reviewers and delegating work.
