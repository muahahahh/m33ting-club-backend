create table if not exists "app"."meeting_tag" (
    "meeting_id" uuid,
    "tag_name" varchar(255),
    primary key ("meeting_id", "tag_name"),
    constraint "fk_meeting"
       foreign key ("meeting_id")
           references "app"."meeting"("id")
);
