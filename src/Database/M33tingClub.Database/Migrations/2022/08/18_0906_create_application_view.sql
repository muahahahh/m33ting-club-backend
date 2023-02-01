create or replace view "app"."v_application" as
(
select
    "application"."meeting_id",
    "application"."user_id",
    "application"."status",
    "user"."first_name",
    "user"."last_name",
    "user"."image_id"
    from "app"."application" "application"
    inner join "app"."user" "user" on "application"."user_id" = "user"."id"
);
