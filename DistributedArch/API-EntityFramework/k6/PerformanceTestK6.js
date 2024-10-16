import http from 'k6/http';
import { check, sleep } from 'k6';


//sleep(1) Cada VU realiza uma requisicao por segundo

//Performance test
//Isso significa que o teste executará uma carga com 1 usuário virtual por 30 segundos, e o teste será considerado bem-sucedido se pelo menos 99% das verificações passarem
//Com é um teste de performance, vamo definir estagios de carga
/* Nome dos estagios
    Ramp-up: Aumentar gradualmente o número de usuários para a carga desejada.
    Steady-state: Manter a carga constante durante um período de tempo.
    Ramp-down: Reduzir gradualmente o número de usuários até zero.
*/

export let options = {

    stages: [
        { duration: '10s', target: 10 },
    ],
    thresholds: {
        checks: ['rate > 0.95'], //Verifica se pelo menos 95% dos checks são bem-sucedidos.
        http_req_failed: ['rate < 0.01'], //Verifica se menos de 1% das requisições HTTP falham.
        http_req_duration: ['p(95) < 500'], // Verifica se 95% das requisições HTTP são concluídas em menos de 500 milissegundos.

    },
};

export default function () {

    const BASE_URL = `https://test-api.k6.io/`;
    const USER = `${Math.random()}.mail.com`
    const PASS = 'pass123'

    console.log(USER + ', ' + PASS)

    const res = http.post(`${BASE_URL}user/register`, {
        username: USER,
        first_name: "first",
        last_name: "last",
        email: USER,
        password: PASS
    });

    check(res, {
        "status code 201": (r) => r.status === 201,
    });
    sleep(1)
}