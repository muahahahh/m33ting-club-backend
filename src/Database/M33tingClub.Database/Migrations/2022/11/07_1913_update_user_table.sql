drop view "app"."v_user";

drop view "app"."v_application";

drop view "app"."v_meeting";

drop view "app"."v_participant";

alter table "app"."user"
    drop column "last_name";

alter table "app"."user"
    rename column "first_name" to "name";

create view "app"."v_user" as (
  select
	  "user"."id" as "id",
	  "user"."firebase_id" as "firebase_id",
	  "user"."name" as "name",
	  "user"."birthday" as "birthday",
	  "user"."gender" "gender"
  from "app"."user");

create view "app"."v_application" as
(
select
	"application"."meeting_id",
	"application"."user_id",
	"application"."status",
	"user"."name",
	"user"."image_id"
from "app"."application" "application"
		 inner join "app"."user" "user" on "application"."user_id" = "user"."id"
	);

create view "app"."v_participant"
as
(
select "participant"."meeting_id"   as "meeting_id",
	   "participant"."user_id"      as "user_id",
	   "participant"."meeting_role" as "meeting_role",
	   "user"."name"          		as "name",
	   "user"."image_id"            as "image_id"
from "app"."participant" as "participant"
		 inner join "app"."user" as "user" on "participant"."user_id" = "user"."id"
	);

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
	   "participant"."participant"      as "participants"
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
