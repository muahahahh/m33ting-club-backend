create table if not exists "app"."participant" (
    "meeting_id" uuid,
    "user_id" uuid,
    "meeting_role" varchar(63),
    primary key ("meeting_id", "user_id"),
    constraint "fk_meeting"
        foreign key ("meeting_id")
        references "app"."meeting"("id")
);

create view "app"."v_participant" as (
    select 
        "meeting_id",
        "user_id",
        "meeting_role"
    from "app"."participant"
);
                                    
