alter table "app"."meeting"
    add column "start_date" timestamptz null;

alter table "app"."meeting"
    add column "end_date" timestamptz null;

update "app"."meeting"
set "start_date" = current_date + 1;

update "app"."meeting"
set "end_date" = current_date + 2;

alter table "app"."meeting"
    alter column "start_date" set not null;

alter table "app"."meeting"
alter column "end_date" set not null;