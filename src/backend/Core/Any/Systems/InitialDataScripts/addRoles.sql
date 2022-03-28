BEGIN TRANSACTION [AddRoles]
BEGIN TRY

		INSERT INTO Roles ([Name])
			VALUES('User');
		INSERT INTO Roles ([Name])
			VALUES('ProtocoledUser');
		INSERT INTO Roles ([Name])
			VALUES('Admin');

	COMMIT TRANSACTION[AddRoles];
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION [AddRoles];
	SELECT
       ERROR_NUMBER() AS ErrorNumber,
       ERROR_SEVERITY() AS ErrorSeverity,
       ERROR_STATE() AS ErrorState,
       ERROR_PROCEDURE() AS ErrorProcedure,
       ERROR_LINE() AS ErrorLine,
       ERROR_MESSAGE() AS ErrorMessage;
END CATCH