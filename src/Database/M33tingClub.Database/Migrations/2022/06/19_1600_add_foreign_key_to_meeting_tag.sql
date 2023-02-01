
alter table "app"."meeting_tag"
add constraint "fk_tag"
    foreign key ("tag_name")
        references "app"."tag"("name");
