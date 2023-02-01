create table if not exists "app"."user" (
    "id" uuid,
    "firebase_id" varchar(1000) unique,
    primary key ("id")
    )