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


## Motivação
Muitas coisas que eu vi e fiz, acabam ficando espalhadas e eu preciso sempre fica caçando isso. E como também to com o roadmap de estudos.
Achei que faria sentido a abordagem. Mas o que seria o resultado?!
Tem alguns projetos (a principio em .net, mas isso eu quero abrir pra node e outas linguagens), que vão ter alguns padrões e abordagens.
Por exemplo, quero estudar **EntityFramework**, eu vou ter um projeto aqui que vai ter essa abordagem, com **Migrations** e casos de uso, etc...
Projeto com integração com **RabbitMq**...
Projetos distríbuidos, que quero praticar conceitos de **Tracing**


E lógico... Cada projeto também vai levar documentação do seu escopo, também como prática.