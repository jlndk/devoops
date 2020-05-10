## System's Perspective

The web application itself is written in C#, using the ASP.NET web framework. 
This stack was chosen since it is the de-facto-standard for larger web applications (at least in Denmark), which makes it really well supported and maintained. This also means that many third-party packages is available to utilize, like Newtonsoft JSON, which is the de-facto way of serializing JSON, and Prometheus-net for interacting with Prometheus.
Furthermore no one in the group has worked on a full-scale ASP.NET app previously and therefore wanted to learn it. 

When given the task of rewriting the MiniTwit application a tech stack had to be chosen. 
This not only includes a programming language and framework, but also which services the application depends on, such as a database, monitoring service, and logging solution.
To this purpose C#, using the ASP.NET web framework, was chosen as it is well supported and maintained. 
The framework also has a large community behind it, which also means that many third-party packages are available to utilize, such as Newtonsoft JSON, which is the de-facto way of (de)serialize JSON, and Prometheus-net for interacting with Prometheus.
Another reason for choosing this tech stack was that no one in the group has worked on a full-scale ASP.NET app previously and therefore wanted to learn it. 
Since the framework is also backed by Microsoft, which makes the platform viable for businesses, it is a very lucrative skill to have when looking for jobs.

Besides the main programming language and framework, the application also needed to run in production.
For this, all the services were containerized with docker and orchestrated them with docker-compose.
The system has been running on a single server provided by the cloud provider [DigitalOcean](https://www.digitalocean.com/) throughout the entire project.


In the following chapters will discuss the architecture, dependencies and different subsystems in the system, ending in description of the system in its current state.