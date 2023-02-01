create or replace view "app"."v_feed" as
(
select
	"user"."id" as "user_id",
	"user"."name" as "user_name",
	"user"."image_id" as "user_image_id",
	"participant"."meeting_role" as "meeting_role",
	"meeting"."id" as "meeting_id",
	"meeting"."name" as "meeting_name",
	"meeting"."image_id" as "meeting_image_id",
	"participant"."joined_date" as "occured_on"
from "app"."user"
		 inner join "app"."participant" on "user"."id" = "participant"."user_id"
		 inner join "app"."meeting" on "participant"."meeting_id" = "meeting"."id"
	)