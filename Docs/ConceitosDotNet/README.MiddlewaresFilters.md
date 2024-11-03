# Middlewares e Filters no ASP.NET Core

## Middlewares
Em .NET, middlewares são componentes de software que são executados em uma pipeline de solicitação e resposta no ASP.NET Core. Eles são responsáveis por diversas tarefas, como autenticação, autorização, registro de logs, tratamento de erros, entre outros. Os middlewares processam solicitações HTTP e podem modificar a resposta enviada ao cliente.

### Funcionamento do Middleware:
- **Pipeline**: As solicitações passam por uma série de middlewares, cada um podendo realizar uma ação ou passar a solicitação para o próximo.
- **Ordem de Execução**: A ordem em que os middlewares são registrados no `Startup.cs` determina a ordem de execução.

### Exemplo de Middleware:
**Registro no Pipeline**:
   ```csharp
   public class Startup
   {
       public void Configure(IApplicationBuilder app, IHostingEnvironment env)
       {
           app.UseMiddleware<MeuMiddleware>();
           app.UseRouting();
           app.UseEndpoints(endpoints =>
           {
               endpoints.MapControllers();
           });
       }
   }
   ```

### Implementação do Middleware:
**Registro no Pipeline**:
   ```csharp
   public class MeuMiddleware
    {
        private readonly RequestDelegate _next;

        public MeuMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Lógica antes de passar para o próximo middleware
            await _next(context);
            // Lógica após o próximo middleware
        }
    }
   ```

### Usos Comuns:
* Autenticação e Autorização: Validar identidades e permissões.

* Registro de Logs: Capturar e armazenar logs de atividades.

* Compressão de Respostas: Otimizar o tamanho da resposta.

* Tratamento de Erros: Manipular exceções e erros.



## Filters

Filters são componentes do ASP.NET Core usados para adicionar comportamentos antes ou depois de certas fases no pipeline de processamento de requisições. Existem vários tipos de filters:

* Authorization Filters: Executam antes de qualquer outro filter para verificar a autorização.

* Resource Filters: Executam depois dos Authorization Filters e antes dos Model Binding.

* Action Filters: Executam antes e depois de uma ação do controlador.

* Exception Filters: Tratam exceções não gerenciadas que ocorrem durante a execução de uma ação.

Result Filters: Executam antes e depois da execução de um Action Result.

### Exemplo de Filter:
```csharp
   public class MyActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Lógica antes da execução da ação
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Lógica após a execução da ação
        }
    }
   ```

   ### Registro de Filter:
```csharp
   services.AddControllers(options =>
    {
        options.Filters.Add<MyActionFilter>();
    }
   ```


## Filter em um método em controller
### Exemplo de Filter:
```csharp
  public class MyActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Lógica antes da execução da ação
            Console.WriteLine("Antes da ação: " + context.ActionDescriptor.DisplayName);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Lógica após a execução da ação
            Console.WriteLine("Depois da ação: " + context.ActionDescriptor.DisplayName);
        }
    }
   ```
   ### Registro de Filter:
```csharp
   public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Registrar o filtro
            services.AddScoped<MyActionFilter>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
   ```
   ### Utilização:
```csharp
[ApiController]
[Route("[controller]")]
public class MyController : ControllerBase
{
    [HttpGet]
    [ServiceFilter(typeof(MyActionFilter))]
    public IActionResult Get()
    {
        return Ok("Olá do Controller!");
    }
}
   ```




# Diferenças entre Middlewares e Filters:
1 - Escopo de Aplicação:

* Middlewares: Atuam globalmente na aplicação, em todo o pipeline de requisições.

* Filters: Atuam em um escopo mais específico, como uma ação ou controlador.

2 - Fase de Execução:

* Middlewares: São chamados no início do pipeline e podem envolver toda a aplicação.

* Filters: São chamados em fases específicas no pipeline de MVC (antes ou depois de ações, por exemplo).

3 - Propósito:

* Middlewares: Geralmente usados para tarefas transversais, como autenticação, logging e compressão.

* Filters: Usados para lógica relacionada ao ciclo de vida de ações específicas, como validação de modelo ou tratamento de exceções.


## Resumo:
* Middlewares: Atuam globalmente e mais cedo no pipeline, usados para tarefas transversais.

* Filters: Atuam em pontos específicos, usados para lógica de controlador/ação.