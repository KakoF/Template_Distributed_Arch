# Docs

Vai ter abordagem de vários assuntos e anotações


Comandos para lembrar
```cmd
git rm -r --cached <folder>

docker-compose down

docker-compose up -d --build

dotnet run --launch-profile https


docker logs:
docker logs jaeger-template

ping:
curl -v http://localhost:4317

Encontrar ip do container:
docker inspect -f "{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}" jaeger-template
```