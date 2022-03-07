BEGIN TRANSACTION [Migration]
BEGIN TRY
	
	ALTER TABLE RefreshTokens 
		ADD [CreatedAt] AS GETDATE();

	COMMIT TRANSACTION[Migration];
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION [Migration];
	SELECT
       ERROR_NUMBER() AS ErrorNumber,
       ERROR_SEVERITY() AS ErrorSeverity,
       ERROR_STATE() AS ErrorState,
       ERROR_PROCEDURE() AS ErrorProcedure,
       ERROR_LINE() AS ErrorLine,
       ERROR_MESSAGE() AS ErrorMessage;
END CATCH