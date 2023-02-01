create view "app"."v_meeting_tag" as (
     select
         "meeting_id",
         "tag_name"
     from "app"."v_meeting_tag"
 );