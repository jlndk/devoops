### System Design 
The MiniTwit application follows the Repository design pattern, with a database abstraction layer, a backend with repositories and a frontend that receives data from these repositories. 

The application is written completely in C#, partially because the team wanted to learn the different aspects of C#, and partially because it made it easier to connect all the parts of the application. Thus, the frontend of the application is written as an ASP.NET Core application using cshtml, the security layer is made using ASP.NET Identity, and EntityFrameworkCore is used for the database abstraction layer.

The application has three main C# Projects, MiniTwit.Web.App, containing the web app, MiniTwit.Models containing the repositories for the different types of data handled by the application and MiniTwit.Entities, containing the entities of the application, and the DBContext from EntityFramework called MiniTwitContext. The application also has other projects containing utility functions, and projects for testing.

A component diagram describing the flow of data through the MiniTwit system itself can be seen in the figure below.  

![Component diagram](./images/component_diagram.png)

The `MinitwitContext`, allows the repositories to access their relevant data. The repositories then contain the relevant methods for passing the data on in specific formats to the frontend controllers, contained in `MiniTwit.Web.App`. The `HomeController` passes this data on to the views in the Views directory, where they are used when accessed in a browser, while the `APIcontroller` instead exposes the API that the simulator uses.

The choice of the general repository structure, was made because the team had worked with the structure in C# before, and thus would be able to implement it quickly, and have a reference point of how it should be set up.