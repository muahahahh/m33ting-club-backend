drop view "app"."v_user";

create view "app"."v_user" as (
  select
      "user"."id" as "id",
      "user"."firebase_id" as "firebase_id",
      "user"."first_name" as "first_name",
      "user"."last_name" as "last_name",
      "user"."birthday" as "birthday",
      "user"."gender" "gender"
  from "app"."user");