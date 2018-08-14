# TvMazeScraper

This app works getting the shows from Redis Cache as L1 storage. If the required shows are not in cache it will try to get from TvMazeAPI endpoint and then caching the result for the next requests.

# How to run the app:
## On local docker:
  Open powershell and run '.\build.ps1'
  
## Without docker:
  You need to run both the front-end project (BFF) and the http client back-end (Scraper.Redis.HttpTrigger) in order to get the results. The instance of Redis used will be the one I've spinned on my Azure.
  To open two projects at same time right-click the Solution root on Visual Studio -> Select properties -> On Startup Project tab select Multiple startup projects -> From the dropdowns select 'Start' for both BFF and Scraper.Redis.HttpTrigger projects.
  Start the project under 'Debug' tab in Visual Studio.
   
  
# TODO:
- Deploy to Azure using full features of Azure Functions.
