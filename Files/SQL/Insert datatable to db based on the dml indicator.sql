CREATE TYPE DataTableType AS TABLE (
    user_id INT,
    post_id INT,
    unique_id INT,
    language_id INT,
    languages VARCHAR(100),
    is_read BIT,
    is_write BIT,
    is_speak BIT,
    dml_indicator CHAR(1)  -- Assuming dml_indicator is a single character ('I', 'U', 'D')
);
GO
-------------------------------------
CREATE PROCEDURE CreateNewTableFromDataTable
    @DataTable DataTableType READONLY  -- Parameter of type DataTableType
AS
BEGIN
    -- Drop the table if it already exists (optional, use with caution)
    IF OBJECT_ID('dbo.NewTable', 'U') IS NOT NULL
        DROP TABLE dbo.NewTable;

    -- Create the new table
    CREATE TABLE dbo.NewTable (
        user_id INT,
        post_id INT,
        unique_id INT,
        language_id INT,
        language VARCHAR(100),
        is_read BIT,
        is_write BIT,
        is_speak BIT,
        dml_indicator CHAR(1)  -- Include dml_indicator column in NewTable
    );

    -- Declare variables for processing
    DECLARE @UserID INT, @PostID INT, @LanguageID INT;
    DECLARE @Action CHAR(1), @ExistingRows INT;

    -- Cursor to iterate through @DataTable
    DECLARE cur CURSOR FOR
    SELECT user_id, post_id, language_id, dml_indicator
    FROM @DataTable;

    OPEN cur;
    FETCH NEXT FROM cur INTO @UserID, @PostID, @LanguageID, @Action;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF @Action = 'I'
        BEGIN
            -- Insert new row
            INSERT INTO dbo.NewTable (user_id, post_id, unique_id, language_id, language, is_read, is_write, is_speak, dml_indicator)
            SELECT user_id, post_id, unique_id, language_id, language, is_read, is_write, is_speak, dml_indicator
            FROM @DataTable
            WHERE user_id = @UserID AND post_id = @PostID AND language_id = @LanguageID;
        END
        ELSE IF @Action = 'U'
        BEGIN
            -- Update existing row
            UPDATE dbo.NewTable
            SET unique_id = dt.unique_id,
                language = dt.language,
                is_read = dt.is_read,
                is_write = dt.is_write,
                is_speak = dt.is_speak
            FROM dbo.NewTable nt
            JOIN @DataTable dt ON nt.user_id = dt.user_id AND nt.post_id = dt.post_id AND nt.language_id = dt.language_id
            WHERE nt.user_id = @UserID AND nt.post_id = @PostID AND nt.language_id = @LanguageID;
        END
        ELSE IF @Action = 'D'
        BEGIN
            -- Delete existing row
            DELETE FROM dbo.NewTable
            WHERE user_id = @UserID AND post_id = @PostID AND language_id = @LanguageID;
        END

        FETCH NEXT FROM cur INTO @UserID, @PostID, @LanguageID, @Action;
    END

    CLOSE cur;
    DEALLOCATE cur;

    PRINT 'New table dbo.NewTable created successfully with DML operations performed.';
END;
GO
-------------------------------
-- Declare and populate a table variable with your data
DECLARE @DataToProcess DataTableType;
INSERT INTO @DataToProcess (user_id, post_id, unique_id, language_id, language, is_read, is_write, is_speak, dml_indicator)
VALUES
    (1, 101, 1001, 1, 'English', 1, 0, 1, 'I'),  -- Insert operation
    (2, 102, 1002, 2, 'Spanish', 2, 1, 0, 'I'),  -- Update operation
    (3, 103, 1003, 3, 'French', 0, 0, I, 'D');  -- Delete operation

-- Execute the stored procedure with the table variable as parameter
EXEC CreateNewTableFromDataTable @DataTable = @DataToProcess;
