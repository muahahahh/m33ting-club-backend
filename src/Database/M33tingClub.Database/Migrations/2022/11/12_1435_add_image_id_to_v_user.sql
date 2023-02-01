﻿create or replace view "app"."v_user" as (
  select
	  "user"."id" as "id",
	  "user"."firebase_id" as "firebase_id",
	  "user"."name" as "name",
	  "user"."birthday" as "birthday",
	  "user"."gender" as "gender",
	  "user"."image_id" as "image_id"
  from "app"."user");