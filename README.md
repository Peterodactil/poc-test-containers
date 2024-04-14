# POC

Projeto feito em [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

Solução composta por um projeto web API e um projeto de teste utilizando [xUnit](https://xunit.net/).

Projeto de teste possui testes que faz uso da API para requisitar um endpoint e obter dados de um banco de dados SQL Server.

O serviço de banco MSSQL é providenciado por meio de container utilizando a lib [TestContainers](https://testcontainers.com/).

O container do banco é providenciando durante a inicialização do mock da API.
- se dá inicio à inicialização da instância da API
- container MSSQL é providênciado
- conclusão da inicialização da API
- Program.cs é executado
- instrução `criar banco de dados` é executada pelo Entity Framework
- migração do banco é concluída
- scripts.sql da pasta `Scripts` são executados
- dá inicio a execução do teste de integração

# Classes de teste

1. faz requisição de todos os dados de uma tabela do SQL Server
2. faz requisição do primeiro registro em uma tabela do SQL Server

**Todos os testes compartilham o mesmo mock da API tal como os mesmos containers.**

# Habilitar acesso TestContainers ao Docker no WSL2

_Este recurso é opcional_

Caso esteja fazendo uso do sistema operacional Windows com WSL2, com o Docker instalado no WSL2 e queria fazer com que a lib `TestContainers` acesse o Docker no WSL2, faça:

## Configurando a lib TestContainers para acessar o Docker de outra máquina

1. no Windows, abra a pasta do seu usuário `C:\Users\_seu_usuario_`

2. crie ou edite o arquivo `.testcontainers.properties`

    [Mais informações sobre configurações customizadas](https://dotnet.testcontainers.org/custom_configuration/)

3. adicione o atributo `docker.host` com o IP do Linux que está em execução no WSL2

    exemplo:
    > docker.host`=http://172.28.82.97:2375

## Configurando o servidor do Docker (daemon)

Para que a lib `TestContainers` faça uso do Docker de outra máquina, será necessário expor a API do servidor do Docker. Para isso, faça:

1. crie ou edite o arquivo `/etc/docker/daemon.json` e adicione o conteúdo

    [Mais informações - daemon arquivo de configuração](https://docs.docker.com/reference/cli/dockerd/#daemon-configuration-file)

```json
{
  "debug": true,
  "hosts": ["unix:///var/run/docker.sock", "tcp://0.0.0.0:2375"]
}
```
2. edite o arquivo `/lib/systemd/system/docker.service`

**<p style="color:gold">Caso o Docker seja reinstalado ou atualizado, este passo deve ser refeito.</p>**

```sh
# Comente a instrução existente ExecStart
# ExecStart=/usr/bin/dockerd -H fd:// --containerd=/run/containerd/containerd.sock
#
# E adicione a nova abaixo
ExecStart=/usr/bin/dockerd
```

3. reinicie o servidor do docker
> sudo systemctl daemon-reload

4. reinicie o serviço do docker
> sudo service docker restart