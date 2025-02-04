# RDManipulacao

Variveis de ambiente enviadas por email!
Carga de Dados:

1 - Executar o serviço WS.IntegracaoYouTube 
       Configuração -> Abrir o arquivo appsetting.json e configurar com as inforções desejadas
       Detalhes: o serviço irá criar tabelas no sistema e irá alimenta-las com informações da request do youtube conforme os parametros passado no appsetting.json
2 - API.YoutubeService.
       Configuração -> Abrir o arquivo appsettings.json já contém os apontamentos das variaveis de ambiente.
       Detalhes: a api contém swagger com as devidas chamadas dos endpoints. 
       Configuração das requests -> Primeiro passo é preencher o authorize com as informações username: Admin e Password: youtube. 
       Após isso executar o metodo Authorization, caso sucesso o retorno contém o token de autorização.
       Com a informação do token é necessário clicar novamente no Authorize e inserir o dado no campo Value: Bearer token
