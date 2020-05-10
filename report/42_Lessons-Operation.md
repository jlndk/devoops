### Operation 

While running the MiniTwit system some issues came up which had to be resolved. 

The largest issue was that the original Droplet on which the system was hosted was located in New York. This had the problem of increasing the ping when answering the Api Simulator which was hosted in Frankfurt. To fix this problem it was decided to migrate servers to Frankfurt. But DigitalOcean does not have any simple path for migrating Droplets so the process had to be done manually. The plan for the migration can be read here: https://github.com/jlndk/devoops/issues/123

Two methods of migrating with minimal downtime was explored:
1. Replicating the database to a Droplet in Europe. Then spin up a Api endpoint a European Droplet and wait for the domain name record to be updated. Postgres comes with built in streaming replication, so this would be possible without major refactoring. 
2. Spinning up a concurrent copy of the system on a European Droplet and copy over data at that point in time. Then wait for domain name. This method had the downside of losing around half an hour of data, which in theory could have been added back in if one took the difference between the two system. 

In the end the second method was chosen as it was simpler and the data loss was decided to be acceptable as it would be unnoticed on the chart. In an actual production environment data loss would have been unacceptable and the first method would have been chosen. 