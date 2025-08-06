# 🛠️ Guia de Configuração de Máquina Virtual Ubuntu com Grafana e DNS na Azure

Este documento descreve o passo a passo completo para criar uma máquina virtual na Azure, instalar o Grafana e configurar acesso via DNS público com IP estático.

---

## 📦 1. Criar a Máquina Virtual na Azure

1. Acesse [portal.azure.com](https://portal.azure.com)
2. Vá em **"Criar um recurso > Computação > Máquina Virtual"**
3. Preencha:
   - **Nome**: `vm-grafana`
   - **Região**: `Brazil South`
   - **Imagem**: Ubuntu Server 20.04 LTS
   - **Tamanho**: Standard B1s (ou outro conforme necessidade)
   - **Autenticação**: Senha ou Chave SSH
   - **Usuário**: `marcos`
4. Configure as portas de entrada:
   - **22 (SSH)** – acesso remoto
   - **3000 (Grafana)** – acesso via navegador
5. Clique em **"Revisar + criar"** e confirme a criação.

---

## 🌐 2. Definir o IP como Estático

1. No recurso de **IP público** da VM, acesse as configurações.
2. Mude de **Dinâmico** para **Estático**.
   - Garante estabilidade de acesso e preserva configurações de DNS.

---

## 📛 3. Configurar Rótulo DNS da Azure

1. Na VM, vá em **Configurações > Endereço IP público**
2. Defina o campo **Rótulo de nome DNS** como `grafana-marcos`
3. O endereço de acesso se tornará:  
   `http://grafana-marcos.brazilsouth.cloudapp.azure.com:3000`

---

## 🔐 4. Acessar a VM via SSH

No terminal:

```bash
ssh marcos@grafana-marcos.brazilsouth.cloudapp.azure.com


# Atualizar pacotes
sudo apt-get update

# Adicionar repositório oficial do Grafana
sudo apt-get install -y software-properties-common
sudo add-apt-repository "deb https://packages.grafana.com/oss/deb stable main"
wget -q -O - https://packages.grafana.com/gpg.key | sudo apt-key add -
sudo apt-get update

# Instalar Grafana
sudo apt-get install grafana

# Iniciar serviço
sudo systemctl enable grafana-server
sudo systemctl start grafana-server

# Verificar status
sudo systemctl status grafana-server
```

Acesse o Grafana via navegador: http://grafana-marcos.brazilsouth.cloudapp.azure.com:3000
Credenciais padrão:
- Usuário: admin
- Senha: admin (será solicitada alteração na primeira entrada)
