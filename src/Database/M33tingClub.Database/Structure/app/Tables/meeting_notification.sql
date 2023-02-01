create table "app"."meeting_notification"
(
	"id"           uuid         not null,
	"type"         varchar(255) not null,
	"meeting_id"   uuid         not null,
	"performer_id" uuid         not null,
	"receiver_id"  uuid         not null,
	"was_seen"     boolean      not null,
	"occured_on"   timestamptz  not null,
	primary key ("id")
);