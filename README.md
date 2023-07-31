# Force

General-purpose library for rapid .NET applications development.
Built around CQRS and DDD approaches.

Install stable version into your project via Nuget:

    Install-Package Force

And when you start, just keep

    using Force; // :-)

## Graylog configuration

Start graylog server with command

    docker compose -f docker-compose.yml -f docker-compose.override.yml -p force up -d

Open http://localhost:9000/, authorize and go to System/Inputs/Select input/GELF TCP and press Launch new input and set 0.0.0.0 to Bind address field and 12201 to Port field.

Now graylog prepare to get logs from WebApp.

## Use Force as a dotnet new template

Clone this repository. Open repository directory in CMD (you should open directory with .template.config). Run command

    dotnet new install .

This command will save this template on your machine. Later you can create a new solution with this template by command

    dotnet new ForceWebApp

Be sure that your directory doesn't contain any useless directories like .git or bin/Debug etc when you run install command otherwise they will be installed as part of the template. 

