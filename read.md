
Build and Publish on Heroku

Instalar CLI Heroku
Iniciar Docker Desktop

On git bash

    1. heroku login
        1.1 Ctrl + C / N para incluir novo comando sem para a conexao com o client heroku
    2. heroku container:login
    3. Criar o app no site heroku umfgcloud-loja-service
        3.1 Config Vars configurar  ASPNETCORE_ENVIRONMENT / Development -> Add
    4. docker build -t umfgcloud-loja-service . criar projeto no heroku com mesmo nome
        4.1 Apenas na primeira vez rodar o comando: heroku stack:set container -a umfgcloud-loja-service
            4.1.1 y + enter para confirmar
    5. heroku container:push web -a umfgcloud-loja-service
    6. heroku container:release web -a umfgcloud-loja-service

View log error's

On git bach

    heroku logs --tail --app umfgcloud-loja-service
    exit: Ctrl +