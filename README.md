# Criar um repositório para estudo centralizado

## Infraestrutura
O projeto vai contar com um arquivo composer para gerenciar toda infrastrutura, que depende do estudos de caso, contato até o momento com:

```
Prometheus
Grafana
Elasticsearch
Kibana
Rabittmq
Mongo
Mongo-Express
SqlServer
Postgres
```
Como tem muita infra envolvida, avalie bem se realmente no momento você precisará colocar todos os containers de pé ao mesmo tempo. 
Na pasta **ComposerTemplate**, executar:

```cmd
docker compose up -d 
```