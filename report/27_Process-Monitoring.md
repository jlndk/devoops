### Monitoring

It is important to have knowledge of quantities, frequencies, and speed in a system, both to improve technical bottlenecks and to communicate important metrics to project stakeholders.  
Monitoring is the process of obtaining and saving this information, along with having easy access to the data, possibly through visualizations and dashboards.

The simplest monitoring of the application happens at [Digital Ocean](https://www.digitalocean.com/), the cloud provider for the system.
Built into their dashboards are graphs over system metrics, such as uptime and both CPU, memory and network usage.
The Digital Ocean dashboards are very basic, especially for uptime moniroting, but they are also incredibly simple to set up and use.
This was, in the early stages, all the monitoring the system had.

In order to monitor more closely what each process is doing, the project utilizes [Prometheus](https://prometheus.io/docs/introduction/overview/). Prometheus is a systems monitoring toolkit, capable of gathering and storing information about processes running as well as being extended to monitor even more specific things, based on what is running on the system. The default metrics include simple low-level information, such CPU, memory and disk usage for both the system and the specific process.

In the project, Prometheus has an extension to interface with the web app through the NuGet package ([prometheus-net](https://www.nuget.org/packages/prometheus-net)).
It makes Prometheus capable of logging information relating to ASP.NET, things like HTTP request length, request frequency/volume, error rates and the like.

For visualizing the data, [Grafana](https://grafana.com/grafana/) is used.
Grafana is an analytics platform for easy visualization and alerts in relation to monitored metrics, with built-in support for Prometheus.
It is easy and fast to set up dashboards, and Grafana is also very extensible.
One plugin the project utilizes is the PostgreSQL plugin, making it easy to visualize data from the database layer via SQL queries.

The things that the team chose to visualize are:

- Response times for different segments of the application (web-app, Node Exporter, Prometheus)
- Average response time per API action
- Request volume per API action
- Amount of registered users (via SQL query)
- Amount of posted twits (via SQL Query)

![Minitwit - Grafana](images/Minitwit-Grafana.png)

These business metrics are pretty basic and many more could be included. But there simply weren't the necessary time to implement more metrics in Grafana.
By using node_exporter and an open-source dashboard it was possible to get a lot of metrics without a lot of effort, they can be viewed [here https://grafana.minitwit.tk/d/rYdddlPWk/node-exporter-full?orgId=1](https://grafana.minitwit.tk/d/rYdddlPWk/node-exporter-full?orgId=1).

![Minitwit - Grafana - node_exporter](images/node_exporter.png)
 
One of the things that cannot be monitored reliably with these two solutions is whether the system is accessible through the internet.
For this, a service called [Oh Dear](https://ohdear.app) was used. This service attempts to visit the configured application from multiple places every 2 minutes, and will send notifications through email and on discord if the site cannot be accessed. Furthermore, Oh Dear also keeps track of total downtime along with various other metrics, such as TLS certificate health and broken links.
This tool has been immensely useful for ensuring that the system was available at all times, as well as monitoring our SLA.
See appendix 1 for an attached uptime report.
