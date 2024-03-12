# FastFoodUserManagement

O repositorio FastFoodUserManagement tem por objetivo implementar uma Lambda Function respons�vel por realizar a cria��o e autentica��o de usu�rios utilizando o AWS Cognito.

## Execu��o do proejto
Para executar o projeto � f�cil, basta apenas definir os valores paras as vari�veis de ambiente dele, que se encontram no launchsettings.json da API que est� presente na camada Presentation.
Ap�s isso, basta executar o projeto da forma que preferir, n�s utilizamos o Docker para isso.


### Vari�veis de ambiente
Todas as vari�veis de ambiente do projeto visam fazer integra��o com algum servi�o da AWS. Explicaremos a finalidade de cada uma:

- AWS_ACCESS_KEY_DYNAMO: "Access key" da AWS. Recurso gerado no IAM para podermos nos conectar aos servi�os da AWS;
- AWS_SECRET_KEY_DYNAMO: "Secret key" da AWS. Recurso gerado no IAM para podermos nos conectar aos servi�os da AWS. Deve ser utilizado corretamente com seu par AWS_ACCESS_KEY_DYNAMO;
- AWS_TABLE_NAME_DYNAMO: Nome da tabela de usu�rios cadastrada no DynamoDB;
- LOG_REGION: Regi�o do Log Group criado no Cloudwatch para monitoramento de logs;
- LOG_GROUP: Nome do Log Group criado no Cloudwatch para monitoramento de logs;
- AWS_USER_POOL_ID: Nome da user pool criada no AWS Cognito;
- AWS_CLIENT_ID_COGNITO: ClientId da pool no AWS Cognito;
- GUEST_EMAIL: Usu�rio padr�o para realizar autentica��o de forma an�nima no AWS Cognito;
- GUEST_IDENTIFICATION: senha do usu�rio padr�o para realizar autentica��o de forma an�nima no AWS Cognito.

### Execu��o com Docker

Utilize o seguinte comando na pasta raiz do projeto para criar a imagem Docker

```
docker build -t fast_food_user_management -f .\src\Presentation\FastFoodUserManagement\Dockerfile .
```

Utilize o seguinte comando para subir um container com essa imagem:

```
	docker run -d -p 8080:8080 \
	-e AWS_ACCESS_KEY_DYNAMO="sua_access_key" \
	-e AWS_SECRET_KEY_DYNAMO="sua_secret_key" \
	-e AWS_TABLE_NAME_DYNAMO="nome_da_tabela" \
	-e LOG_REGION="regiao_do_log_group" \
	-e LOG_GROUP="nome_do_log_group" \
	-e AWS_USER_POOL_ID="id_da_user_pool" \
	-e AWS_CLIENT_ID_COGNITO="client_id_do_cognito" \
	-e GUEST_EMAIL="email_do_usuario_padrao" \
	-e GUEST_IDENTIFICATION="senha_do_usuario_padrao" \
	fast_food_user_management
```

ou 

```
docker run -d -p 8080:8080 -e AWS_ACCESS_KEY_DYNAMO="sua_access_key" -e AWS_SECRET_KEY_DYNAMO="sua_secret_key" -e AWS_TABLE_NAME_DYNAMO="nome_da_tabela" -e LOG_REGION="regiao_do_log_group" -e LOG_GROUP="nome_do_log_group" -e AWS_USER_POOL_ID="id_da_user_pool" -e AWS_CLIENT_ID_COGNITO="client_id_do_cognito" -e GUEST_EMAIL="email_do_usuario_padrao" -e GUEST_IDENTIFICATION="senha_do_usuario_padrao" fast_food_user_management
```

Dessa forma o container estar� executando a API.

## Arquitetura do projeto
A seguinte arquitetura foi utilizada para o projeto:

![Texto Alternativo](./images/ArqLambda.png)

Utilizamos 3 servi�os da AWS apenas nese projeto: AWS Cognito, Cloudwatch e DynamoDB.

Como decidimos utilizar a AWS como plataforma nuvem, utilizamos o AWS Cognito para trabalharmos com gerenciamento dos nossos usu�rios. Atrav�s dela, conseguimos cadastrar e autenticar usu�rios de forma f�cil.
Para gerenciar melhor os dados do usu�rio, optamos por utilizar o DynamoDB. Por ser um banco estruturado em tabelas e n�o tendo a necessidade de utilizar mais do que uma, pensamos em utiliz�-lo para armazenar os dados dos usu�rios cadastrados. Nessa solu��o, apenas uma tabela foi utilizada, o nome dela deve ser fornecido pela vari�vel de ambiente AWS_TABLE_NAME_DYNAMO. Como apenas uma �nica tabela foi utilizada, exclu�mos a necessidade de utilizar um sgbd relacional. Utilizar um NoSQL nos d� uma liberdade maior caso seja necess�rio fazer altera��es no esquema de usu�rios salvo no banco, algo que seria mais complicado de se lidar no modelo relacional, onde ter�amos que nos preocupar com a cria��o do esquema e com o versionamento do mesmo.
Para lidar com os erros ocorridos na execu��o do c�digo fonte, utilizamos o Cloudwatch, onde salvamos todos os logs necess�rios de exce��es disparadas durante a execu��o de alguma funcionalidade.
Conforme foi solicitado, essa solu��o est� sendo publicada como uma Lambda function.
