CREATE OR REPLACE FUNCTION "app"."calculate_distance_m"(
    a point,
    b point)
    RETURNS numeric
    LANGUAGE 'sql'
    COST 100
    IMMUTABLE LEAKPROOF PARALLEL UNSAFE

    RETURN ((a <@> b) * (1609.344)::double precision);