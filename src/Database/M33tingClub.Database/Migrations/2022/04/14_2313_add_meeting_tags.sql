create table if not exists "app"."meeting_tag" (
    "meeting_id" uuid,
    "tag_name" varchar(255),
    primary key ("meeting_id", "tag_name"),
    constraint "fk_meeting"
        foreign key ("meeting_id")
        references "app"."meeting"("id")
);

create view "app"."v_meeting_tag" as (
    select 
        "meeting_id",
        "tag_name"
    from "app"."meeting_tag"
);

drop view if exists "app"."v_meeting";
create view "app"."v_meeting" as (
    select
        "meeting"."id" as "id",
        "meeting"."name" as "name",
        "meeting"."description" as "description",
        "meeting"."participants_limit" as "participants_limit",
        "meeting"."is_public" as "is_public",
        "meeting"."picture" as "picture",
        "meeting"."localization_name" as "location_name",
        "meeting"."localization_coordinates" as "localization_coordinates"
    from "app"."meeting"
 );
