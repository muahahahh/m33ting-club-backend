create or replace view "app"."v_tag" as (
    select 
       "name" as "name", 
       "is_official" as "is_official" 
    from "app"."tag");

create or replace view "app"."v_all_tags" as(
    select 
       "v_tag"."name" as "name", 
       "v_tag"."is_official" as "is_official"
    from "app"."v_tag" as "v_tag"
             left join (
        select
            "tag_name" as "name",
            count("tag_name") as "number_of_meetings"
        from "app"."v_meeting_tag"
        group by "tag_name") as "v_meeting_tag" on "v_tag"."name" = "v_meeting_tag"."name"
    order by coalesce("v_meeting_tag"."number_of_meetings", 0) desc 
);
	