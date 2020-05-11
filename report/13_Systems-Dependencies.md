### System Dependencies 

The dependencies of the systems will be split up into each subsystem or tool.
Dependencies of dependencies will be left out for brevity, as it would the majority of the rapport otherwise.

**General**
- Docker
- Docker-Compose
- Grafana
- node_exporter
- Prometheus
- ElasticSearch@6.2.4
- Kibana@6.2.4
- Ubuntu@18.04
- Traefik@v2.1
- Postgres@12

**MiniTwit C# Application**
- DotNet Core@3.1.1
- Microsoft.AspNetCore.Identity.EntityFrameworkCore@3.1.1
- Microsoft.Data.Sqlite@3.1.1
- Microsoft.EntityFrameworkCore.Sqlite@3.1.1
- Microsoft.EntityFrameworkCore.InMemory@3.1.1
- Microsoft.EntityFrameworkCore.Sqlite.Core@3.1.1
- Microsoft.EntityFrameworkCore.SqlServer@3.1.1
- Microsoft.EntityFrameworkCore@3.1.1
- Microsoft.VisualStudio.Web.CodeGeneration.Design@3.1.1
- Newtonsoft.Json@11.0.2
- Newtonsoft.Json@9.0.1
- prometheus-net@3.5.0
- prometheus-net.AspNetCore@3.5.0
- Serilog@2.9.0
- Serilog.AspNetCore@3.2.0
- Serilog.Exceptions@5.4.0
- Serilog.Exceptions.EntityFrameworkCore@5.4.0
- Serilog.Extensions.Hosting@3.0.0
- Serilog.Extensions.Logging@3.0.1
- Serilog.Sinks.ElasticSearch@8.0.1
- Microsoft.NET.Test.Sdk@16.2.0
- Moq@4.13.1
- xunit@2.4.0
- xunit.runner.visualstudio@2.4.0
- coverlet.msbuild@2.8.0
- System.Data.SQLite@1.0.112
- Npgsql.EntityFrameworkCore.PostgreSQL@3.1.1.1

**CI Tools**
- Pandoc
- LiveTex
- fifsky/ssh-action@master
- minitwit_simulator.py 
- foo-software/lighthouse-check-action@master
- actions/upload-artifact@master
- SonarCloud
- sonarscanner
- zip