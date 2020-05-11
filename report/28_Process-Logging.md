### Logging

Logging output from processes is important to know what is going on in the system.
If a program outputs no diagnostics information, it is hard to diagnose faults or detect attacks, which can make fixing those things harder.
Since the entire application is written in .NET, it makes sense to rely on already existing logging modules to assemble and aggregate system information.
One such module, called [Serilog](https://github.com/serilog/serilog), is what this project uses.

Serilog is a powerful low-overhead diagnostic logging library, capable of logging information in familiar log levels to many different log formats and log aggregators.
Using it is simple, as setup simply involves initializing the logger singleton with write locations (such as console, file, or Elasticsearch), after which the project, through dependency injection, is capable of referring to the same logger when needed.
Logging in code becomes as easy as calling a method through the injected interface.

All API calls are logged, some examples of which are:

- calls to `/latest`.
- Getting messages by all/specific user(s), along with a count of returned messages.
- Posting twits, both for the successful and failure paths.
- Registering a user, including cases with mail collisions and the like.
- Getting messages for follows, including count (again).
- Any of the calls made when unauthorized.

This logging is done through the standard .NET logging interface, `ILogger`, with a default logging severity level, `information`.
Having this level of severity is correct for most of the information logged, as it is just users using the service correctly. But unauthorized calls should be logged with a severity level of warning as it could be someone trying to misuse the API. With the current severity level, it would be much harder to know when something is going wrong.

Logs alone can be daunting to search through and store.
Search engines for looking through logs are viable for being able to extract information like quantity, similarity, and frequency of events, and for this [Elasticsearch](https://aws.amazon.com/elasticsearch-service/the-elk-stack/what-is-elasticsearch/) is used together with [Kibana](https://www.elastic.co/kibana).
Elasticsearch is one of the leading open-source text search engines, common for use in data analytics.
It has a RESTful JSON API (and a Java interface) and is capable of being deployed distributed, making it ideal for most logging situations.
Kibana is a data visualizer developed alongside Elasticsearch, making it ideal for visualization of the system logs.

No visualizations were setup from the logs. Nor was the logs used in any other meaningful way.
It was made to work for illustration purposes, but the logs were not used any further, due in part to unfamiliarity with the Kibana interface.

Another possible problem is that even though Elasticsearch is for searching through text, without context for numbers and information, the data is just lost without context.
Serilog has a specific syntax for saving associated data to log messages that are not used in the project, where instead the simpler ASP.NET logging extension syntax is used.
This may be another cause of lack of tinkering with Kibana.
