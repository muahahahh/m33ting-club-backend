create schema if not exists app;

set search_path to app, public, "$user";

create table "app"."meeting" (
    "id" uuid,
    "name" varchar(255) not null,
    "description" varchar(255) null,
    "participants_limit" int null,
    "is_public" boolean not null,
    "picture" varchar(255) null,
    "localization_name" varchar(255) not null,
    "localization_coordinates" point not null,
    primary key("id")
);

create view "app"."v_meeting" as (
    select 
        "id",   
        "name" ,   
        "description",   
        "participants_limit",
        "is_public",  
        "picture",
        "localization_name",
        "localization_coordinates"
     from "app"."meeting"
 );