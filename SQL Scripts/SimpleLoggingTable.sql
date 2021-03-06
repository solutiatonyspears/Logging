USE [Logging]
GO
/****** Object:  StoredProcedure [dbo].[WriteLog]    Script Date: 2/22/2018 3:25:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[WriteLog]
(
	@SeverityLevel nvarchar(50),
	@Source nvarchar(50),
	@Message nvarchar(4000),
	@LogId int OUTPUT
)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Log]
           ([SeverityLevel]
           ,[Message]
           ,[Source]
		   ,DateCreated)
     VALUES
           (@SeverityLevel
           ,@Message
           ,@Source
		   ,GETDATE())

	SET @LogId = @@IDENTITY
	RETURN @LogId
END

GO

USE [Logging]
GO

/****** Object:  StoredProcedure [dbo].[WriteLog]    Script Date: 2/23/2018 4:57:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[WriteLog]
(
	@SeverityLevel nvarchar(50),
	@Source nvarchar(50),
	@Message nvarchar(4000),
	@LogId int OUTPUT
)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Log]
           ([SeverityLevel]
           ,[Message]
           ,[Source]
		   ,DateCreated)
     VALUES
           (@SeverityLevel
           ,@Message
           ,@Source
		   ,GETDATE())

	SET @LogId = @@IDENTITY
	RETURN @LogId
END



GO


