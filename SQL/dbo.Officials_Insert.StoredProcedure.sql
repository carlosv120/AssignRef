USE [AssignRef]
GO
/****** Object:  StoredProcedure [dbo].[Officials_Insert]    Script Date: 5/5/2023 9:03:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: <Marcela Aburto>
-- Create date: <04/08/2023>
-- Description: <Officials_Insert>
-- Code Reviewer:

-- MODIFIED BY: author
-- MODIFIED DATE:12/1/2020
-- Code Reviewer: Mykhailo Zezul
-- Note: Is working
-- =============================================

CREATE proc [dbo].[Officials_Insert]
		@UserId int,
		@PrimaryPositionId int,
		@LocationId int,
		@Id int OUTPUT

as

/* ------- Test Code -------

	Declare @Id int = 0;

	Declare @UserId int = 20,
		@PrimaryPositionId int = 6,
		@LocationId int = 4


	Execute [dbo].[Officials_Insert]
				@UserId,
				@PrimaryPositionId,
				@LocationId,
				@Id OUTPUT


*/

BEGIN

INSERT INTO [dbo].[Officials]
           ([UserId]
           ,[PrimaryPositionId]
           ,[LocationId])
     VALUES
           (@UserId
           ,@PrimaryPositionId
           ,@LocationId)

	Set @Id = SCOPE_IDENTITY()
	
END

GO
