FROM postgres:14.2-alpine
COPY ./src/Database/M33tingClub.Database/Initialization/initialize.sql /docker-entrypoint-initdb.d/
