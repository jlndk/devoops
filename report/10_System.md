## System's Perspective

<!-- Short Introduction to System goes here.
Double-check that for all the weekly tasks (those in the end of the lecture notes) you include the corresponding information. -->

Our web application itself is written in C#, using the ASP.NET web framework. 
We have chosen this stack since it is the de-facto-standard for larger web applications (at least in Denmark), which makes it really well supported and maintained. This also means that many third-party packages is available to utilize, like Newtonsoft JSON, which is the de-facto way of (de)serialize JSON, and Prometheus-net for interacting with Prometheus.
Furthermore none of us in the group has worked on a full-scale ASP.NET app previously and therefore wanted to learn it. 

Furthermore we has containerized our entire application with docker and orchestrated it with docker-compose.
We almost has almost been able to switch over to a distributed docker swarm cluster, but due to time constraints we didn't finish it so we stuck with our single-machine setup.

The server we have chosen is offered by the cloud provider [DigitalOcean](https://www.digitalocean.com/). We have chosen DigitalOcean since they have a friendly and easy-to-use interface, compared to their competitors, while still providing cheap, stable and powerful products. To be more precise, we run our entire system on a single "droplet" (DigitalOceans name for a virtual server), with 2 CPU cores and 4 GB of RAM. We figured this was a good balance between cost and compute power to serve the amount of traffic we expected to get.

In the following chapters we will discuss the architecture, dependencies and different subsystems in our system. Lastly we will describe the current state of our application.
