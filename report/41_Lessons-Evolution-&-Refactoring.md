### Refactoring

The first task in this project was to refactor the provided system twice; first to Python 3, then a tech stack of the team's own choice.

#### Python 2 to Python 3

The conversion from Python 2 to 3 was almost symbolic for easy wins in DevOps.
To do the conversion, a tool called [2to3](http://python3porting.com/2to3.html) was used; feed Python 2 in, get Python 3 out.

It worked almost completely right away, which was surprising as the introduction to the tool in class said to be careful that nothing broke.
Only thing needing a fix was the tests, which were previously dependent on the more raw string handling of Python 2, but that was not terribly hard to fix (See [This PR](https://github.com/jlndk/devoops/pull/6) for more).

#### Python 3 with SQLite to ASP.NET with PostgreSQL

The following conversion was much larger than the previous.
The team decided to refactor the web server to an ASP.NET Core 3 application with PostgreSQL as the DBMS, as the language was familiar to some members, while being something stable to learn for the rest of the team.

This refactor turned out to be quite monolithic, in part due to everything being changed at once.

First part was difficult in and of itself, as ASP.NET provides at least 6 different ways to make a server-side rendered web page.
The team settled on using a template built with Model-View-Controller architecture in mind, along with using ASP.NET Razor template pages for providing the HTML templates.

Next part was to convert the Flask template pages to a format that Razor understands, which was simple enough, though tedious.
Learning the syntax of Razor and Flask at the same time provided somewhat of a challenge, and porting to a MVC architecture at the same time didn't make this transition easier.

The arguably biggest part was the implementation of Entity Framework (EF) and porting to PostgreSQL.
For each part of the model, an entity would have to be established in code, and a context and repository layer for accessing and changing that entity would have to be written.
Then, on top of that, an API layer was necessary to access and mutate this state.
On the bright side, this completely eliminated plain text SQL queries in the code itself.

If such a large refactor was to be done some other time, then a more incremental approach might be better.
If the database was first changed to rely on another DBMS, and _then_ changed to work with EF, then the refactor could become more incremental and thus less monolithic.
The only problem here is that it would equate to doing double the work, and when a refactor has to be done in a small timeframe, then double work doesn't seem practical.

#### Make sure it all works

In order to ensure that the refactor didn't break anything, tests could be useful.
Only problem is that the previous tests were an API test script along with Python unit tests.

In order to see immediate results, a Github Action was created to automate running the API test.
That didn't really work, as it didn't catch all edge cases, so instead the project got configured to be tested with the API simulator.

Implementation of a proper test suite came later down the line, with unit tests for the dotnet runtime, as well as several end to end tests.
The team was always aware of the fact that there should be more tests, and were even reminded when things broke in production (like when the register page broke completely).
Though it is a lesson that the team has already learned in previous projects, here the team learned it once again.

### Evolution

Once the necessary refactoring steps were done, the team was capable of expanding the original implementation.

#### Automation and its benefits

Early on in the development process, not many automatic steps were set up in the project pipeline.
This made evolution much harder than it had to be, as tests would have to be manually run all the time to verify builds, and releases would have to be done manually.

In the pipeline, the team has focused on having many actions done automatically, as mentioned in [TODO: 2.3: CI/CD](23_Process-CI-CD-Description.md).
This was, at first, because the course demanded it, but once the team started using automatic steps, then the value was apparent.
The pipeline was expanded repeatedly, with the steps themselves even being expanded, as is the case with `e2e.yml`.

The takeaway here is that automation is very useful, as these manual, menial tasks could now be carried out automatically.
A focus for the team members moving forward is to set up robust automation steps in future projects as soon as possible.

#### Choose scalable technologies early

The team knew that choosing scalable techonologies early was important.
If a choice to switch from a monolithic server to a distributed system comes too late, then the changes that will have to be made could become quite big, resulting in a lot of technical debt.

This was why dockerization came early for the project, with separation between components for easier clusterization.
The team didn't quite get Docker Swarm to work, but because these considerations were done early, Docker Swarm was actually obtainable, instead of being buried in debt from not having a RESTful API, for example.

#### Graphs are motivational

After the simulator had been live for a few weeks, the statistics showed that this project lacked behind the other participating projects by quite a big margin.
Having this as a motivator to improve was vital to finding some of the bugs in the system.

First, it was discovered that the project lacked behind because response times were highly influential for the graph trend lines.
It was then considered whether HTTPS and Traefik routing were the thing slowing down the simulator, but removing those things only made very slight improvements, so they were re-enabled.
The actual reason turned out to be the server location, as will be described in [TODO: 4.2 Operations](42_Lessons-Operation.md).

Without the graph, the statistics, and the competitive nature of the team as motivators, these things would never have been discovered.
Replication of this feeling would be difficult, as there rarely are 10 teams competing to implement the best and fastest project when looking at the job market.
Gamification of collected metrics might be able to lead teams to improve these things, for example through having a big screen with several desired trend lines/thresholds along with actual statistics.
The more prominent the gamification, the more this competitive nature could drive the team to do better.

#### Proper communication

A problem that the team never got to improve on was communication, especially of the asynchronous kind.
When co-located, the team managed to get lots of work done and reviewed.
At the times between co-located sessions and after the pandemic hit the world, the reviews were slow and impractical.

A possible reason could be that reminding people to review is easier and more direct when working next to each other.
Another could be that the other courses and work got in the way.

A third possibility could be that there were no channels for politely nudging towards a review.
The team tried to alleviate this with the `#review` channel on Discord, where anyone could push for a review.
This helped in some ways, as Discord is a more prominently used communication tool among the developers.

Though the team hasn't found better solutions than just being more demanding towards each other, it acknowledges that this is a problem that needs improving somehow down the line.

This doesn't mean that all communication was bad, far from it.
Discord and GitHub were great places for collecting information, the problems were mainly with the project management idea of assigning reviewers and such.
