
alter table app.participant
    add constraint fk_user
        foreign key (user_id)
            references app.user(id)
            on delete cascade ;

alter table app.application
    add constraint fk_user
        foreign key (user_id)
            references app.user(id)
            on delete cascade ;