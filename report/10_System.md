## System's Perspective

The web application itself is written in C#, using the ASP.NET web framework. 
This stack was chosen since it is the de-facto-standard for larger web applications (at least in Denmark), which makes it really well supported and maintained. This also means that many third-party packages is available to utilize, like Newtonsoft JSON, which is the de-facto way of serializing JSON, and Prometheus-net for interacting with Prometheus.
Furthermore no one in the group has worked on a full-scale ASP.NET app previously and therefore wanted to learn it. 

Furthermore the entire application was containerized with docker and orchestrated it with docker-compose.
A switch to a distributed docker swarm cluster was almost completed, but due to time constraints it wasn't finished so the single-machine setup was kept.

The server chosen is offered by the cloud provider [DigitalOcean](https://www.digitalocean.com/). DigitalOcean was chosen since they have a friendly and easy-to-use interface, compared to their competitors, while still providing cheap, stable and powerful products. The entire system was ran on a single "Droplet" (DigitalOceans name for a virtual server), with 2 CPU cores and 4 GB of RAM. This configuration was selected as it was a good balance between cost and compute power to serve the amount of traffic expected.

In the following chapters will discuss the architecture, dependencies and different subsystems in the system, ending in description of the system in its current state.
