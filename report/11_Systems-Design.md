### System Design 
Our version of the MiniTwit application is a web app written in C# using ASP.NET. Data is stored using Entity Framework Core (EF Core) as a database abstraction layer working with a PostgreSQL database. This stack was chosen because it has both excellent support, maintenance and documentation. Furthermore, this means that many third-party packages are available, like SeriLog which makes interaction with ElasticSearch painless and Prometheus-net for plug and play communication with Prometheus.

The application has three main C# Projects. 
- MiniTwit.Web.App, which contains the web app that users interact with. 
- MiniTwit.Models, an abstraction layer for data transfer between the frontend and backend using the [Repository design pattern](https://martinfowler.com/eaaCatalog/repository.html).  
- MiniTwit.Entities, containing the data entities of the application, and the database context, an EntityFramework abstraction for database interaction. 

The application also has other projects containing utility functions, as well as projects for testing.

![Component diagram](./images/component_diagram.png)*Figure x: A component diagram describing the main components of the MiniTwit system.*

As illustrated in figure x (TODO: Update), the `MiniTwitContext` allows the repositories to access the data relevant to them. The repositories then contain the appropriate methods for passing the data on in specific formats to the frontend controllers, contained in `MiniTwit.Web.App`. The `HomeController` passes this data on to the views in the Views directory, where they are used when accessed in a browser, while the `ApiController` instead exposes the API that the simulator uses.

The application is written completely in C# because the team wanted to learn more about the different aspects of using .NET. 
After all, it made it easier to connect all the parts of the application, and the team had experience with it, allowing work to be done faster. 
Thus, the frontend of the application is written as an ASP.NET Core application using cshtml, the security layer is made using ASP.NET Identity, and EF Core is used for the database abstraction layer.

The choice of using the repository pattern, was made because of the team's prior experience with it, allowing for a quick implementation and a common understanding of how it worked.
