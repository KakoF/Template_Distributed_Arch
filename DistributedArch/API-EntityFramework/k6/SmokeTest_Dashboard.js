import http from 'k6/http';
import { check } from 'k6';

//Importamos uma lib externa para relatorio
import { htmlReport } from "https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js";

//Smoke test
//Isso significa que o teste executará uma carga com 1 usuário virtual por 30 segundos, e o teste será considerado bem-sucedido se pelo menos 99% das verificações passarem
export let options = {
    vus: 5, //Define o número de virtual users (usuários virtuais). Aqui, está configurado para 1, ou seja, um usuário virtual simulando requisições.
    duration: '30s', //Define a duração do teste. Aqui, está configurado para 30 segundos.
    thresholds: { //Define os critérios de sucesso para o teste. Neste caso, está verificando se a taxa de sucesso das verificações (checks) é maior que 99% (rate > 0.99).
        checks: [
            'rate > 0.99'
        ]
    },
};

export default function () {
    const BASE_URL = "https://localhost:7197/User/1";
    const res = http.get(BASE_URL);

    check(res, {
        "status code 200": (r) => r.status === 200,
    });
}

//run --out web-dashboard .\SmokeTest_Dashboard.js
