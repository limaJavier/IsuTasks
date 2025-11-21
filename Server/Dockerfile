# Stage 1: Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

# restore
COPY ["src/IsuTasks.Api/IsuTasks.Api.csproj", "IsuTasks.Api/"]
RUN dotnet restore 'IsuTasks.Api/IsuTasks.Api.csproj'

# build
COPY ["src/IsuTasks.Api", "IsuTasks.Api/"]
WORKDIR /src/IsuTasks.Api
RUN dotnet build 'IsuTasks.Api.csproj' -c Release -o /app/build

# Stage 2: Publish Stage

FROM build AS publish
RUN dotnet publish 'IsuTasks.Api.csproj' -c Release -o /app/publish

# Stage 3: Run Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0

ENV ASPNETCORE_URLS=http://+:9090
EXPOSE 9090

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "IsuTasks.Api.dll" ]
