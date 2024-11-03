# Plano Execução

Para identificar o custo da sua consulta em SQL, você pode usar planos de execução. Esses planos fornecem uma análise detalhada de como o SQL Server processa sua consulta, permitindo que você veja o custo estimado em termos de recursos utilizados.

## Passos para Identificar o Custo da Consulta:

### Usar o "EXPLAIN" ou "EXPLAIN ANALYZE" no PostgreSQL:
```sql
EXPLAIN SELECT * FROM sua_tabela;

mais detalhada
EXPLAIN ANALYZE SELECT * FROM sua_tabela;
```

### Usar o "EXPLAIN" no MySQL:
```sql
EXPLAIN SELECT * FROM sua_tabela;
```

### Usar o "SET SHOWPLAN_TEXT ON" ou "SET SHOWPLAN_ALL ON" no SQL Server:
```sql
SET SHOWPLAN_TEXT ON;
GO
SELECT * FROM sua_tabela;
GO
SET SHOWPLAN_TEXT OFF;
GO
```
### Usar o "EXPLAIN PLAN" no Oracle:
sql

```sql
EXPLAIN PLAN FOR SELECT * FROM sua_tabela;
SELECT * FROM TABLE(DBMS_XPLAN.DISPLAY);
```

## Interpretação do Plano de Execução:
* Custo Total: Indica o custo estimado da consulta.

* Operações: As etapas que a consulta passa (varredura de índice, junção, etc.).

* Linhas e Tamanho: A quantidade de linhas e o tamanho dos dados processados em cada etapa.