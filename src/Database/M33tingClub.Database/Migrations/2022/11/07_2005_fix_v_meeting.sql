create or replace view "app"."v_meeting" as
(
select "meeting"."id"                   as "id",
	   "meeting"."name"                 as "name",
	   "meeting"."description"          as "description",
	   "meeting"."participants_limit"   as "participants_limit",
	   "meeting"."start_date"           as "start_date",
	   "meeting"."end_date"             as "end_date",
	   "meeting"."image_id"             as "image_id",
	   "meeting"."location_name"        as "location_name",
	   "meeting"."location_description" as "location_description",
	   "meeting"."location_coordinates" as "location_coordinates",
	   "meeting"."status"               as "status",
	   "meeting_tag"."tag_names"        as "tag_names",
	   "participant"."participant"      as "participants",
	   "meeting"."is_public"            as "is_public"
from "app"."meeting"
		 left join (
	select "meeting_id", array_agg("tag_name") as "tag_names"
	from "app"."meeting_tag"
	group by "meeting_id"
) as "meeting_tag" on "app"."meeting".id = "meeting_tag"."meeting_id"
		 inner join (
	select "meeting_id",
		   json_agg(json_build_object('user_id', "user_id", 'name', "name", 'image_id', "image_id", 'meeting_role', "meeting_role")) as "participant"
	from "app"."v_participant"
	group by "meeting_id"
) as "participant" on "app"."meeting".id = "participant"."meeting_id"
	)