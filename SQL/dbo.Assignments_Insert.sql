USE [AssignRef]
GO
/****** Object:  StoredProcedure [dbo].[Assignments_Insert]    Script Date: 5/11/2023 2:31:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ishimine Stidhum
-- Create date: 4/10/2023
-- Description:Insert into dbo.Assignments
--			    
--Params:	@GameId int,
--				@AssignmentTypeId int,
--				@PositionId int,
--				@UserId int,
--				@Fee decimal(8,2),
--				@AssignmentStatusId int,
--				@CreatedBy int,
--				@ModifiedBy int,
--				@Id int OUTPUT
-- Code Reviewer: 

-- MODIFIED BY: 
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
--
-- =============================================
ALTER proc [dbo].[Assignments_Insert]
				@GameId int,
				@AssignmentTypeId int,
				@PositionId int,
				@UserId int,
				@Fee decimal(8,2),
				@AssignmentStatusId int,
				@CreatedBy int,
  			    @ModifiedBy int,
				@Id int OUTPUT
As
/*
	Declare @Id int = 0

	Declare @GameId int = 8
				,@AssignmentTypeId int = 1
				,@PositionId int = 9
				,@UserId int = 109
				,@Fee decimal(8,2) = 100.00
				,@AssignmentStatusId int = 2
				,@CreatedBy int = 8
  			    ,@ModifiedBy int = 8

	Execute dbo.Assignments_Insert
				@GameId,
				@AssignmentTypeId,
				@PositionId,
				@UserId,
				@Fee,
				@AssignmentStatusId,
				@CreatedBy,
  			    @ModifiedBy,
				@Id OUTPUT

	SELECT *
	FROM dbo.Assignments
	WHERE Id = @Id
				
*/
BEGIN
	

INSERT INTO [dbo].[Assignments]
           ([GameId]
           ,[AssignmentTypeId]
           ,[PositionId]
           ,[UserId]
           ,[Fee]
           ,[AssignmentStatusId]
           ,[CreatedBy]
           ,[ModifiedBy])
     VALUES
           (@GameId,
				@AssignmentTypeId,
				@PositionId,
				@UserId,
				@Fee,
				@AssignmentStatusId,
				@CreatedBy,
  			    @ModifiedBy)

	SET @Id = SCOPE_IDENTITY();



END


