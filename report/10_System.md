## System's Perspective

<!-- Short Introduction to System goes here.
Double-check that for all the weekly tasks (those in the end of the lecture notes) you include the corresponding information. -->

When given the task of rewriting the MiniTwit application we had to choose a tech stack. 
This not only includes a programming language and framework, but also which services our application depends on, such as a database, monitoring service, and logging solution.
After discussing we settled on C#, using the ASP.NET web framework since it is well supported and maintained. 
The framework also has a large community behind it, which also means that many third-party packages are available to utilize, such as Newtonsoft JSON, which is the de-facto way of (de)serialize JSON, and Prometheus-net for interacting with Prometheus.
Another reason for choosing this tech stack was that no one in the group has worked on a full-scale ASP.NET app previously and therefore wanted to learn it. 
Since the framework is also backed by Microsoft, which makes the platform viable for businesses, it is a very lucrative skill to have when looking for jobs.

Besides the main programming language and framework, we also needed to decide how to run the application in production.
For this we have containerized all our services with docker and orchestrated them with docker-compose.
The system has been running on a single server provided by the cloud provider [DigitalOcean](https://www.digitalocean.com/) throughout the entire project.

In the following chapters, we will discuss the architecture, dependencies, and different subsystems in the system. Lastly, we will describe the current state of the application.
