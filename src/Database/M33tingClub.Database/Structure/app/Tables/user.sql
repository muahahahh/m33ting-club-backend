create table if not exists "app"."user" (
   "id" uuid,
   "firebase_id" varchar(1000) unique,
   "name" varchar(255) not null,
   "birthday" date not null,
   "gender" varchar(10) not null,
   "image_id" uuid,
   "phone_number" varchar(15) not null unique,
   "is_deleted" boolean not null,
    primary key ("id")
);