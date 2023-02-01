create or replace view "app"."v_followership" as (
 select
	 "follower"."id" as "follower_id",
	 "follower"."name" as "follower_name",
	 "following"."id" as "following_id",
	 "following"."name" as "following_name",
	 "followership"."created_at" as "created_at"
 from "app"."followership" "followership"
		  inner join "app"."user" "follower" on "followership"."follower_id" = "follower"."id"
		  inner join "app"."user" "following" on "followership"."following_id" = "following"."id"
 );