# API com Handle (Middleware) para erros

## Proposta do Projeto

Temos umas rotas que vão gerar exceptions para visualizar como o middleware se comporta


## Infraestrutura
Ja existe um composer com a infra, e nesse projeto tem 2 dependencias desse arquivo:

```
Elasticsearch
Kibana
Prometheus
Grafana
```
Como tem muita infra envolvida, avalie bem se realmente no momento você precisará colocar todos os containers de pé ao mesmo tempo. 
Na pasta **ComposerTemplate**, executar:

```cmd
docker compose up -d 
```

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

### Handle (Middleware) para erros

O conceito é que **DomainException** são erros **tratados** e o resto seria **erros inesperados** 

O projeto é simples ele apenas apresenta uma forma de lançar seus erros para os controllers usando a classe **DomainException** com um Middleware que consegue interpretar essa exceção.
Sendo controlado pelo código em seu fluxo:

Exemplos de utilização:
```c#
if(user == null)
    throw new DomainException("User not found");
    
return user;
```
```js
Aprensetando esse reponse:
{
  "StatusCode": 400,
  "Message": "User not found"
}
```
```c#
if(user == null)
    throw new DomainException("User not found", 404);
    
return user;
```
```js
Aprensetando esse reponse:
{
  "StatusCode": 404,
  "Message": "User not found"
}
```
```c#
if(user == null)
    throw new DomainException("User not found", new List<string>{"Erro 1", "Erro 2"} ,404);
    
return user;
```
```js
Aprensetando esse reponse:
{
  "statusCode": 404,
  "message": "User not found",
  "errors": [
    "Erro 1", 
    "Erro 2"
  ]
}
```



Esse exception é tratado pelo **ErrorHandlingMiddleware**, registrado no **WebApplication** na inicialização do projeto
```c#
app.UseMiddleware(typeof(ErrorHandlingMiddleware));
```


if (exception is DomainException)
{
	var ex = (DomainException)exception;
	code = ex.StatusCode;
	error = new ErrorResponse(code, exception.Message, ex.Errors);
	_logger.LogWarning(exception, exception.Message);
}
else
{
	code = (int)HttpStatusCode.InternalServerError;
	error = new ErrorResponse(code);
	_logger.LogError(exception, exception.Message);
}


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
      "API_ErrorHandler": "Information" // Define Information para sua aplicação
    }
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning",
        "API_ErrorHandler": "Information"
      }
    }
  },
  "ElasticConfiguration": {
    "Uri": "http://localhost:9200"
  },
  "AllowedHosts": "*"
}
```
Onde registramos apenas Warnings do **Default, Microsoft e System**. E para Logs de info que escolhermos lançar, seria utilizada pelo **API_ErrorHandler** (lembrando que isso é o namespace do projeto, se for fazer o mesmo em outro, precisa colocar o mesmo namespace)



O middleware de erro também registra logs pelo método:
```c#
int code;
ErrorResponse error;

if (exception is DomainException)
{
	var ex = (DomainException)exception;
	code = ex.StatusCode;
	error = new ErrorResponse(code, exception.Message, ex.Errors);
	_logger.LogWarning(exception, exception.Message);
}
else
{
	code = (int)HttpStatusCode.InternalServerError;
	error = new ErrorResponse(code);
	_logger.LogError(exception, exception.Message);
}

var result = JsonConvert.SerializeObject(error);
context.Response.ContentType = "application/json";
context.Response.StatusCode = (int)code;
return context.Response.WriteAsync(result);
```
Onde **DomainException** geram Logs de Warnings

E **Exceptions** geram Logs de Erros

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
  .AddElasticsearch(builder.Configuration["ElasticConfiguration:Uri"]!, timeout: TimeSpan.FromSeconds(2), name: "elasticsearch", failureStatus: HealthStatus.Unhealthy, tags: new[] { nameof(builder.Environment) }); // Configuração para seu Elasticsearch
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

**Lembrar apenas de configurare os 2 datasources no grafana:
* Prometheus: http://localhost:9090
* Elastic: http://localhost:9200
* Kibana: http://localhost:5601
* Grafana: http://localhost:3000