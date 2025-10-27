-- Run these scripts in pgAdmin in this specific order because of dependencies. 
-- Clear the tables (DELETE FROM ...) before running if you need to re-insert. 




------------------------------------------------ 01_Insert_Routes.sql ------------------------------------------------

-- Clear existing data (optional)
-- DELETE FROM "Routes";

-- Insert Routes and capture generated UUIDs for reference if needed later
DO $$
DECLARE
    dhaka_raj_route_id UUID := gen_random_uuid();
    dhaka_ctg_route_id UUID := gen_random_uuid();
    ctg_dhaka_route_id UUID := gen_random_uuid();
BEGIN
    INSERT INTO "Routes" ("Id", "From", "To") VALUES
    (dhaka_raj_route_id, 'Dhaka', 'Rajshahi'),
    (dhaka_ctg_route_id, 'Dhaka', 'Chittagong'),
    (ctg_dhaka_route_id, 'Chittagong', 'Dhaka');

    -- Outputting the generated IDs (optional, view in Messages tab in pgAdmin)
    RAISE NOTICE 'Dhaka -> Rajshahi Route ID: %', dhaka_raj_route_id;
    RAISE NOTICE 'Dhaka -> Chittagong Route ID: %', dhaka_ctg_route_id;
    RAISE NOTICE 'Chittagong -> Dhaka Route ID: %', ctg_dhaka_route_id;
END $$;

-- Verify
SELECT * FROM "Routes";

------------------------------------------------ 02_Insert_Stops.sql ------------------------------------------------

-- Clear existing data (optional)
-- DELETE FROM "Stops";

-- Insert Stops, linking them to Routes using subqueries
INSERT INTO "Stops" ("Id", "RouteId", "Name", "Type") VALUES
-- Dhaka-Rajshahi Stops (Type 0 = Boarding, Type 1 = Dropping)
(gen_random_uuid(), (SELECT "Id" FROM "Routes" WHERE "From" = 'Dhaka' AND "To" = 'Rajshahi' LIMIT 1), '[06:00 AM] Kallyanpur', 0),
(gen_random_uuid(), (SELECT "Id" FROM "Routes" WHERE "From" = 'Dhaka' AND "To" = 'Rajshahi' LIMIT 1), '[06:15 AM] Mohakhali', 0),
(gen_random_uuid(), (SELECT "Id" FROM "Routes" WHERE "From" = 'Dhaka' AND "To" = 'Rajshahi' LIMIT 1), '[10:30 AM] Baneshore', 1),
(gen_random_uuid(), (SELECT "Id" FROM "Routes" WHERE "From" = 'Dhaka' AND "To" = 'Rajshahi' LIMIT 1), '[12:30 PM] Rajshahi', 1),
(gen_random_uuid(), (SELECT "Id" FROM "Routes" WHERE "From" = 'Dhaka' AND "To" = 'Rajshahi' LIMIT 1), '[01:00 PM] Rajabari', 1),

-- Dhaka-Chittagong Stops
(gen_random_uuid(), (SELECT "Id" FROM "Routes" WHERE "From" = 'Dhaka' AND "To" = 'Chittagong' LIMIT 1), '[07:00 AM] Sayedabad', 0),
(gen_random_uuid(), (SELECT "Id" FROM "Routes" WHERE "From" = 'Dhaka' AND "To" = 'Chittagong' LIMIT 1), '[07:30 AM] Arambagh', 0),
(gen_random_uuid(), (SELECT "Id" FROM "Routes" WHERE "From" = 'Dhaka' AND "To" = 'Chittagong' LIMIT 1), '[11:00 AM] Comilla', 1),
(gen_random_uuid(), (SELECT "Id" FROM "Routes" WHERE "From" = 'Dhaka' AND "To" = 'Chittagong' LIMIT 1), '[01:00 PM] Chittagong GEC', 1),

-- Chittagong-Dhaka Stops
(gen_random_uuid(), (SELECT "Id" FROM "Routes" WHERE "From" = 'Chittagong' AND "To" = 'Dhaka' LIMIT 1), '[08:00 AM] Chittagong BRTC', 0),
(gen_random_uuid(), (SELECT "Id" FROM "Routes" WHERE "From" = 'Chittagong' AND "To" = 'Dhaka' LIMIT 1), '[08:30 AM] Dampara', 0),
(gen_random_uuid(), (SELECT "Id" FROM "Routes" WHERE "From" = 'Chittagong' AND "To" = 'Dhaka' LIMIT 1), '[12:00 PM] Feni', 1),
(gen_random_uuid(), (SELECT "Id" FROM "Routes" WHERE "From" = 'Chittagong' AND "To" = 'Dhaka' LIMIT 1), '[02:00 PM] Dhaka Sayedabad', 1);

