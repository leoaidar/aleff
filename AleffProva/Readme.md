A Solucao foi feita no Visual Studio 2019 .NET Framework 4.8;

Solucao contendo 2 projetos web api e web mvc e projetos com a arquitetura de software em camadas que dá apoio a estes 2;

O Arquivo do Banco de dados está no diretorio /Database importar o banco para alguma ide;

O Banco de dados já está configurado com as tabelas e procedures, caso nao esteja existe uma pasta com os Scripts;

Entre no Web.Config do Aleff.Web.API e do Aleff.Web.MVC e mude na connectionstring no AttachDbFilename o caminho absoluto onde esta o diretorio /Database que o banco sera atachado:
Exemplo: AttachDbFilename=C:\Users\leona\source\repos\AleffProva\Database\alef.mdf;

No projeto do Aleff.Web.API tem uma documentacao do Swagger assim que iniciar aparecerá um botao que leva pra documentacao da API.
