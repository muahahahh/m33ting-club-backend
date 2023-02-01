create or replace view "app"."v_tag" as (
select
	"name" as "name",
	"is_official" as "is_official"
from "app"."tag");