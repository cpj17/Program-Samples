-- To retrieve information about the parameters of a stored procedure, including their names, data types, and lengths, you can query the system catalog views in SQL Server. Here’s how you can do it:

-- Using sys.parameters and sys.procedures

-- The sys.parameters catalog view provides information about parameters for stored procedures and functions, while the sys.procedures catalog view provides information about stored procedures.

SELECT 
    p.name AS ParameterName,
    t.name AS DataType,
    p.max_length AS Length,
    CASE 
        WHEN p.is_output = 1 THEN 'Output'
        ELSE 'Input'
    END AS ParameterType
FROM 
    sys.parameters AS p
INNER JOIN 
    sys.procedures AS sp ON p.object_id = sp.object_id
INNER JOIN 
    sys.types AS t ON p.user_type_id = t.user_type_id
WHERE 
    sp.name = 'YourStoredProcedureName'  -- Replace with your stored procedure name
ORDER BY 
    p.parameter_id;


-- Method 2 with null status

SELECT 
    p.name AS ParameterName,
    t.name AS DataType,
    p.max_length AS Length,
    CASE 
        WHEN p.is_output = 1 THEN 'Output'
        ELSE 'Input'
    END AS ParameterType,
    CASE 
        WHEN p.is_nullable = 1 THEN 'Nullable'
        ELSE 'Not Nullable'
    END AS Nullability
FROM 
    sys.parameters AS p
INNER JOIN 
    sys.procedures AS sp ON p.object_id = sp.object_id
INNER JOIN 
    sys.types AS t ON p.user_type_id = t.user_type_id
WHERE 
    sp.name = 'YourStoredProcedureName'  -- Replace with your stored procedure name
ORDER BY 
    p.parameter_id;


-- use this

SELECT 
    p.name AS ParameterName,
    t.name AS DataType,
    CASE 
        WHEN t.name IN ('decimal', 'numeric') THEN 
            CAST(p.precision AS VARCHAR) + ',' + CAST(p.scale AS VARCHAR)
        ELSE 
            CASE 
                WHEN p.max_length = -1 THEN 'MAX'
                ELSE CAST(p.max_length AS VARCHAR)
            END
    END AS LengthOrPrecisionScale,
    CASE 
        WHEN p.is_output = 1 THEN 'Output'
        ELSE 'Input'
    END AS ParameterType,
    CASE 
        WHEN p.is_nullable = 1 THEN 'Nullable'
        ELSE 'Not Nullable'
    END AS Nullability
FROM 
    sys.parameters AS p
INNER JOIN 
    sys.procedures AS sp ON p.object_id = sp.object_id
INNER JOIN 
    sys.types AS t ON p.user_type_id = t.user_type_id
WHERE 
    sp.name = 'YourStoredProcedureName'  -- Replace with your stored procedure name
ORDER BY 
    p.parameter_id;
