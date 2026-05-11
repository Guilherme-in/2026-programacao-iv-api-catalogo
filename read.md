
Build and Publish on Heroku

Instalar CLI Heroku
Iniciar Docker Desktop

On git bash

    heroku login
    heroku container:login
    docker build -t marscloud-atlas-service . 3.1 Apenas na primeira vez rodar o comando: heroku stack:set container -a marscloud-atlas-service
    heroku container:push web -a marscloud-atlas-service
    heroku container:release web -a marscloud-atlas-service

View log error's

On git bach

    heroku logs --tail --app marscloud-atlas-service
    exit: Ctrl +
