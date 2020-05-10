### Architecture 

![Deployment](images/Deployment.png "Deployment Diagram")


The MiniTwit system is deployed on a Droplet by using Docker-Compose which starts the systems in the appropriate dependency order. In the current iteration of the system this is all done on a single server as can be seen from the above diagram. But each component could be moved to separate servers without major problems, a process which was in progress on the branch [swarm](https://github.com/jlndk/devoops/tree/swarm), but due to being short on time this was not completed in time. More on this in the Scaling section. 

#### Sub systems

Traefik is used as a reverse proxy and Edge Router. As can be seen from the diagram it interacts with MiniTwit.Web.App, Kibana and Grafana. These three are the public facing applications and Traefik is used to manage SSL certificates and HTTPS. When using Docker Swarm load balancing can be done through Traefik. 

The main application, MiniTwit.Web.App, which was described in the previous section, interacts with the Postgres database where all data is stored. 

Monitoring can be seen on the left with Grafana and Prometheus. Here Prometheus interacts with the main application and pulls data which then can be displayed in Grafana.

Logging happens through ElasticSearch which has data pushed to it from the main application through the use of the C# library SeriLog. Kibana then pulls data from ElasticSearch to display to the user.

