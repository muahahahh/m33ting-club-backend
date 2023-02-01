create table "app"."job_history"(
    "id" uuid,
    "name" varchar(255) not null,
    "start_date" timestamptz not null,
    "end_date" timestamptz null
);