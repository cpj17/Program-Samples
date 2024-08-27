-- retrieve information about table columns, including their names, data types, nullability, and primary key status. Here's how you can do this:

-- Retrieve Column Information: To get information about columns in a particular table, you can query the INFORMATION_SCHEMA.COLUMNS view. This view provides details about column names, data types, and nullability.

-- Retrieve Primary Key Information: To determine if a column is part of a primary key, you can query the INFORMATION_SCHEMA.KEY_COLUMN_USAGE view in combination with INFORMATION_SCHEMA.TABLE_CONSTRAINTS.

SELECT 
    c.COLUMN_NAME,
    c.DATA_TYPE,
    CASE 
        WHEN c.IS_NULLABLE = 'YES' THEN 'YES'
        ELSE 'NO'
    END AS IS_NULLABLE,
    CASE 
        WHEN pk.COLUMN_NAME IS NOT NULL THEN 'YES'
        ELSE 'NO'
    END AS IS_PRIMARY_KEY
FROM 
    INFORMATION_SCHEMA.COLUMNS c
LEFT JOIN 
    (
        SELECT 
            ku.COLUMN_NAME
        FROM 
            INFORMATION_SCHEMA.KEY_COLUMN_USAGE ku
        JOIN 
            INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
        ON 
            ku.CONSTRAINT_NAME = tc.CONSTRAINT_NAME
        WHERE 
            tc.CONSTRAINT_TYPE = 'PRIMARY KEY'
        AND 
            ku.TABLE_NAME = 'YourTableName' -- Replace with your table name
    ) pk
ON 
    c.COLUMN_NAME = pk.COLUMN_NAME
WHERE 
    c.TABLE_NAME = 'YourTableName'; -- Replace with your table name
