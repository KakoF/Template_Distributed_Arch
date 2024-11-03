# Configurar e Utilizar Rate Limiting

Rate limiting é uma técnica essencial para controlar a quantidade de requisições que uma aplicação pode receber, ajudando a prevenir sobrecargas e ataques DDoS. No ASP.NET Core, você pode implementar rate limiting de várias maneiras. Vamos ver um exemplo prático usando o middleware AspNetCoreRateLimit

## Instalação
Adicione o pacote AspNetCoreRateLimit ao seu projeto.
  ```csharp
   dotnet add package AspNetCoreRateLimit
 
   ```

## Configurar appsettings.json:
Adicione a configuração de rate limiting no seu arquivo appsettings.json
   ```json
   {
  "IpRateLimiting": {
    "EnableEndpointRateLimiting": true,
    "StackBlockedRequests": false,
    "HttpStatusCode": 429,
    "GeneralRules": [
      {
        "Endpoint": "*",
        "Period": "1m",
        "Limit": 5
      }
    ]
  }
}
   ```

## Configurar Startup.cs:
Configure os serviços de rate limiting no arquivo Startup.cs
   ```csharp
   public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            // Adiciona serviços de Rate Limiting
            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
            services.AddInMemoryRateLimiting();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseIpRateLimiting();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

   ```

## Criação de Middleware Personalizado (Opcional):
Implementar um middleware de rate limiting personalizado
```csharp
public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly int _requestLimit = 100;
    private static int _requestCount = 0;

    public RateLimitingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (_requestCount >= _requestLimit)
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            await context.Response.WriteAsync("Rate limit exceeded. Try again later.");
            return;
        }

        _requestCount++;
        await _next(context);
        _requestCount--;
    }
}

public static class RateLimitingMiddlewareExtensions
{
    public static IApplicationBuilder UseRateLimiting(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RateLimitingMiddleware>();
    }
}
```


## Registro do Middleware Personalizado
```csharp
public class Startup
{
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        app.UseRateLimiting();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}

```
## Explicação

* Pacote AspNetCoreRateLimit: Fornece uma implementação fácil de configurar para rate limiting.

* Configuração: Define as regras de rate limiting, como o número de requisições permitidas por minuto.

* Registro de Serviços: Adiciona os serviços de rate limiting ao contêiner de injeção de dependências.

* Middleware Personalizado: Um exemplo opcional de como você pode implementar rate limiting manualmente.