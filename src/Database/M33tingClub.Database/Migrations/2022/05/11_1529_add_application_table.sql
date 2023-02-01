create table if not exists "app"."application"
(
    "meeting_id" uuid,
    "user_id"    uuid,
    "status"     varchar(63),
    primary key ("meeting_id", "user_id"),
    constraint "fk_meeting"
        foreign key ("meeting_id")
            references "app"."meeting" ("id")
);

--TODO: Add fk for user