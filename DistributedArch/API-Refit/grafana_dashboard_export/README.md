### Exemplos de querys usadas:

```js
Saude da própria aplicação
healthcheck{healthcheck="self", job="API_Refit"}
```
```js
Saude da elasticsearch
healthcheck{healthcheck="elasticsearch", job="API_Refit"}
```
```js
Uso da CPU pelo serviço
system_runtime_cpu_usage{job="API_Refit"}
```
```js
Uso da memória pelo serviço (bytes / 1024 / 1024 conversão do byte para megabyte)
process_working_set_bytes{job="API_Refit"} / 1024 / 1024
```

```js
Total de requests

Total de requests, não agrupado
increase(http_request_duration_seconds_count{job="API_Refit"}[1m])

Total de requests, agrupado
sum(increase(http_request_duration_seconds_count{job="API_Refit"}[1m]))
```

```js
Taxa de requests

Taxa de requests, não agrupado
rate(http_request_duration_seconds_count{job="API_Refit"}[1m])

Taxa de requests, agrupado
sum(rate(http_request_duration_seconds_count{job="API_Refit"}[1m]))
```


```js
Análise de Latência de Requisição

calcula o 50º percentil (ou mediana) da duração das requisições para o job "API_Refit". Em outras palavras, metade das requisições são mais rápidas que esse valor, e a outra metade é mais lenta.
histogram_quantile(0.5, sum(rate(http_request_duration_seconds_bucket{job="API_Refit"}[1m])) by (le))

Calcula o 90º percentil da duração das requisições. Aqui, 90% das requisições são mais rápidas que esse valor, e apenas 10% são mais lentas.
histogram_quantile(0.9, sum(rate(http_request_duration_seconds_bucket{job="API_Refit"}[1m])) by (le))

Calcula o 95º percentil da duração das requisições. Portanto, 95% das requisições são mais rápidas que esse valor, enquanto somente 5% são mais lentas.
histogram_quantile(0.95, sum(rate(http_request_duration_seconds_bucket{job="API_Refit"}[1m])) by (le))
```
