# API com EntityFramewor

## Proposta do Projeto

Simples implementação do EntityFramework

## Infraestrutura
Ja existe um composer com a infra, e nesse projeto tem 2 dependencias desse arquivo:

```
Elasticsearch
Kibana
Prometheus
Grafana
SqlServer
Jaeger
```
Como tem muita infra envolvida, avalie bem se realmente no momento você precisará colocar todos os containers de pé ao mesmo tempo. 
Na pasta **ComposerTemplate**, executar:

```cmd
docker compose up -d 
```

## SQLServer
Banco relacional, esse projeto aponta para apenas uma tabela **User** no Database **PerformRetriveData**
Tem um script na raiz do projeto, dentro de docs, que gera toda a estrutura...

Seguimos com a abordagem database-first

## Elasticsearch

Rota do container: http://localhost:5601/app/home
DataView Gerado: logs-api-errorhandler-{env}-{yyyy-mm}

O Elasticsearch é um mecanismo de busca e análise distribuído, baseado na tecnologia Lucene, que permite armazenar, pesquisar e analisar grandes volumes de dados de maneira rápida e em tempo real. Aqui estão alguns pontos-chave sobre o Elasticsearch:

* **Armazenamento de Dados**: Ele armazena dados em documentos JSON, permitindo que você consulte e manipule informações de maneira flexível.

* **Busca Rápida**: Elasticsearch é projetado para consultas rápidas e eficientes, permitindo busca textual e filtragem complexa em grandes conjuntos de dados.

* **Distribuição**: O Elasticsearch é distribuído por natureza, permitindo que ele escale horizontalmente, ou seja, você pode adicionar mais nós (servidores) para lidar com maiores volumes de dados e consultas.

* **Análise de Dados**: Ele suporta a análise em tempo real, permitindo que você obtenha insights dos dados enquanto eles são indexados.

* **API RESTful**: O Elasticsearch oferece uma API RESTful fácil de usar, o que facilita a integração com diferentes aplicações e linguagens de programação.

* **Integração com o ELK Stack**: Frequentemente é usado como parte do ELK Stack (Elasticsearch, Logstash e Kibana), que é uma solução popular para análise e visualização de logs e dados.

Em resumo, o Elasticsearch é uma poderosa ferramenta para busca e análise de dados em larga escala, amplamente utilizada em diversas aplicações, desde busca em sites até monitoramento de logs e análises de dados em tempo real.

## Kibana

Rota do container: http://localhost:9200/

Kibana é uma plataforma de visualização de dados que faz parte do ELK Stack (Elasticsearch, Logstash e Kibana). Ele permite que os usuários explorem, visualizem e interajam com dados armazenados no Elasticsearch de maneira intuitiva. Aqui estão alguns pontos principais sobre o Kibana:

* **Interface Gráfica**: Oferece uma interface gráfica amigável para facilitar a criação de dashboards e visualizações a partir dos dados do Elasticsearch.

* **Visualizações Interativas**: Permite a criação de gráficos, tabelas, mapas e outros tipos de visualizações interativas, ajudando na análise e compreensão dos dados.

* **Dashboards Personalizáveis**: Os usuários podem criar dashboards personalizados para monitorar métricas específicas e visualizar informações relevantes de forma consolidada.

* **Análise em Tempo Real**: Kibana permite visualizar dados em tempo real, tornando-o útil para monitoramento de logs, métricas de desempenho e outras informações críticas.

* **Facilidade de Busca**: Os usuários podem realizar buscas e filtragens nos dados diretamente da interface, aproveitando a capacidade de busca poderosa do Elasticsearch.

* **Alertas e Notificações**: Complementa suas funcionalidades com a capacidade de configurar alertas e notificações baseados em condições específicas dos dados.

Em resumo, o Kibana é uma ferramenta essencial para a visualização e análise de dados no Elasticsearch, tornando a exploração de grandes volumes de dados mais acessível e interativa.

### Exemplo de busca no devTools

```json
GET api-errorhandler-development-*/_search
{
  "query": {
    "match_all": {}
  }
}
```

## Jaeger

Rota do container: **http://localhost:16686/**

Nome do Service: **API-EntityFramework**

Jaeger é uma ferramenta de rastreamento distribuído (distributed tracing) desenvolvida inicialmente pela Uber Technologies. Ele permite monitorar e analisar transações complexas em ambientes distribuídos, ajudando a entender como diferentes serviços interagem entre si.

* Agent:

Um daemon que escuta as traces enviadas pelos serviços. Ele coleta as informações e as encaminha para o coletor.

* Collector:

Recebe as traces do agent e as processa. Em seguida, armazena os dados no backend (por exemplo, Elasticsearch).

* Query:

Uma interface de usuário web que permite visualizar e explorar as traces armazenadas.

* Storage:

O backend onde os dados de rastreamento são armazenados. Pode ser Elasticsearch, Cassandra ou outro.

