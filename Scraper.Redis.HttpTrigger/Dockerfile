FROM microsoft/dotnet:2.1-sdk AS installer-env

COPY . /src/dotnet-function-app
WORKDIR /src/dotnet-function-app/Scraper.Redis.HttpTrigger
RUN mkdir -p /home/site/wwwroot && \
    dotnet publish *.csproj --output /home/site/wwwroot

FROM microsoft/azure-functions-dotnet-core2.0:2.0

COPY --from=installer-env ["/home/site/wwwroot", "/home/site/wwwroot"]