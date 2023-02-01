# Migrations

## Run command
```
./build MigrateDatabase --ConnectionString [your_Db_Connection_String]
```

## Run inside container

At first you need to build migrator docker container.
```
docker build -f migrator.Dockerfile -t m33tingclub-migrator .
```

Then you can run migrations. Remember to have Postgres running (in docker).
```
docker run --network m33ting-club-backend_m33ting-club-network  -it m33tingclub-migrator [your_Db_Connection_String] Migrations
```