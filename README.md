# Sistema de Processamento de Pedidos

Este projeto implementa uma API para processamento de pedidos utilizando arquitetura em camadas, mensageria assíncrona e múltiplos mecanismos de persistência e cache. O objetivo é simular um fluxo real de processamento com rastreabilidade, resiliência e escalabilidade.

---

## 🚀 Execução do projeto

Para executar todo o ambiente, é necessário apenas ter o **Docker** e o **Docker Compose** instalados.

O `docker-compose` já inclui todos os serviços necessários:

* API **.NET 10**
* **SQL Server** para dados transacionais
* **RabbitMQ** para mensageria
* **MongoDB** para logs e auditoria
* **Redis** para cache

Execute:

```bash
docker compose up --build
```

Após subir os containers, a API estará disponível junto com a documentação interativa via Swagger.

---

## 🧱 Arquitetura do projeto

Foi adotada **arquitetura em camadas (Layered Architecture)** com separação clara de responsabilidades:

```
Domain
Application
Infrastructure
Messaging (Consumer)
API
```

### Responsabilidades

* **Domain** → Entidades, enums e regras de negócio
* **Application** → Casos de uso e orquestração das operações
* **Infrastructure** → Acesso a banco de dados, cache e integrações externas
* **Messaging** → Consumidores de fila e processamento assíncrono
* **API** → Exposição de endpoints HTTP

Essa estrutura facilita testes, manutenção e evolução do sistema.

---

## 📦 Enums utilizados na API

Alguns endpoints utilizam enums para filtragem e ordenação.

### Status do Pedido

```csharp
public enum PedidoStatus
{
    Recebido = 1,
    EmProcessamento = 2,
    Processado = 3,
    Falha = 4
}
```

### Ordenação

```csharp
public enum Ordenacao
{
    Asc,
    Desc
}
```

---

## 📨 Mensageria com RabbitMQ

Foi utilizada a biblioteca **MassTransit** para integração com o RabbitMQ.

### Motivações

* Abstrai a criação de filas e exchanges
* Reduz código repetitivo e configurações manuais
* Facilita implementação de retries e políticas de falha
* Integração nativa com .NET e DI

O uso dessa biblioteca evita a necessidade de escrever manualmente DDL e configurações específicas exigidas pela biblioteca oficial do RabbitMQ.

---

## 🔄 Processamento de pedidos

Ao consumir uma mensagem da fila, o sistema simula o processamento do pedido.
Para fins de testes, o resultado é definido de forma **aleatória**:

* Sucesso → pedido marcado como **Processado**
* Falha → pedido marcado como **Falha**

Isso permite validar o funcionamento do endpoint de **reprocessamento**.

---

## 🗄️ Persistência de dados

O sistema utiliza dois bancos de dados com finalidades diferentes:

### SQL Server

Responsável por armazenar:

* pedidos
* histórico de status

Isso garante consistência e suporte a transações.

### MongoDB

Responsável por armazenar:

* logs de processamento
* auditoria das operações

Essa separação permite manter logs de forma escalável sem impactar a base transacional.

---

## ⚡ Cache com Redis

O **Redis** foi utilizado como camada de cache para reduzir a carga no banco de dados e melhorar o tempo de resposta.

### Endpoint cacheado

```
GET /api/pedidos/{id}
```

### Estratégia adotada

* Cache **por ID**
* TTL: **5 minutos**
* Cada alteração de status do pedido **invalida a chave correspondente**

Fluxo:

```
API → Redis → SQL Server (em caso de cache miss)
```

Isso garante consistência dos dados sem comprometer performance.

---

## 🧪 Reprocessamento de pedidos

Caso um pedido falhe, ele pode ser reprocessado via endpoint específico.
O sistema reenfileira a mensagem no RabbitMQ e executa novamente o fluxo de processamento.

---

## 📁 Estrutura geral do projeto

```
src/
 ├── Pedido.Api
 ├── Pedido.Application
 ├── Pedido.Domain
 ├── Pedido.Infrastructure
 └── Pedido.Messaging

docker-compose.yml
```

---

## 🛠️ Melhorias futuras

Abaixo estão melhorias planejadas que podem evoluir o sistema em termos de observabilidade, robustez e integração:

### Observabilidade e monitoramento

* Integração com **Grafana** para visualização de métricas
* Monitoramento das filas do RabbitMQ
* Métricas de cache hit/miss no Redis

### Evolução dos endpoints

* Expandir o endpoint:

```
GET /api/pedidos/{id}
```

para também consultar o MongoDB e retornar:

* histórico completo de processamento
* logs associados ao pedido

### Integrações externas

* Implementar configuração dinâmica para **envio de webhooks**
* Permitir que sistemas externos sejam notificados quando o status do pedido mudar

### Resiliência

* Implementar políticas de retry exponencial no consumidor
* Circuit breaker para dependências externas

### Qualidade e manutenção

* Cobertura de testes unitários e de integração
* Versionamento de API
* Validação de contratos com testes de contrato (contract testing)

### Escalabilidade

* Separar API e consumidores em serviços independentes
* Configurar múltiplas instâncias do consumer para processamento paralelo

### Configuração e DevOps

* Uso de variáveis de ambiente para todas as configurações sensíveis
* Health checks para todos os serviços no docker-compose

