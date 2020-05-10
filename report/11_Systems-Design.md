A component digram describing the flow of data through the MiniTwit system itself can be seen in the figure below.  

![Component diagram](./images/component_diagram.png)

The MiniTwit application has two main C# Projects, MiniTwit.Web.App, containing the web app, and MiniTwit.Models containing the repositories for the different types of data handled by the application. The application also has other projects containing the MiniTwitContext connecting to the Database, or containing utility functions, but these are not illustrated in the diagram above. 

The ```MinitwitContext``` (based in EntityFrameworkCore) functions as the database abstraction layer, and allows the repositories to access their relevant data. The repositories then contain the relevant methods for passing the data on in specific formats to the frontend controllers, contained in ```MiniTwit.Web.App```. The ```HomeController``` passes this data on to the views in the Views directory, where they are used when accessed in a browser, while the ```APIcontroller``` instead exposes the API that the simulator uses.

In general, the application is written in C# whenever possible, partially because the team wanted to learn the different aspects of C#, and partially because it made it easier to connect all the parts of the application. Thus, the frontend of the application is written in ASP.NET pages, the security layer is made using ASP.NET Identity, and EntityFrameworkCore is used for the database abstraction layer.

The choice of the general repository structure, was made because the team had worked with the structure in C# before, and thus would be able to implement it quickly, and have a reference point of how it should be set up.