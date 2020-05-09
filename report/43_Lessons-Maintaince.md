## Maintenance
To facilitate maintenance of our the MiniTwit system without we have multiple CI tools which checked that the system was still in working order after changes. These were the Unit Test, API simulator, SonarCloud and Google's Lighthouse. 
The last of which, Lighthouse, inadvertently showed us we had a issue when merging code to master. 
But due to the issue not getting spotted and misconfiguration of which CI checks had to pass before code could be merged, we ended up breaking the register page on our website. This was first realized when a user reported the error to us. 
If one were to look at the [pull request (Fix SonarCloud issues #186)](https://github.com/jlndk/devoops/pull/186) it is quite clear that a issue came up when Lighthouse tried to reach the register page, as all the check came back red with a score of 0. 


There were similar issues which our CI tools caught but that were missed. These issues that got through to Master and got deployed could probably have been caught in the pull request, if a more rigorous and defined review process was defined. 
A more defined review process could have alleviated the problem of not knowing what to look for when going through a pull request, which was a frequent problem we had over the course of this project.