-- Verify
SELECT * FROM "Stops";

------------------------------------------------ 03_Insert_Buses.sql ------------------------------------------------

-- Clear existing data (optional)
-- DELETE FROM "Buses";

INSERT INTO "Buses" ("Id", "CompanyName", "BusName", "TotalSeats", "CancellationPolicy") VALUES
(gen_random_uuid(), 'National Travels', '101 - Non AC', 40, '4 hours before departure'),
(gen_random_uuid(), 'National Travels', '102 - AC Business', 36, '6 hours before departure, 10% charge'),
(gen_random_uuid(), 'Hanif Enterprise', 'H-501 Scania AC', 36, 'No cancellation allowed'),
(gen_random_uuid(), 'Hanif Enterprise', 'H-205 Non AC', 40, '2 hours before departure'),
(gen_random_uuid(), 'Green Line', 'GL-Volvo B11R', 32, '12 hours before departure'),
(gen_random_uuid(), 'Green Line', 'GL-Hino AC', 36, '12 hours before departure'),
(gen_random_uuid(), 'Shohagh Paribahan', 'SP-Non AC', 40, '6 hours before departure'),
(gen_random_uuid(), 'Ena Transport', 'ENA-Hyundai AC', 36, 'Cancel anytime, 5% charge'),
(gen_random_uuid(), 'Ena Transport', 'ENA-Hino Non AC', 40, 'Cancel anytime, 5% charge'),
(gen_random_uuid(), 'Saintmartin Hyundai', 'SM-Hyundai Universe AC', 28, '24 hours notice required');

-- Verify
SELECT * FROM "Buses";

------------------------------------------------ 04_Insert_BusSchedules.sql ------------------------------------------------

-- Clear existing data (optional)
-- DELETE FROM "BusSchedules";

INSERT INTO "BusSchedules" (
    "Id", "BusId", "RouteId", "DepartureTime", "ArrivalTime", "Price",
    "DepartureLocation", "ArrivalLocation",
    "ServiceCharge", "PGWCharge", "Discount"
) VALUES
-- Dhaka-Rajshahi Schedules (Use correct TotalSeats from Buses table for consistency)
(gen_random_uuid(), (SELECT "Id" FROM "Buses" WHERE "BusName" = '101 - Non AC' LIMIT 1), (SELECT "Id" FROM "Routes" WHERE "From" = 'Dhaka' AND "To" = 'Rajshahi' LIMIT 1), '2025-10-23 06:00:00+06', '2025-10-23 13:30:00+06', 700, 'Kallyanpur', 'Rajshahi', 20, 28, 48),
(gen_random_uuid(), (SELECT "Id" FROM "Buses" WHERE "BusName" = 'H-205 Non AC' LIMIT 1), (SELECT "Id" FROM "Routes" WHERE "From" = 'Dhaka' AND "To" = 'Rajshahi' LIMIT 1), '2025-10-23 07:00:00+06', '2025-10-23 14:30:00+06', 750, 'Gabtoli', 'Natore Bypass', 25, 30, 0),
(gen_random_uuid(), (SELECT "Id" FROM "Buses" WHERE "BusName" = '102 - AC Business' LIMIT 1), (SELECT "Id" FROM "Routes" WHERE "From" = 'Dhaka' AND "To" = 'Rajshahi' LIMIT 1), '2025-10-23 09:00:00+06', '2025-10-23 15:00:00+06', 1200, 'Mohakhali', 'Rajshahi', 50, 40, 100),
-- Add another Dhaka-Rajshahi schedule for variety
(gen_random_uuid(), (SELECT "Id" FROM "Buses" WHERE "BusName" = 'SP-Non AC' LIMIT 1), (SELECT "Id" FROM "Routes" WHERE "From" = 'Dhaka' AND "To" = 'Rajshahi' LIMIT 1), '2025-10-23 10:00:00+06', '2025-10-23 17:00:00+06', 720, 'Kallyanpur', 'Rajshahi', 20, 28, 10),


-- Dhaka-Chittagong Schedules
(gen_random_uuid(), (SELECT "Id" FROM "Buses" WHERE "BusName" = 'GL-Volvo B11R' LIMIT 1), (SELECT "Id" FROM "Routes" WHERE "From" = 'Dhaka' AND "To" = 'Chittagong' LIMIT 1), '2025-10-24 08:00:00+06', '2025-10-24 13:00:00+06', 1500, 'Arambagh', 'Chittagong GEC', 60, 50, 0),
(gen_random_uuid(), (SELECT "Id" FROM "Buses" WHERE "BusName" = 'SP-Non AC' LIMIT 1), (SELECT "Id" FROM "Routes" WHERE "From" = 'Dhaka' AND "To" = 'Chittagong' LIMIT 1), '2025-10-24 09:30:00+06', '2025-10-24 15:30:00+06', 650, 'Sayedabad', 'Comilla', 15, 25, 0),
(gen_random_uuid(), (SELECT "Id" FROM "Buses" WHERE "BusName" = 'ENA-Hyundai AC' LIMIT 1), (SELECT "Id" FROM "Routes" WHERE "From" = 'Dhaka' AND "To" = 'Chittagong' LIMIT 1), '2025-10-24 11:00:00+06', '2025-10-24 16:00:00+06', 1100, 'Mohakhali', 'Chittagong GEC', 40, 35, 50),

