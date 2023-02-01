CREATE OR REPLACE VIEW "app"."v_participant"
AS
(
SELECT "participant"."meeting_id"   as "meeting_id",
	   "participant"."user_id"      as "user_id",
	   "participant"."meeting_role" as "meeting_role",
	   "participant"."joined_date"  as "joined_date",
	   "user"."name"          		as "name",
	   "user"."image_id"            as "image_id"
FROM "app"."participant" as "participant"
		 inner join "app"."user" as "user" on "participant"."user_id" = "user"."id"
	);