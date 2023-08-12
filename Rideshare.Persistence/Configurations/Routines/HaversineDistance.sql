-- Drop the table if it exists
DROP FUNCTION IF EXISTS public.haversine_distance;

CREATE FUNCTION public.haversine_distance(
    lon1 double precision,
    lat1 double precision,
    lon2 double precision,
    lat2 double precision
)
RETURNS double precision AS
$$
DECLARE
    R double precision := 6378.1; -- Earth's radius in kilometers
    dLat double precision := radians(lat2 - lat1);
    dLon double precision := radians(lon2 - lon1);
    a double precision := sin(dLat / 2) * sin(dLat / 2) + cos(radians(lat1)) * cos(radians(lat2)) * sin(dLon / 2) * sin(dLon / 2);
    c double precision := 2 * atan2(sqrt(a), sqrt(1 - a));
BEGIN
    RETURN R * c;
END;
$$
LANGUAGE plpgsql;