-- Chittagong-Dhaka Schedules
(gen_random_uuid(), (SELECT "Id" FROM "Buses" WHERE "BusName" = 'SM-Hyundai Universe AC' LIMIT 1), (SELECT "Id" FROM "Routes" WHERE "From" = 'Chittagong' AND "To" = 'Dhaka' LIMIT 1), '2025-10-25 07:00:00+06', '2025-10-25 12:30:00+06', 1800, 'Dampara', 'Dhaka Sayedabad', 70, 60, 150),
(gen_random_uuid(), (SELECT "Id" FROM "Buses" WHERE "BusName" = 'ENA-Hino Non AC' LIMIT 1), (SELECT "Id" FROM "Routes" WHERE "From" = 'Chittagong' AND "To" = 'Dhaka' LIMIT 1), '2025-10-25 09:00:00+06', '2025-10-25 15:00:00+06', 600, 'Chittagong BRTC', 'Feni', 10, 20, 0),
(gen_random_uuid(), (SELECT "Id" FROM "Buses" WHERE "BusName" = 'Green Line - GL-Hino AC' LIMIT 1), (SELECT "Id" FROM "Routes" WHERE "From" = 'Chittagong' AND "To" = 'Dhaka' LIMIT 1), '2025-10-25 10:00:00+06', '2025-10-25 15:30:00+06', 1300, 'Dampara', 'Arambagh', 55, 45, 0);

-- Verify
SELECT * FROM "BusSchedules";

------------------------------------------------ 05_Insert_Seats.sql ------------------------------------------------

-- Clear existing data (optional)
-- DELETE FROM "Seats";

-- Loop through all schedules and insert the appropriate number of seats for each bus
DO $$
DECLARE
    schedule_record RECORD; 
    bus_record RECORD; 
    seat_prefix CHAR(1);
    seat_num INT;
    total_seats_for_bus INT;
    rows_needed INT;
    cols_per_row INT := 4;
BEGIN
    -- Loop through each schedule to get its ID and the associated BusId
    FOR schedule_record IN SELECT "Id", "BusId" FROM "BusSchedules" LOOP

        -- Find the TotalSeats for the bus associated with this schedule
        SELECT "TotalSeats" INTO total_seats_for_bus
        FROM "Buses"
        WHERE "Id" = schedule_record."BusId"
        LIMIT 1;

        IF total_seats_for_bus IS NULL THEN
            RAISE NOTICE 'Bus not found for Schedule ID: %, skipping seat insertion.', schedule_record."Id";
            CONTINUE; 
        END IF;

        RAISE NOTICE 'Inserting % seats for Schedule ID: %', total_seats_for_bus, schedule_record."Id";

        -- Calculate number of rows needed (e.g., 40 seats / 4 per row = 10 rows)
        rows_needed := CEIL(total_seats_for_bus::decimal / cols_per_row);
        IF rows_needed > 26 THEN rows_needed := 26; END IF;

        -- Loop to create rows (A, B, C...)
        FOR row_num IN 1..rows_needed LOOP
            seat_prefix := CHR(64 + row_num); 

            -- Loop to create columns (1, 2, 3, 4)
            FOR col_num IN 1..cols_per_row LOOP
                -- Check if we've inserted enough seats already
                IF (row_num - 1) * cols_per_row + col_num > total_seats_for_bus THEN
                    EXIT; 
                END IF;

                seat_num := col_num;
                -- Insert the seat for the current schedule_record.Id
                INSERT INTO "Seats" ("Id", "BusScheduleId", "SeatNumber", "Status") VALUES
                (gen_random_uuid(), schedule_record."Id", seat_prefix || seat_num, 0);
            END LOOP; 

             -- Check again if total seats reached after inner loop finishes
            IF (row_num - 1) * cols_per_row + cols_per_row >= total_seats_for_bus THEN
                 EXIT;
            END IF;

        END LOOP;

    END LOOP;
    RAISE NOTICE 'Seat insertion complete.';
END $$;

-- Verify (Check count for one schedule)
-- SELECT COUNT(*) FROM "Seats" WHERE "BusScheduleId" = (SELECT "Id" FROM "BusSchedules" LIMIT 1);

