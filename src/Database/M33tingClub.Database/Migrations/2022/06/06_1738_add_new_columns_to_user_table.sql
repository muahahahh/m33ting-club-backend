alter table "app"."user"
    add column "first_name" varchar(255) not null;

alter table "app"."user"
    add column "last_name" varchar(255) not null;

alter table "app"."user"
    add column "birthday" date not null;

alter table "app"."user"
    add column "gender" varchar(10) not null;