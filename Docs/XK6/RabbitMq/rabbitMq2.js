import http from 'k6/http';
import { check, sleep } from 'k6';
import crypto from 'k6/crypto';

export let options = {
    stages: [
        { duration: '30s', target: 50 },
        { duration: '1m', target: 100 },
        { duration: '10s', target: 0 }
    ]
};

function toBase64(str) {
    //return Buffer.from(str).toString('base64');
    return 'dXNlcjpwYXNzd29yZA==';
}

export default function () {
    //let url = 'http://localhost:15672/api/exchanges/%2f/amq.default/publish';
    let url = 'http://localhost:15672/api/exchanges/%2f/amq.direct/publish';

    let payload = JSON.stringify({
        properties: {
            delivery_mode: 2,
            priority: 0,
            content_type: "text/plain"
        },

        routing_key: "test",
        payload: "Esta é uma mensagem de teste enviada para o RabbitMQ",
        payload_encoding: "string"
    });

    let params = {
        headers: {
            'Authorization': `Basic ${toBase64('user:password')}`,
            'Content-Type': 'application/json'
        }
    };

    let res = http.post(url, payload, params);
    console.log(res)
    sleep(1)
}
