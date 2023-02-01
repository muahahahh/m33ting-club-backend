create table "app"."meeting"(
    "id" uuid,
    "name" varchar(255) not null,
    "description" varchar(255) null,
    "participants_limit" int null,
    "start_date" timestamptz not null,
    "end_date" timestamptz not null,
    "picture" varchar(255) null,
    "status" varchar(255),
    "location_name" varchar(255) not null,
    "location_description" varchar(255) not null,
    "location_coordinates" point not null,
	"is_public" bool not null,
	"confidential_info" varchar(255) null,
    primary key ("id")
);