drop view if exists "app"."v_meeting";
create view "app"."v_meeting" as (
     select
         "meeting"."id" as "id",
         "meeting"."name" as "name",
         "meeting"."description" as "description",
         "meeting"."participants_limit" as "participants_limit",
         "meeting"."is_public" as "is_public",
         "meeting"."picture" as "picture",
         "meeting"."localization_name" as "localization_name",
         "meeting"."localization_coordinates"[0] as "latitude",
         "meeting"."localization_coordinates"[1] as "longitude",
         "meeting_tag"."tag_names" as tag_names
     from "app"."meeting"
     left join (
                select "meeting_id", array_agg("tag_name") as "tag_names"
                from "app"."meeting_tag"
                group by "meeting_id"
               ) as "meeting_tag" on "app"."meeting".id = "meeting_tag"."meeting_id"     
);
