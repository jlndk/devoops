# DevOops Project Report

This document is the final report of Group D (a.k.a. Group DevOops) for the course "DevOps, Software Evolution and Software Maintenance" at the IT University of Copenhagen. The report covers a project done in the period of February to May of 2020 by the following members:

- Hjalte Sorgenfrei Mac Dalland, hjda@itu.dk
- Jakob Israelsen, jais@itu.dk
- Jonas Lindenskov Nielsen, joln@itu.dk
- Tobias Løfgren, tobl@itu.dk
- Simon Green Kristensen, sigk@itu.dk

Throughout the course students were expected to learn how to maintain and evolve a legacy system.
For this purpose, a legacy twitter clone, implemented in Flask with Python 2 running on an old macOS version, was given to the group along with the task of refactoring the application.
The goal of the refactoring was for it to run on a modern operating system, Ubuntu 18.04, and not use outdated and unsupported technologies. To facilitate a smoother development and minimize lead time, DevOps practices like Continuous Integration (CI) and Continuous Deployment (CD) were to be utilized. In addition to CI/CD, extensive logging and monitoring were to be employed to aid in spotting and fixing problems.

The production environment can be found online at [www.minitwit.tk](https://minitwit.tk) until 2020-06-11. Grafana and Kibana can be found at [grafana.minitwit.tk](https://grafana.minitwit.tk) & [kibana.minitwit.tk](https://kibana.minitwit.tk).

This report consists of three parts, a description of the system in its final state when the course ended, an explanation of the development process and tools used, and finally the lessons learned while developing the system.
