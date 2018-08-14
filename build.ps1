docker-compose -f "docker-compose.yml" -f "docker-compose.override.yml" build
Start-Process -FilePath "http://localhost:80/shows?offset=0&limit=10" 
docker-compose -f "docker-compose.yml" -f "docker-compose.override.yml" up