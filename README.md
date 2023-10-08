# Gomoku Web API

## Origin


Gomoku, also called Five in a Row, is an abstract strategy board game. It is traditionally played with Go pieces (black and white stones) on a 15×15 Go board[1][2] while in the past a 19×19 board was standard.[3][4] Because pieces are typically not moved or removed from the board, gomoku may also be played as a paper-and-pencil game. The game is known in several countries under different names.

https://en.wikipedia.org/wiki/Gomoku

Rules
Players alternate turns placing a stone of their color on an empty intersection. Black plays first. The winner is the first player to form an unbroken line of five stones of their color horizontally, vertically, or diagonally. In some rules, this line must be exactly five stones long; six or more stones in a row does not count as a win and is called an overline.[5][6] If the board is completely filled and no one can make a line of 5 stones, then the game ends in a draw.

### Freestyle gomoku
Freestyle gomoku has no restrictions on either player and allows a player to win by creating a line of five or more stones, with each player alternating turns placing one stone at a time.

## Projects

### Seedwork
This project contains reusable classes and components to implement value-objects especially when doing domain-driven design
<br/>

### Domain
Contains business entities relating to database persistence.
<br/>

### Infrastructure
This contains dependencies that are external in nature -- such as database persistence, email, message queues and pipelines etc.
<br/>

### Application
Clean architecture -- this is an overkill but chose to include anyway with guard-clauses, validation and logging.

## Local Setup

Run the following on the terminal.

#### Adding Migration
<br/>

In the sample below, `Initial` is the named instance of the migration.

```
dotnet ef migrations add 
/usr/local/share/dotnet/dotnet ef migrations add --project Gomoku.Infrastructure/Gomoku.Infrastructure.csproj --startup-project Gomoku-WebApp/Gomoku-WebApp.csproj --context Gomoku.Infrastructure.GameContext --configuration Debug testst --output-dir Migrations
```

#### Update Database
<br/>

In the script below, `20221214123342_Initial` is the target migration version.

```
/usr/local/share/dotnet/dotnet ef database update --project Gomoku.Infrastructure/Gomoku.Infrastructure.csproj --startup-project Gomoku-WebApp/Gomoku-WebApp.csproj --context Gomoku.Infrastructure.GameContext --configuration Debug 20231008043338_Initial
```

#### Legend

`--project migration.csproj`:  project w/c contains the DbContext<br/>
`--startup-project`: start-up project <br/>
`--context` : full name of the context (in the migration.csproj) <br/>
`--configuration`: Debug (or Release) etc. <br/>
`--output-dir`: the path where the migration classes be dumped into.
<br/>
<br/>

#### ON RIDER (I am using JETBRAINS RIDER)

Why I can't see my project in a `Startup projects` field?
If you can't see your project in the "Startup project" dropdown, check that your project satisfies the requirements:
`Microsoft.EntityFrameworkCore.Design` or `Microsoft.EntityFrameworkCore.Tools` NuGet package is installed.
Project's TargetFramework is at least `netcoreapp3.1`.
<br/><br/>

#### ON VISUAL STUDIO

Make sure that the start-up project references `Microsoft.EntityFrameworkCore.Design` and/or `Microsoft.EntityFrameworkCore.Tools`

#### Connection String

Important to note that `TrustServerCertificate=true;MultiSubNetFailover=True` must be set if using `Docker` instance of SQL Server

```
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=taaldb;User Id=xxx;Password=xxxxxx;TrustServerCertificate=True;MultiSubnetFailover=True;",
    "DevConnection":"DataSource=app.db;Cache=Shared"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}

```

### Setting up (on Mac)
<br/>

#### Docker
See link: https://www.twilio.com/blog/using-sql-server-on-macos-with-docker

<br/>

#### Snippets
<br/>


```
docker run -d --name sql_server -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=someThingComplicated1234' -p 1433:1433 mcr.microsoft.com/mssql/server:2019-latest

```
<br/>

`-d` will launch the container in `"detached"` mode and is optional. This means that containers will run in the background and you can close the terminal window.

`--name` sql_server will assign a name to the container and is optional, but recommended for easier management!

`-e` will allow you to set environment variables:
`'ACCEPT_EULA=Y'` SQL Server requires the user to accept the "End User Licence Agreement" or EULA. The Y here indicates acceptance.

