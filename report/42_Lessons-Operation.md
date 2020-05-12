### Operation 

While running the MiniTwit system some issues came up which had to be resolved.

The largest issue was that the original Droplet on which the system was hosted was located in New York. This had the problem of increasing the response time when answering the API simulator which was hosted in Frankfurt. To fix this problem it was decided to migrate servers to Frankfurt. DigitalOcean does however not have any simple path for migrating Droplets so the process had to be done manually. The plan for the migration can be read here: [https://github.com/jlndk/devoops/issues/123](https://github.com/jlndk/devoops/issues/123).

Two methods of migrating with minimal downtime were explored:

1. Replicating the database to a Droplet in Europe. Then spin up an API endpoint on the European Droplet and wait for the domain name record to be updated. Postgres comes with built-in streaming replication, so this would be possible without major refactoring.
2. Spinning up a concurrent copy of the system on an European Droplet and copy over data at that point in time. Then wait for the domain name to point to the European droplet. This method had the downside of losing around half an hour of data, which in theory could have been added back in after the fact if the team analysed the differences in data between the two systems.

In the end, the second method was chosen as it was simpler and the data loss was decided to be acceptable because of the constraints of this project. In an actual production environment, data loss would have been unacceptable and the first method would have been chosen.
