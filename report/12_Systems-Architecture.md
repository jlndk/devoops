### Architecture 

![Deployment](images/Deployment.png "Deployment Diagram")<br/>
*Figure 2: Deployment diagram of the MiniTwit system.*

The MiniTwit system is deployed on a Droplet, a server hosted by the cloud provider [DigitalOcean](https://www.digitalocean.com/).
The different services are containerized with docker and orchestrated with docker-compose, which also handles many aspects of the deployment such as, startup order, replacing old containers, restarting dead containers, and so on. In the current iteration of the system, this is all done on a single server as can be seen in figure 2. Each component could be moved to separate servers without major problems, a process that was in progress on the branch [swarm](https://github.com/jlndk/devoops/tree/swarm) but ultimately was not finished in time. More on this in the Scaling section.

#### Sub systems

[Traefik](https://containo.us/traefik/) is used as a reverse proxy and edge router. As shown in figure 2, Traefik interacts with the three public-facing parts of the system, namely MiniTwit.Web.App, Kibana, and Grafana. Traefik also provides both SSL certificate management, using [Let's Encrypt](https://letsencrypt.org/), HTTPS termination, as well as load balancing when more than one replica of a given container is available.

The main application, MiniTwit.Web.App, described in the previous section, interacts with the PostgreSQL database where all data is stored.

Monitoring can be seen on the left in figure 2 with Grafana and Prometheus. Here Prometheus interacts with the main application and pulls data which then is displayed in Grafana.

Logging happens through ElasticSearch which has data pushed to it from the main application through the use of the C# library SeriLog. Kibana then pulls data from ElasticSearch to make visualizations and statistics.
