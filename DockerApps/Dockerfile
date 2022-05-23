FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV SimpleProperty="Hello-From-Base-Dockerfile"

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DockerApps/DockerApps.csproj", "DockerApps/"]
RUN dotnet restore "DockerApps/DockerApps.csproj"
COPY . .
WORKDIR "/src/DockerApps"
RUN dotnet build "DockerApps.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DockerApps.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV SimpleProperty="Hello-From-Final-Dockerfile"
ENTRYPOINT ["dotnet", "DockerApps.dll"]