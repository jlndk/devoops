### Current state of the system

While the specific tools used for static analysis will be described in later sections, there are three main measures for quality assessment in the system:

- A static analysis report from Sonarcloud.io
- A test coverage report from CodeCov
- A performance and accessibility report from Lighthouse 

According to the [Sonarcloud report](https://sonarcloud.io/dashboard?id=jlndk_devoops), the project has 0 bugs, 0 vulnerabilities, 0 debt, and 1% duplication. This if does of course not mean that the system is guaranteed to contain 0 bugs, but these metrics can be used to catch the more obvious ones that the static analysis can detect, while unit tests should be able to catch most other types of bugs. There used to be vulnerabilities and code smell, but all of it was fixed after it was highlighted by SonarCloud. There is also 0% test coverage reported, but this is due test coverage being handled by Codecov and therefore not setup in the SonarCloud pipeline.

![Codecov](images/codecov.png)<br/>
*Figure 3: Codecov report breakdown*

The [CodeCov report](https://codecov.io/gh/jlndk/devoops) states that the project overall has 31.11% line coverage. Details about which parts of the application are analyzed by CodeCov, and a breakdown of their coverage can be seen in figure 3. Note that some part of the application are not shown because they do not have any tests, one of these part is the MiniTwit.Web.App.

![Lighthouse](images/lighthouse.png)<br/>
*Figure 4: Lighthouse report*

Finally, according to the last report made by Lighthouse, which can be seen in figure 4, the application scores 100 on accessibility, 93 on best practices, 100 on performance, 48 on progressive web app, and 97 on SEO. It is worth mentioning that the 93 on best practices stems from the fact that tests are done via HTTP, while the version of the application in production uses HTTPS. 

As such, the application generally scores high on the chosen metrics, with the exception of Code Coverage which could be much better. This is likely because developers have not been forced to make tests when developing code, which coupled with the difficulty of writing tests for ASP.NET, means that large part of the program remains untested. The other metrics are great however, as the tools meant that developers can see what aspects of the application could be improved and therefore was a focus on improving them. Thus, the use of these tools as a DevOps practice has led to a better project overall. 
