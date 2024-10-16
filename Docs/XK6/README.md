# XK6

## Doc

https://github.com/grafana/xk6?form=MG0AV3

## Breve explicação

é uma ferramenta de linha de comando que permite criar builds personalizadas do k6, um framework de teste de carga open-source. Com o xk6, você pode adicionar extensões personalizadas ao k6 para atender a necessidades específicas, como integrações com outras ferramentas ou funcionalidades adicionais. É especialmente útil para desenvolvedores que precisam de maior flexibilidade e customização nos seus testes de performance.

Precisamos de uma execução do docker para gerar o executável

```cmd
docker run --rm -it -e GOOS=windows -v "%cd%:/xk6" grafana/xk6 build v0.45.1 --output k6.exe --with github.com/mostafa/xk6-kafka@v0.17.0 --with github.com/grafana/xk6-output-influxdb@v0.3.0
```

E após gerar o executável conseguimos usa-lo. Com os exemplos:

```cmd
x-k6 -v
x-k6 run -u 10 -d 10s test.js
```

Caso precise de um container com Rabbit
```cmd
docker run -d --hostname local-rabbit --name rabbit -e RABBITMQ_DEFAULT_USER=user -e RABBITMQ_DEFAULT_PASS=password -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```