import http from 'k6/http';
import { check } from 'k6';

//Importamos uma lib externa para relatorio
import { htmlReport } from "https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js";

//Smoke test
//Isso significa que o teste executará uma carga com 1 usuário virtual por 30 segundos, e o teste será considerado bem-sucedido se pelo menos 99% das verificações passarem
export let options = {
    vus: 1, //Define o número de virtual users (usuários virtuais). Aqui, está configurado para 1, ou seja, um usuário virtual simulando requisições.
    duration: '3s', //Define a duração do teste. Aqui, está configurado para 30 segundos.
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

/*
Exemplo de retorno de resposta:
checks.........................: 100.00% 497 out of 497                                                                 -> Verificacao do check que colocamos
     data_received..................: 77 kB   26 kB/s                                                                   -> Quantidade de dados recebidos durante o teste. 77 kB recebidos a uma taxa de 26 kB/s.
     data_sent......................: 22 kB   7.3 kB/s                                                                  -> Quantidade de dados enviados durante o teste. 22 kB enviados a uma taxa de 7.3 kB/s.
     http_req_blocked...............: avg=83.48µs min=0s     med=0s     max=41.45ms  p(90)=0s       p(95)=0s            -> Tempo que as requisições HTTP ficaram bloqueadas. Média: 83.48µs, Mediana: 0s, Máximo: 41.45ms
     http_req_connecting............: avg=0s      min=0s     med=0s     max=0s       p(90)=0s       p(95)=0s            -> empo de conexão para as requisições HTTP. Todos os tempos são 0s (provavelmente porque as conexões foram reutilizadas).
     http_req_duration..............: avg=5.83ms  min=2.55ms med=4.06ms max=658.51ms p(90)=5.4ms    p(95)=6ms           -> Duração total das requisições HTTP. Média: 5.83ms, Mediana: 4.06ms, Máximo: 658.51ms, Percentis (90% e 95%): 5.4ms e 6ms.
       { expected_response:true }...: avg=5.83ms  min=2.55ms med=4.06ms max=658.51ms p(90)=5.4ms    p(95)=6ms           -> Estatísticas específicas para respostas esperadas. Igual ao http_req_duration
     http_req_failed................: 0.00%   0 out of 497                                                              -> Taxa de falhas das requisições HTTP. 0.00%, ou seja, nenhuma falha em 497 requisições.
     http_req_receiving.............: avg=73.23µs min=0s     med=0s     max=1.5ms    p(90)=224.28µs p(95)=531.57µs      -> empo gasto recebendo a resposta HTTP. Média: 73.23µs, Mediana: 0s, Máximo: 1.5ms, Percentis (90% e 95%): 224.28µs e 531.57µs
     http_req_sending...............: avg=59.07µs min=0s     med=0s     max=1.12ms   p(90)=0s       p(95)=532.32µs      -> empo gasto enviando a requisição HTTP. Média: 59.07µs, Mediana: 0s, Máximo: 1.12ms, Percentis (90% e 95%): 0s e 532.32µs.
     http_req_tls_handshaking.......: avg=72.29µs min=0s     med=0s     max=35.93ms  p(90)=0s       p(95)=0s            -> Tempo gasto na negociação TLS. Média: 72.29µs, Mediana: 0s, Máximo: 35.93ms
     http_req_waiting...............: avg=5.69ms  min=2.55ms med=4ms    max=655.88ms p(90)=5ms      p(95)=5.91ms        -> Tempo de espera pela resposta do servidor (latência). Média: 5.69ms, Mediana: 4ms, Máximo: 655.88ms, Percentis (90% e 95%): 5ms e 5.91ms
     http_reqs......................: 497     165.522673/s                                                              -> Número total de requisições HTTP. 497 requisições a uma taxa de 165.52/s.
     iteration_duration.............: avg=6.03ms  min=2.97ms med=4.25ms max=704.07ms p(90)=5.54ms   p(95)=6ms           -> Duração total de cada iteração de teste. Média: 6.03ms, Mediana: 4.25ms, Máximo: 704.07ms, Percentis (90% e 95%): 5.54ms e 6ms
     iterations.....................: 497     165.522673/s                                                              -> Número total de iterações. 497 iterações a uma taxa de 165.52/s.
     vus............................: 1       min=1          max=1                                                      -> Número de usuários virtuais. 1 usuário virtual no total.
     vus_max........................: 1       min=1          max=1                                                      -> Número máximo de usuários virtuais. 1 usuário virtual no máximo.
*/



//Faze de desmontagem, podemos gerar relatórios
//Se fizermos isso ele não da mais a saida no terminal

export function handleSummary(data) {
    return {
        "./relatorio/smokeTest/relatorio_k6.html": htmlReport(data),
    };
}