# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY MiniTwit.Entities/*.csproj ./MiniTwit.Entities/
COPY MiniTwit.Entities.Tests/*.csproj ./MiniTwit.Entities.Tests/
COPY MiniTwit.FlagTool/*.csproj ./MiniTwit.FlagTool/
COPY MiniTwit.FlagTool.Tests/*.csproj ./MiniTwit.FlagTool.Tests/
COPY MiniTwit.Models.Test/*.csproj ./MiniTwit.Models.Test/
COPY MiniTwit.Models/*.csproj ./MiniTwit.Models/
COPY MiniTwit.Utils/*.csproj ./MiniTwit.Utils/
COPY MiniTwit.Web.App/*.csproj ./MiniTwit.Web.App/
RUN dotnet restore

# copy everything else and build app
COPY MiniTwit.Entities/. ./MiniTwit.Entities/
COPY MiniTwit.Entities.Tests/. ./MiniTwit.Entities.Tests/
COPY MiniTwit.FlagTool/. ./MiniTwit.FlagTool/
COPY MiniTwit.FlagTool.Tests/. ./MiniTwit.FlagTool.Tests/
COPY MiniTwit.Models.Test/. ./MiniTwit.Models.Test/
COPY MiniTwit.Models/. ./MiniTwit.Models/
COPY MiniTwit.Utils/. ./MiniTwit.Utils/
COPY MiniTwit.Web.App/. ./MiniTwit.Web.App/
WORKDIR /source/MiniTwit.Web.App
RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
EXPOSE 5001
EXPOSE 5000
EXPOSE 80
EXPOSE 443
ENV applicationUrl=https://localhost:5001;http://localhost:5000
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "MiniTwit.Web.App.dll"]