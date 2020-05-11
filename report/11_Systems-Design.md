### System Design 
The MiniTwit application is a web app written in C# using ASP.NET. Data is stored using EntityFrameworkCore as a database abstraction layer working with a Postgres database. This stack was chosen since it is a well supported and maintained stack with plenty of available documentation. This means that many third-party packages are available, like SeriLog, which makes interaction with ElasticSearch painless, and Prometheus-net for plug and play communication with Prometheus.

The application has three main C# Projects. 
- MiniTwit.Web.App, which contains the web app which users interact with. 
- MiniTwit.Models, an abstraction layer for data transfer between the frontend and backend using the [Repository design pattern](https://martinfowler.com/eaaCatalog/repository.html).  
- MiniTwit.Entities, containing the data entities of the application, and the database context, an EntityFramework abstraction for database interaction. 

The application also has other projects containing utility functions, and projects for testing.

![Component diagram](./images/component_diagram.png)*Figure x: A component diagram describing the main components of the MiniTwit system.*

As illustrated in figure x, the `MiniTwitContext` allows the repositories to access their relevant data. The repositories then contain the relevant methods for passing the data on in specific formats to the frontend controllers, contained in `MiniTwit.Web.App`. The `HomeController` passes this data on to the views in the Views directory, where they are used when accessed in a browser, while the `ApiController` instead exposes the API that the simulator uses.

The application is written completely in C# because the team wanted to learn the different aspects of C#. 
After all, it made it easier to connect all the parts of the application, and the team had experience with it allowing work to be done faster. 
Thus, the frontend of the application is written as an ASP.NET Core application using cshtml, the security layer is made using ASP.NET Identity, and EntityFrameworkCore is used for the database abstraction layer.

The choice of the repository pattern, was made because the team had worked with the pattern in C# before. Thus, it could be implemented quickly, and the team would have a reference point of how it should be set up.
