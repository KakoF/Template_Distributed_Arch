# Documentação e exemplos para uso do Kibana


## DevTools
Na aba de devtools, conseguimos executar algumas querys, vou registrar alguns exeplos de como foram feitas:

Querys utilizadas para visualização dos logs, utilizando filtro, sort e size
```
GET api-*-*-2024-10/_search
{
  "size": 10,
  "sort": [
    {
      "@timestamp": {
        "order": "desc"
      }
    }
  ],
  "query": {
    // "match_all": {}
    "match": {
      "fields.TracingId": "a24d331f-243a-4016-a552-e58cb5e5ebe4"
    }
  }
}

GET api-*-*-*/_search
{
  "size": 10,
  "sort": [
    {
      "@timestamp": {
        "order": "desc"
      }
    }
  ],
  "query": {
    // "match_all": {}
    "match": {
      "fields.TracingId": "a24d331f-243a-4016-a552-e58cb5e5ebe4"
    }
  }
}
```