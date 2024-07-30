# FastFoodUserManagement

O repositorio FastFoodUserManagement tem por objetivo implementar uma Lambda Function responsável por realizar a criação e autenticação de usuários utilizando o AWS Cognito.

### Variáveis de ambiente
Todas as variáveis de ambiente do projeto visam fazer integração com algum serviço da AWS. Explicaremos a finalidade de cada uma:

- AWS_ACCESS_KEY_DYNAMO: "Access key" da AWS. Recurso gerado no IAM para podermos nos conectar aos serviços da AWS;
- AWS_SECRET_KEY_DYNAMO: "Secret key" da AWS. Recurso gerado no IAM para podermos nos conectar aos serviços da AWS. Deve ser utilizado corretamente com seu par AWS_ACCESS_KEY_DYNAMO;
- AWS_TABLE_NAME_DYNAMO: Nome da tabela de usuários cadastrada no DynamoDB;
- AWS_USER_POOL_ID: Nome da user pool criada no AWS Cognito;
- AWS_CLIENT_ID_COGNITO: ClientId da pool no AWS Cognito;
- GUEST_EMAIL: Usuário padrão para realizar autenticação de forma anônima no AWS Cognito;
- GUEST_IDENTIFICATION: senha do usuário padrão para realizar autenticação de forma anônima no AWS Cognito.
- AWS_SQS: Url da fila de log no SQS da AWS.
- AWS_SQS_GROUP_ID: Group Id da fila de log no SQS da AWS.

### Execução

Para executar com docker, basta executar o seguinte comando na pasta raiz do projeto para gerar a imagem:

``` docker build -t fast_food_user_management -f .\src\Presentation\FastFoodUserManagement\Dockerfile . ```

Para subir o container, basta executar o seguinte comando:

``` 
docker run -e AWS_ACCESS_KEY_DYNAMO=""
-e AWS_SECRET_KEY_DYNAMO=""
-e AWS_USER_POOL_ID=""
-e AWS_CLIENT_ID_COGNITO=""
-e GUEST_EMAIL=""
-e GUEST_IDENTIFICATION=""
-e AWS_SQS=""
-e AWS_SQS_GROUP_ID=""
-p 8081:8081 -p 8080:8080 fast_food_user_management
```

Observação: as variáveis de ambiente não estão com valores para não expor meu ambiente AWS. Para utilizar o serviço corretamente, é necessário definir um valor para as variáveis.

Além disso, como o projeto foi desenvolvido em .NET, também é possível executá-lo pelo Visual Studio ou com o CLI do .NET.



### Testes

Conforme foi solicitado, estou postando aqui as evidências de cobertura dos testes. A cobertura foi calculada via integração com o [SonarCloud](https://sonarcloud.io/) e pode ser vista nesse [link](https://sonarcloud.io/organizations/techchallengefernandomelim/projects). A integração com todos os repositórios poderá ser vista nesse link.


### Endpoints

Os endpoints presentes nesse projeto são:

- POST /User/CreateUser: responsável por criar um usuário.
- GET /User/AuthenticateUser/{cpf}: responsável por autenticar um usuário com apenas seu CPF utilizando o cognito.
- GET /User/AuthenticateAsGuest: responsável por autenticar o usuário como anônimo.
- GET /User/GetUsers: responsável por retornar todos os usuários.