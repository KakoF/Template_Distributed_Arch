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


### k6

É uma ferramenta de teste de carga de código aberto desenvolvida pela Grafana Labs. Ela é projetada para ser fácil de usar e produtiva para equipes de engenharia, permitindo testes de desempenho de forma eficiente

No projeto temos uma pasta k6, com scripts para execução dos testes

## Principais Características
* Desempenho: K6 permite simular tráfego realista para testar a capacidade e o desempenho de aplicativos web.

* Flexibilidade: Pode ser usado para testar APIs, sites e outras aplicações web.

 *Extensibilidade: Oferece uma API rica e suporte para scripts personalizados, permitindo que você adapte os testes às suas necessidades específicas

* Integração: Pode ser integrado com outras ferramentas de monitoramento e análise, como Grafana

## Como Funciona
Você escreve scripts em JavaScript que simulam requisições HTTP para o seu aplicativo, e K6 executa esses scripts sob carga para medir o desempenho e identificar possíveis gargalos.

## Benefícios

* Facilidade de Uso: Scripts simples e intuitivos.

* Resultados Detalhados: Fornece métricas detalhadas sobre o desempenho do teste.

* Código Aberto: Gratuito e com uma comunidade ativa de desenvolvedores.

## Tipos de testes

### Smoke test
É uma verificação rápida e inicial para garantir que as principais funcionalidades de um sistema ou aplicativo estão funcionando corretamente. Ele é geralmente o primeiro teste realizado após uma compilação para detectar falhas básicas que possam impedir testes mais aprofundados. A ideia é confirmar que o "sistema básico não pega fogo", daí o nome "smoke test". Se o smoke test passar, significa que o sistema está pronto para ser submetido a testes mais rigorosos.

Geralmente abrange:

* Inicialização do aplicativo.

* Funcionalidades principais.

* Conexões com bancos de dados ou serviços externos.

### Load test (teste de carga/performance)

É um tipo de teste de desempenho que simula um número crescente de usuários ou transações em um sistema para determinar como ele se comporta sob carga. O objetivo é identificar gargalos de desempenho, tempos de resposta e capacidade máxima antes que o sistema comece a falhar ou se tornar lento. Aqui está um panorama sobre isso:

**Objetivos do Load Test:**
* Identificar Limites de Capacidade: Saber quantos usuários simultâneos ou transações o sistema pode suportar.

* Medição de Desempenho: Avaliar tempos de resposta e throughput em diferentes níveis de carga.

* Detecção de Gargalos: Identificar componentes do sistema que causam lentidão ou falhas sob carga.

**Ferramentas Populares de Load Testing:**
* Apache JMeter: Ferramenta open-source que pode simular diferentes cargas de trabalho e medir o desempenho de aplicações web.

* Gatling: Ferramenta de código aberto com uma DSL em Scala para definir cenários de teste de carga.

* k6: Uma ferramenta de teste de carga moderna, fácil de usar, com scripts em JavaScript.

* LoadRunner: Ferramenta comercial com suporte abrangente para diferentes tipos de aplicações.

**Como Realizar um Load Test:**
* Definir Cenários de Teste: Identificar os casos de uso mais críticos a serem testados.

* Configurar a Ferramenta: Preparar os scripts de teste na ferramenta escolhida.

* Executar Testes de Carga: Gradualmente aumentar a carga e monitorar o comportamento do sistema.

* Analisar Resultados: Examinar métricas de desempenho e identificar possíveis gargalos.

* Ajustar e Repetir: Ajustar a configuração do sistema com base nos resultados e repetir os testes conforme necessário.

**Benefícios:**
* Melhora a Confiabilidade: Garante que o sistema possa suportar cargas reais de produção.

* Identifica Problemas: Detecta problemas antes que afetem os usuários finais.

* Planejamento de Capacidade: Ajuda a planejar a escalabilidade do sistema para futuras necessidades.


### Stress test  e Spike test:

Perguntas que queremos responder com esses 2 cenários de testes:
 1. Como seu sistema se comporta em condições extremas?
 2. Qual é a capacidade máxima do seu sistema em termos de usuários ou taxa de tranferência?
 3. Ponto de ruptura do seu sistema?
 4. O sistema se recupera sem intervenção manual após o término do teste de estresse?

**Resoluções:**

* Objetivo: Avaliar a capacidade do sistema de lidar com condições extremas além da carga normal esperada.

* Como Funciona: Coloca o sistema sob uma carga muito alta ou priva-o de recursos críticos, como * memória ou CPU.

* Benefícios: Identifica o ponto de falha do sistema e ajuda a garantir que ele possa se recuperar de uma falha.

**Spike Test:**

* Objetivo: Avaliar a capacidade do sistema de lidar com picos repentinos de carga.

* Como Funciona: Aumenta drasticamente a carga de forma abrupta e depois retorna ao nível normal.

* enefícios: Testa a resiliência do sistema e garante que ele possa gerenciar picos de tráfego inesperados sem falhar.


### Soak test:

É um tipo de teste de desempenho que verifica a estabilidade e a confiabilidade de um sistema ao submetê-lo a uma carga normal por um período prolongado. O objetivo é identificar problemas como vazamentos de memória, degradação de desempenho e outros comportamentos anômalos que podem não ser detectados em testes de carga de curta duração. Basicamente, você "embebe" o sistema em carga, mantendo-o sob pressão constante por um longo tempo para ver como ele se comporta.

**Benefícios do Soak Test:**

* Detecção de Vazamentos de Recursos: Identifica vazamentos de memória ou conexões que podem levar a falhas ao longo do tempo.

* Avaliação de Desempenho a Longo Prazo: Garante que o sistema mantenha um desempenho aceitável mesmo após um uso prolongado.

* Estabilidade: Verifica se o sistema pode operar continuamente sem falhas ou degradação significativa de desempenho.


### Breakpoint test:

É uma avaliação que determina o ponto exato em que o sistema ou aplicativo começa a falhar sob carga específica. É uma combinação de Load Testing e Stress Testing, onde você aumenta gradualmente a carga até encontrar o limite de capacidade do sistema.

**Objetivos do Breakpoint Test:**
* Identificar o Ponto de Falha: Saber exatamente quando e onde o sistema começa a falhar.

* Avaliar Limites de Performance: Determinar a capacidade máxima do sistema.

* Melhorar a Resiliência: Usar os resultados para fortalecer áreas críticas do sistema.

