create table "app"."followership"(
	"follower_id" uuid not null,
	"following_id" uuid not null,
	"created_at" timestamptz not null,
	primary key ("follower_id", "following_id"),
	constraint "fk_follower"
	 	foreign key ("follower_id")
		references "app"."user"("id"),
	constraint "fk_following"
	 	foreign key ("following_id")
		references "app"."user"("id")
);