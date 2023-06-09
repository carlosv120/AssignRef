USE [AssignRef]
GO
/****** Object:  StoredProcedure [dbo].[Certifications_Update]    Script Date: 6/3/2023 10:28:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Carlos Vanegas
-- Create date: 2023-05-05
-- Description: Update Procedure for Certifications, considering SeasonId, TestId and UserId as FK. TestId and MinimumScore Nullable values in case IsTestRequired equals 0
-- Code Reviewer: Ashante Jackson

-- MODIFIED BY: 
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================

CREATE PROC	[dbo].[Certifications_Update]
										 @Name						nvarchar(100)
										,@SeasonId					int
										,@IsPhysicalRequired		bit
										,@IsBackgroundCheckRequired	bit
										,@IsTestRequired			bit
										,@TestId					int				= NULL
										,@MinimumScoreRequired		decimal(5,2)	= NULL
										,@IsFitnessTestRequired		bit
										,@IsClinicRequired			bit
										,@DueDate					datetime2(7)
										,@Modifiedby				int
										,@Id						int OUTPUT
AS

/*

	DECLARE		 @Id						int				= 65
				,@Name						nvarchar(100)	= 'Updated Name'
				,@SeasonId					int				= 5				--FK
				,@IsPhysicalRequired		bit				= 0
				,@IsBackgroundCheckRequired	bit				= 1
				,@IsTestRequired			bit				= 1
				,@TestId					int				= 1				--FK
				,@MinimumScoreRequired		decimal(5,2)	= 3.1
				,@IsFitnessTestRequired		bit				= 0
				,@IsClinicRequired			bit				= 0
				,@DueDate					datetime2(7)	= '2023-05-15'
				,@Modifiedby				int				= 9				--FK

		SELECT	*
		FROM	dbo.Certifications
		WHERE	Id = @Id

	EXECUTE		dbo.Certifications_Update
											 @Name
											,@SeasonId					
											,@IsPhysicalRequired		
											,@IsBackgroundCheckRequired	
											,@IsTestRequired			
											,@TestId					
											,@MinimumScoreRequired		
											,@IsFitnessTestRequired		
											,@IsClinicRequired			
											,@DueDate													
											,@Modifiedby
											,@Id	OUTPUT

		SELECT	*
		FROM	dbo.Certifications
		WHERE	Id = @Id


*/

BEGIN
		DECLARE @DateModified	datetime2	=	GETUTCDATE()

		UPDATE [dbo].[Certifications]
		   SET [Name]						= @Name
			  ,[SeasonId]					= @SeasonId
			  ,[IsPhysicalRequired]			= @IsPhysicalRequired
			  ,[IsBackgroundCheckRequired]	= @IsBackgroundCheckRequired
			  ,[IsTestRequired]				= @IsTestRequired
			  ,[TestId]						= @TestId
			  ,[MinimumScoreRequired]		= @MinimumScoreRequired
			  ,[IsFitnessTestRequired]		= @IsFitnessTestRequired
			  ,[IsClinicRequired]			= @IsClinicRequired
			  ,[DueDate]					= @DueDate
			  ,[ModifiedBy]					= @ModifiedBy
			  ,[DateModified]				= @DateModified
		 WHERE Id = @Id
END
GO