### Principais Funcionalidades

* Distributed Context Propagation:

Permite que o contexto de uma transação seja propagado por diversos serviços, facilitando a rastreabilidade de ponta a ponta.

* Performance e Latência:

Ajuda a identificar gargalos de performance e latência em sistemas distribuídos.

* Análise de Dependências:

Visualiza como diferentes serviços interagem e dependem uns dos outros.

Como Funciona na Prática
Quando uma requisição entra em um sistema, ela gera uma "span" (uma unidade de trabalho), que é registrada e propagada através dos serviços que a processam. Cada serviço adiciona informações à "span", criando um "trace" completo que pode ser visualizado no Jaeger para entender todo o ciclo de vida da transação.


### Logs

O projeto também usa a lib **Serilog**

Pela configuração no appsettings:
```js
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning", // Altera o padrão para Warning (ignora informações automáticas)
      "Microsoft": "Warning", // Ignora informações do sistema
      "System": "Warning", // Ignora informações do .NET Core
      "API_EntityFramework": "Information" // Define Information para sua aplicação
    }
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning",
        "API_EntityFramework": "Information"
      }
    }
  },
  "ElasticConfiguration": {
    "Uri": "http://localhost:9200"
  },
  "AllowedHosts": "*"
}
```
Onde registramos apenas Warnings do **Default, Microsoft e System**. E para Logs de info que escolhermos lançar, seria utilizada pelo **API_EntityFramework** (lembrando que isso é o namespace do projeto, se for fazer o mesmo em outro, precisa colocar o mesmo namespace)



### Trace

O projeto também tem uma abordagem de trace da requisições também utilizado como middleware **RequestTracingMiddleware**


```c#
public async Task InvokeAsync(HttpContext context)
{
	// Tenta pegar o Tracing ID do header ou gera um novo
	var tracingId = context.Request.Headers["X-Tracing-ID"].FirstOrDefault() ?? Guid.NewGuid().ToString();

	// Adiciona o Tracing ID ao contexto de logging
	using (LogContext.PushProperty("TracingId", tracingId))
	{
		// Adiciona o Tracing ID aos headers HTTP para serviços downstream
		context.Response.Headers.Add("X-Tracing-ID", tracingId);

		// Chama o próximo middleware na pipeline
		await _next(context);
	}

}
```
Na entrada de requisição ele seta esse valor X-Tracing-ID no header e após TracingId no Log. Porque?!

Imagina que essa requisição integra com outros clients (que devem ter a mesma implementação), então se já tiver um TracingId setado no request que integramos, ele vai persistir o valor
```c#
var tracingId = context.Request.Headers["X-Tracing-ID"].FirstOrDefault() ?? Guid.NewGuid().ToString();
```

E isso você vai conseguir acompanhar nos logs por esse identificador, conseguindo fazer um trace (rastreio) entre serviços:
```
Oct 8, 2024 @ 21:35:32.234      Info      Entrou sem problemas       Client1     23db24e1-246b-49be-97cf-47258797cac2

Oct 9, 2024 @ 21:35:32.234      Erro          Exception              Client2     23db24e1-246b-49be-97cf-47258797cac2
```

Olhando esse exemplo acima, vemos que o **Client1** teve uma info e chamou o client2, que teve um erro. Então pelo TracingId (23db24e1-246b-49be-97cf-47258797cac2) sabemos que essa requisição fazem parte do mesmo fluxo de execução em serviços diferentes


### Prometheus - Grafana

No projeto foi adicionado os healths da própria aplicação e do elastic:
```c#
builder.Services.AddHealthChecks()
.AddCheck("self", () => HealthCheckResult.Healthy())
.AddUrlGroup(new Uri(builder.Configuration["Jaeger:HealthCheck"]!), timeout: TimeSpan.FromSeconds(2), name: "jaeger", failureStatus: HealthStatus.Unhealthy)
.AddDbContextCheck<AppDataContext>();
```

E registrado pelo middleware, no mesmo endpoint /metrics
```c#
//Preciso setar essa conf de options, para quando algum serviço externo ficar Unhealthy, não quebrar integração com Prometheus
app.UseHealthChecksPrometheusExporter("/metrics", options => options.ResultStatusCodes[HealthStatus.Unhealthy] = (int)HttpStatusCode.OK);
app.UseHttpMetrics();
```
Ja tem a conf do prometheus para ouvir essas metricas da aplicação, olhar o arquivo prometheus.yml

E apenas com essa configuração temos as métricas basicas, pra essa aplicação (requisições e healths).

E como temos já um grafana rodando. Na pasta grafana_dashboard_export, tem um dash pra essa aplicação para ser importado

**Lembrar apenas de configurar datasources no grafana:
* Prometheus: http://localhost:9090
* Elastic: http://localhost:9200
* Jeager: http://localhost:16686