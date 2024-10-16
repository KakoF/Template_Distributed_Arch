import http from 'k6/http';
import { check, sleep } from 'k6';
import { SharedArray } from 'k6/data';

//sleep(1) Cada VU realiza uma requisicao por segundo

//Performance test
//Isso significa que o teste executará uma carga com 1 usuário virtual por 30 segundos, e o teste será considerado bem-sucedido se pelo menos 99% das verificações passarem
export let options = {
    //Com é um teste de performance, vamo definir estagios de carga
    /* Nome dos estagios
        Ramp-up: Aumentar gradualmente o número de usuários para a carga desejada.
        Steady-state: Manter a carga constante durante um período de tempo.
        Ramp-down: Reduzir gradualmente o número de usuários até zero.
    */
    stages: [
        { duration: '10s', target: 10 }, // estágio de Ramp-up, dura 10 segundos com 10 VUS
        { duration: '10s', target: 10 }, // estágio de Steady-state, dura 10 segundos com 10 VUS
        { duration: '10s', target: 0 } // estágio de Ramp-down, dura 10 segundos com 0 VUS
    ],
    thresholds: {
        checks: [
            'rate > 0.95' //Define os critérios de sucesso para o teste. Neste caso, está verificando se a taxa de sucesso das verificações (checks) é maior que 95% (rate > 0.95).
        ],
        http_req_duration: ['p(95) < 200'] //Verifica se 95% das requisições HTTP (p(95)) têm uma duração menor que 200 milissegundos. Isso ajuda a garantir que a maioria das requisições seja rápida o suficiente
    },
};

//Criamos um arquivo para listar ID`s do usuario, para deixar dinamico essa busca
const users = new SharedArray('Leitura do Json Users Id', function () {
    return JSON.parse(open('./inputs/dados.json')).users_id
})


export default function () {
    const ids = users[Math.floor(Math.random() * users.length)].id
    //console.log(ids)
    const BASE_URL = `https://localhost:7197/User/${ids}`;
    const res = http.get(BASE_URL);

    check(res, {
        "status code 200": (r) => r.status === 200,
    });
    sleep(1)
}