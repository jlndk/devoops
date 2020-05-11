### Maintenance

To facilitate maintenance of the MiniTwit system multiple CI tools were utilized. These tools verify that the system was still in working order after changes to the code base. They include running unit tests, verifying API simulator still works, getting analysis from SonarCloud and metrics from Google's Lighthouse. 
The last of which, Lighthouse, showed there was an issue when merging a specific pull request to master. 
But due to this issue not getting spotted, combined with misconfiguration of which CI checks had to pass before code could be merged with master, the register page on the website ended up breaking. This was first realized when a user reported the error. 
If one were to look at the [pull request (Fix SonarCloud issues #186)](https://github.com/jlndk/devoops/pull/186) it is quite clear that an issue came up when Lighthouse tried to reach the register page, as all the check came back red with a score of 0. 

This issue, as well as similar ones, could probably have been caught in the pull request, if a more rigorous and defined review process was defined. 
A more defined review process could also have alleviated the problem of not knowing what to look for when going through a pull request, which was a frequent problem over the course of this project.
