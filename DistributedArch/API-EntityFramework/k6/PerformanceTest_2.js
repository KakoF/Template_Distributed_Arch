import http from 'k6/http';

// scenarios: Define múltiplos cenários de teste. Neste exemplo, temos dois cenários: listar e buscar.
export const options = {
    scenarios: {
        listar: {
            executor: 'constant-arrival-rate', //Executa um número constante de requisições por unidade de tempo.
            exec: 'listar', //Nome da função a ser chamada.
            duration: '30s', //Duração total do teste é 30 segundos.
            rate: 200, //Taxa de 200 usuários virtuais por segundo.
            timeUnit: '1s', //Unidade de tempo usada para a taxa.
            preAllocatedVUs: 150, //Número de usuários virtuais pré-alocados.
            gracefulStop: '5s', //Tempo para permitir que o cenário finalize graciosamente.
            tags: { test_type: 'listagem_usuario' }, //Tags para identificar o cenário.
            /*thresholds: {
                'http_req_duration': ['p(95)<200'],
                'http_req_failed': ['rate<0.01']
            },*/
        },
        buscar: {
            executor: 'per-vu-iterations', //Cada usuário virtual executa um número
            exec: 'buscar',
            vus: 50, //Número de usuários virtuais.
            iterations: 20, //Cada usuário executa 20 iterações.
            maxDuration: '1m', //Duração máxima do cenário é 1 minuto.
            tags: { test_type: 'busca_usuario' }, //Tags para identificar o cenário.
            gracefulStop: '5s', //Tempo para permitir que o cenário finalize graciosamente.
            /*thresholds: {
                'http_req_duration': ['p(95)<500'],
                'http_req_failed': ['rate<0.02']
            }*/
        }
    },
    discardResponseBodies: true //Descartar os corpos das respostas para economizar memória durante o teste.
}

export function listar() {
    http.get(__ENV.URL + '/users')
};

export function buscar() {
    // __VU, pegamos qual VU est'a em execução
    if (__VU % 2 === 0) {
        http.get(__ENV.URL + '/users/2')
    } else {
        http.get(__ENV.URL + '/users/1')
    }
};

//Para setar env na execução
//k6 run .\PerformanceTest_2.js -e URL=https://localhost:7197