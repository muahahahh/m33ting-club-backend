drop view "app"."v_user";

alter table "app"."user"
add column is_deleted boolean not null default false;

create view "app"."v_user" as (
select
  "user"."id" as "id",
  "user"."firebase_id" as "firebase_id",
  "user"."name" as "name",
  "user"."birthday" as "birthday",
  "user"."gender" as "gender",
  "user"."image_id" as "image_id",
  "user"."phone_number" as "phone_number",
  "user"."is_deleted" as "is_deleted"
from "app"."user");

