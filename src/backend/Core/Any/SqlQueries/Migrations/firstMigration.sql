DECLARE @tableName nvarchar(64) = N'Migrations'
DECLARE @firstMigrationName nvarchar(64) = N'Init'
BEGIN TRANSACTION [Init]
BEGIN TRY
	IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @tableName)
	BEGIN
		CREATE TABLE Migrations(
			[MigrationId] INT NOT NULL PRIMARY KEY IDENTITY(0, 1),
			[MigrationName] NVARCHAR (1024) NOT NULL);
	END;

	INSERT INTO Migrations (MigrationName)
		VALUES (@firstMigrationName);

	PRINT 'Sucess insert migration in MigrationHistory';
	PRINT 'Try to implement migration';
	
	CREATE TABLE Users(
		[Id] BIGINT IDENTITY(0, 1) PRIMARY KEY,
		[Phone] NVARCHAR(256) NOT NULL,
		[UserName] NVARCHAR(1024) NOT NULL,
		[Email] NVARCHAR(1024) NOT NULL,
		[Password] NVARCHAR (2048) NOT NULL,
		[CreatedAt] AS GETDATE() 
	);

	PRINT 'Create user table'

	CREATE TABLE RefreshTokens(
		[Id] BIGINT IDENTITY(0, 1) PRIMARY KEY,
		[UserId] BIGINT FOREIGN KEY ([UserId]) REFERENCES Users([Id]) NOT NULL,
		[Value] NVARCHAR (512) NOT NULL
	);

	PRINT 'Create RefreshTokenTable'

	CREATE INDEX userIdIndex ON [dbo].[RefreshTokens] (UserId);

	PRINT 'Index created by userid in refreshtokens';

	COMMIT TRANSACTION [Init]
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION [Init];
	SELECT
       ERROR_NUMBER() AS ErrorNumber,
       ERROR_SEVERITY() AS ErrorSeverity,
       ERROR_STATE() AS ErrorState,
       ERROR_PROCEDURE() AS ErrorProcedure,
       ERROR_LINE() AS ErrorLine,
       ERROR_MESSAGE() AS ErrorMessage;
END CATCH