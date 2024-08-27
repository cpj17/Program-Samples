BEGIN try
    BEGIN TRANSACTION;
    INSERT INTO tbl1
                (id,
                 NAME,
                 desc)
    VALUES      (1,
                 'name 1',
                 'desc 1');

    COMMIT TRANSACTION;
END try

BEGIN catch
    IF @@TRANCOUNT > 0
      ROLLBACK TRANSACTION;

    DECLARE @ErrorNumber INT = Error_number();
    DECLARE @ErrorLine INT = Error_line();
    DECLARE @ErrorMessage NVARCHAR(4000) = Error_message();

    PRINT 'Actual error number: '
          + Cast(@ErrorNumber AS VARCHAR(10));

    PRINT 'Actual line number: '
          + Cast(@ErrorLine AS VARCHAR(10));

    RAISERROR(@ErrorMessage,@ErrorNumber,@ErrorLine);
END catch 