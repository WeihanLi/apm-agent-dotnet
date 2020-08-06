FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY NuGet.Config ./
COPY ["CMS.API.Campaign.WebApi/CMS.API.Campaign.WebApi.csproj", "CMS.API.Campaign.WebApi/"]
RUN dotnet restore "CMS.API.Campaign.WebApi/CMS.API.Campaign.WebApi.csproj"
COPY . .
WORKDIR "/src/CMS.API.Campaign.WebApi"
RUN dotnet build "CMS.API.Campaign.WebApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "CMS.API.Campaign.WebApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CMS.API.Campaign.WebApi.dll"]