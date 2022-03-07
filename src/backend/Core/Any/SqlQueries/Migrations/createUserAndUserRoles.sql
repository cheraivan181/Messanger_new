DECLARE @migrationName nvarchar(64) = N'Init';

BEGIN TRANSACTION [Migration]
BEGIN TRY
	
	IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE [name] = 'Roles')
	BEGIN
		CREATE TABLE Roles(
			[Id] INT IDENTITY(0, 1) PRIMARY KEY NOT NULL,
			[Name] NVARCHAR(256) NOT NULL
		);
		PRINT 'RolesCreated'

		INSERT INTO Roles ([Name])
			VALUES('User');
		INSERT INTO Roles ([Name])
			VALUES('ProtocoledUser');
		INSERT INTO Roles ([Name])
			VALUES('Admin');
	END

	IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE [name] = 'UserRoles')
	BEGIN
		CREATE TABLE UserRoles(
			[Id] INT IDENTITY(0, 1) PRIMARY KEY NOT NULL,
			[UserId] BIGINT FOREIGN KEY ([UserId]) REFERENCES Users([Id]) NOT NULL,	
			[RoleId] INT FOREIGN KEY ([RoleId]) REFERENCES Roles([Id]) NOT NULL
		);
		PRINT 'UserRoles created';

		CREATE INDEX userIdIndex ON [dbo].[UserRoles] (UserId);
		CREATE INDEX roleIdIndex ON [dbo].[UserRoles] (RoleId);

		PRINT 'index created';
	END

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