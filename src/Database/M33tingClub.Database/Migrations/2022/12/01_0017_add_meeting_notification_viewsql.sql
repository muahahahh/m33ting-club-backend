create or replace view "app"."v_meeting_notification" as
(
select "meeting_notification"."id"          as "id",
	   "meeting_notification"."type"        as "type",
	   "meeting_notification"."meeting_id"  as "meeting_id",
	   "meeting_notification"."receiver_id" as "receiver_id",
	   "meeting_notification"."was_seen"    as "was_seen",
	   "meeting_notification"."occured_on"  as "occured_on",
	   "performer"."id"                     as "performer_id",
	   "performer"."name"                   as "performer_name"
from "app"."meeting_notification" "meeting_notification"
		 inner join "app"."user" "performer"
					on "meeting_notification"."performer_id" = "performer"."id"
	)