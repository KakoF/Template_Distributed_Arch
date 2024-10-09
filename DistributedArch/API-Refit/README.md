
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