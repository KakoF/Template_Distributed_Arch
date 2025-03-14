import http from 'k6/http';
import { check } from 'k6';
import { sleep } from 'k6';


export let options = {
    stages: [
        { duration: "30s", target: 100 }, // Sobe para 100 usuários em 30 segundos
        { duration: "1m", target: 200 }, // Mantém 200 usuários por 1 minuto
        { duration: "1m", target: 500 }, // Sobe para 500 usuários em 1 minuto
        { duration: "30s", target: 1000 }, // Sobe para 1000 usuários em 30 segundos
        { duration: "30s", target: 0 }, // Reduz para 0 usuários (finaliza o teste)
    ],
};

export default function () {
    const url = "https://localhost:7197/User/1"; // Troque pelo seu endpoint
    const res = http.get(url);
    // Verifica se o status da resposta é 200 (OK)
    check(res, {
        "status é 200": (r) => r.status === 200,
    });

    sleep(1); // Atraso de 1 segundo entre requisições
}