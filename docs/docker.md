# Running app in docker

```shell
docker-compose up
```

App will listen on port `http://localhost:8080/swagger/index.html`.

Remember to run migrations before using app: [Migrations](migrations.md)

Given compose file provides also `PostgreSQL` server, it will run on port `5432`.