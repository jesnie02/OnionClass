-- Opret schema hvis det ikke findes
DO $$
    BEGIN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'surveillance') THEN
            CREATE SCHEMA surveillance;
        END IF;
    END $$;

-- Opret product tabel
CREATE TABLE IF NOT EXISTS surveillance.product (
                                                    id SERIAL PRIMARY KEY,
                                                    name TEXT NOT NULL,
                                                    price DECIMAL
);

-- Opret user tabel
CREATE TABLE IF NOT EXISTS surveillance.user (
                                                 id TEXT PRIMARY KEY,
                                                 email TEXT NOT NULL,
                                                 hash TEXT NOT NULL,
                                                 salt TEXT NOT NULL,
                                                 role TEXT NOT NULL
);

-- Opret devicelog tabel
CREATE TABLE IF NOT EXISTS surveillance.devicelog (
                                                      id TEXT PRIMARY KEY,
                                                      deviceid TEXT NOT NULL,
                                                      value DOUBLE PRECISION NOT NULL,
                                                      timestamp TIMESTAMPTZ NOT NULL,
                                                      unit TEXT NOT NULL
);