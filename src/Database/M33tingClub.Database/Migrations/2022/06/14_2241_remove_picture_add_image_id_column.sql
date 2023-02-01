
alter table "app"."tag"
    add column "image_id" uuid null;

drop view "app"."v_meeting";

alter table "app"."meeting"
    drop column "picture";

alter table "app"."meeting"
    add column "image_id" uuid;


create view "app"."v_meeting" as (
     select
         "meeting"."id" as "id",
         "meeting"."name" as "name",
         "meeting"."description" as "description",
         "meeting"."participants_limit" as "participants_limit",
         "meeting"."start_date" as "start_date",
         "meeting"."end_date" as "end_date",
         "meeting"."image_id" as "image_id",
         "meeting"."location_name" as "location_name",
         "meeting"."location_description" as "location_description",
         "meeting"."location_coordinates" as "location_coordinates",
         "meeting"."status" as "status",
         "meeting_tag"."tag_names" as "tag_names"
     from "app"."meeting"
              left join (
         select "meeting_id", array_agg("tag_name") as "tag_names"
         from "app"."meeting_tag"
         group by "meeting_id"
     ) as "meeting_tag" on "app"."meeting".id = "meeting_tag"."meeting_id"
         );


