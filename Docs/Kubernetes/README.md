# Kubernetes

Kubernetes é uma plataforma open-source para orquestrar e gerenciar contêineres. Ele foi originalmente desenvolvido pelo Google e é agora mantido pela Cloud Native Computing Foundation (CNCF). Aqui estão alguns conceitos básicos:

1. Contêineres: São pacotes leves que incluem tudo o que é necessário para executar um aplicativo, como código, tempo de execução, ferramentas de sistema, bibliotecas e configurações1


2. Pods: A menor unidade de implantação em Kubernetes. Um Pod é um grupo de um ou mais contêineres que são executados juntos na mesma máquina1

3. Deployments: Gerenciam a atualização e o gerenciamento de Pods. Eles permitem que você defina como seus aplicativos devem ser implantados e atualizados.

4. Services: Fornecem um ponto de entrada estável para um conjunto de Pods. Eles atuam como um nome de host virtual que pode ser usado para acessar os Pods.

5. Ingress: Controla o tráfego HTTP/S para os Pods. Ele permite que você configure regras para rotear o tráfego para diferentes serviços.

6. Volumes: Permitem que você compartilhe dados entre contêineres. Eles podem ser usados para armazenar dados persistentes ou compartilhar dados temporários entre contêineres.

7. Namespaces: Permitem que você isole recursos dentro de um cluster Kubernetes. Eles são úteis para organizar recursos e evitar conflitos de nomes.

8. ConfigMaps e Secrets: Usados para armazenar dados que podem ser usados por seus contêineres, como configurações de aplicativos ou senhas.

Esses são alguns dos conceitos básicos do Kubernetes. Se você quiser aprender mais, a documentação oficial (https://kubernetes.io/pt-br/docs/tutorials/kubernetes-basics/?form=MG0AV3) do Kubernetes oferece tutoriais interativos que podem ser muito úteis.

## Pods e Contêineres

* Pods são a menor unidade de implantação no Kubernetes. Eles contêm um ou mais contêineres que compartilham o mesmo endereço IP e armazenamento. Podem ser usados para executar um aplicativo ou um microserviço.

* Contêineres dentro de um Pod compartilham recursos, o que permite a comunicação rápida e eficiente entre eles.

## Deployments
* Deployments gerenciam a criação e a escalabilidade de Pods. Eles garantem que o número desejado de réplicas dos Pods está em execução. Se um Pod falhar, o Deployment automaticamente inicia um novo Pod para substituir o falho.

Um exemplo de Deployment:

```yml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: nginx-deployment
spec:
  replicas: 3
  selector:
    matchLabels:
      app: nginx
  template:
    metadata:
      labels:
        app: nginx
    spec:
      containers:
      - name: nginx
        image: nginx:1.14.2
        ports:
        - containerPort: 80
```


## Services

* Services fornecem uma maneira de expor um ou mais Pods para a rede externa. Existem diferentes tipos de Services, como ClusterIP, NodePort e LoadBalancer.

* ClusterIP é o tipo padrão que expõe o serviço em um IP interno dentro do cluster. NodePort expõe o serviço em um número de porta específico em cada nó do cluster, e LoadBalancer configura um balanceador de carga externo.

## Volumes
* Volumes permitem o armazenamento persistente de dados. Existem diferentes tipos de volumes, como EmptyDir, HostPath, NFS, PersistentVolumeClaim, entre outros.

* PersistentVolume (PV) e PersistentVolumeClaim (PVC) são usados para abstrair detalhes de armazenamento. PVs são recursos de armazenamento provisionados pelo administrador, enquanto PVCs são pedidos de armazenamento feitos por usuários.

## ConfigMaps e Secrets
* ConfigMaps são usados para armazenar dados de configuração em pares chave-valor. Eles permitem que você desacople configurações do contêiner.

* Secrets armazenam dados sensíveis, como senhas e tokens de API, de forma criptografada. Eles garantem que esses dados não sejam expostos diretamente no Pod.

## Namespaces
* Namespaces fornecem um escopo para os recursos. Eles permitem que você separe recursos de diferentes projetos ou equipes dentro do mesmo cluster. Por exemplo, você pode ter um namespace de produção e um namespace de desenvolvimento.

## Ingress
* Ingress gerencia o acesso externo aos serviços dentro de um cluster, normalmente via HTTP e HTTPS. Ele oferece recursos como roteamento baseado em host e caminho.

Um exemplo de configuração de Ingress:

```yml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: example-ingress
spec:
  rules:
  - host: example.com
    http:
      paths:
      - path: /
        pathType: Prefix
        backend:
          service:
            name: example-service
            port:
              number: 80
```

Cada um desses componentes tem um papel crucial na orquestração e gerenciamento de contêineres no Kubernetes


## Autoescalonamento
* Horizontal Pod Autoscaler (HPA): Automaticamente escala o número de réplicas de um Pod com base na utilização de CPU ou outras métricas selecionadas. Isso permite que seu aplicativo se ajuste dinamicamente à carga de trabalho.

* Vertical Pod Autoscaler (VPA): Ajusta automaticamente os limites de recursos (CPU e memória) dos containers em execução para otimizar o uso de recursos.

## Segurança e RBAC
* Role-Based Access Control (RBAC): Implementa a segurança em Kubernetes. Permite que você controle quem tem acesso ao quê, atribuindo permissões específicas para usuários e aplicativos.

* Network Policies: Regras que controlam a comunicação entre Pods. Elas podem ser usadas para restringir o tráfego de rede, garantindo que os Pods só possam se comunicar com outros Pods autorizados.


## Observabilidade e Monitoramento
* Prometheus: Uma ferramenta de monitoramento e alerta que coleta métricas de aplicativos e infraestrutura. Pode ser integrado com Kubernetes para monitorar a saúde dos Pods, nós e serviços.

* Grafana: Uma ferramenta de visualização que pode ser usada junto com Prometheus para criar dashboards interativos e monitorar as métricas em tempo real.

* Logs: Usar kubectl logs para visualizar os logs dos contêineres. Ferramentas como Fluentd podem ser usadas para coletar e centralizar logs de todo o cluster.