`'SA_PASSWORD=someThingComplicated1234'` is a required parameter for SQL Server.This is the System Administrator password.  <strong><br/>See the note below on password strength.</strong>

`-p 1433:1433` will map the local port 1433 to port 1433 on the container. Port `1433` is the default TCP port that SQL Server will listen on.

`mcr.microsoft.com/mssql/server:2019-latest` is the image we wish to run. I have used the latest version of 2019, however, if you need a different version you can check out the Microsoft SQL Server page on Docker Hub.

<br/>


### Note on Password Strength
If you find your image starts but then immediately stops or you get an error such as setup failed with error code 1`, then it may be you haven't created a strong enough password. SQL Server really means it when it requests a strong password. Ensure good length with a mixture of upper and lower case, and a mix of alphanumeric characters. For more information on password requirements take a look at the Microsoft documentation.

## ENDPOINTS

### POST /api/game
#### REQUEST BODY

```
{
  "player_one": "P1",
  "player_two": "P2"
}

```

#### RESPONSE (SUCCESS)

Color legend:
`public enum Pebbles
{
White = 0,
Black = 1,
Empty = 2
}`
```
{
  "error_message": "",
  "is_success": true,
  "data": [
    {
      "gameId": "ed34d543-ef7b-4a60-5b99-08dbc7f546c1",
      "board": [
        {
          "row": 0,
          "column": 0,
          "color": 2
        },
        {
          "row": 0,
          "column": 1,
          "color": 2
        },
        {
          "row": 0,
          "column": 2,
          "color": 2
        },
       ... Concatenated for brevity
      ]
    }
  ]
}

```

### GET api/game/{id}

Only use this to decide w/c player goes first (or as a cheat to have consecutive turns - for testing winner scenarios.)

#### REQUEST QUERY
`api/Game/ed34d543-ef7b-4a60-5b99-08dbc7f546c1`

#### RESPONSE BODY

Use Game Id and PlayerId for the next endpoint.
```
{
  "error_message": "",
  "is_success": true,
  "data": [
    {
      "gameId": "ed34d543-ef7b-4a60-5b99-08dbc7f546c1",
      "playerId": "c198b99d-0b80-4bcc-2dfc-08dbc7f546cd",
      "name": "P1",
      "color": 1
    }
  ]
}
```

### PUT api/game

Disclaimer: Did not really follow thru with the HTTP methods invoke

This is to update or in the game's jargon -- to attack or to put pebbles on the board.

#### REQUEST QUERY
`api/Game/{gameid}?row={row}&column={column}&playerId={playerId}`
`api/Game/ed34d543-ef7b-4a60-5b99-08dbc7f546c1?row=5&column=3&playerId=c198b99d-0b80-4bcc-2dfc-08dbc7f546cd`

### RESPONSE
This response is big - it has the updated board, the players, the current player (turn), the winner (if gameover) etc.. that can be used for displaying/rendering info.
Use the info from below to do request again until a winner emerge (playerId and gameId == choose row and column)

```
{
  "error_message": "",
  "is_success": true,
  "data": [
    {
      "data": [
        {
          "winner": null,
          "board": [
            {
              "row": 0,
              "column": 0,
              "color": 2,
              "id": "e9dbb474-0ab2-4dd1-b9fc-08dbc7f546c8"
            },
            {
              "row": 0,
              "column": 1,
              "color": 2,
              "id": "e9890325-21d7-4af4-b9fd-08dbc7f546c8"
            },
            ... Concatenated for brevity
          ],
          "playerTurn": {
            "gameId": "ed34d543-ef7b-4a60-5b99-08dbc7f546c1",
            "playerId": "4437901a-fdbc-4f09-2dfd-08dbc7f546cd",
            "name": "string",
            "color": 0
          },
          "gameId": "ed34d543-ef7b-4a60-5b99-08dbc7f546c1",
          "players": [
            {
              "name": "P1",
              "color": 1,
              "id": "c198b99d-0b80-4bcc-2dfc-08dbc7f546cd"
            },
            {
              "name": "string",
              "color": 0,
              "id": "4437901a-fdbc-4f09-2dfd-08dbc7f546cd"
            }
          ]
        }
      ]
    }
  ]
}
```

NOTE: This can still be further improved. Less is more.

