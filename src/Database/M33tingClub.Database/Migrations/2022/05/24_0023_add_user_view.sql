
create view "app"."v_user" as (
  select
      "user"."id" as "id",
      "user"."firebase_id" as "firebase_id"
  from "app"."user")

