create table if not exists "app"."participant" (
    "meeting_id" uuid,
    "user_id" uuid,
    "meeting_role" varchar(63),
	"joined_date" timestamptz not null,
    primary key ("meeting_id", "user_id"),
    constraint "fk_meeting"
        foreign key ("meeting_id")
        references "app"."meeting"("id")
);