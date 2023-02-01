create or replace view "app"."v_followership" as (
	select
		"follower"."id" as "follower_id",
		"follower"."name" as "follower_name",
		"follower"."image_id" as "follower_image_id",
		"following"."id" as "following_id",
		"following"."name" as "following_name",
		"following"."image_id" as "following_image_id",
		"followership"."created_at" as "created_at"
	from "app"."followership" "followership"
	inner join "app"."user" "follower" on "followership"."follower_id" = "follower"."id"
	inner join "app"."user" "following" on "followership"."following_id" = "following"."id"
);