import http from 'k6/http';
import { check } from 'k6';

export let options = {
    stages: [
        { duration: '30s', target: 50 },
        { duration: '1m', target: 100 },
        { duration: '10s', target: 0 }
    ]
};

export default function () {
    let url = 'http://localhost:15672/api/exchanges/%2f/amq.default/publish';
    let payload = JSON.stringify({
        properties: {},
        routing_key: "test_queue",
        payload: "Mensagem de teste",
        payload_encoding: "string"
    });

    let params = {
        headers: {
            'Authorization': 'Basic ' + __ENV.RABBITMQ_CREDENTIALS,
            'Content-Type': 'application/json'
        }
    };

    let res = http.post(url, payload, params);

    check(res, {
        'is status 200': (r) => r.status === 200,
        'message sent': (r) => r.json().routed === true
    });
}
