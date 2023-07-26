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
