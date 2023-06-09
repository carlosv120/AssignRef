USE [AssignRef]
GO
/****** Object:  StoredProcedure [dbo].[Certifications_Insert]    Script Date: 5/16/2023 6:46:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Carlos Vanegas
-- Create date: 2023-05-05
-- Description: Insert Procedure for Certifications, considering SeasonId, TestId and UserId as FK. TestId and MinimumScore Nullable values in case IsTestRequired equals 0
-- Code Reviewer: Ashante Jackson

-- MODIFIED BY: 
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================

CREATE PROC	[dbo].[Certifications_Insert]
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
										,@CreatedBy					int
										,@Id						int OUTPUT
AS

/*
		DECLARE		 @Id						int				= 0
					,@Name						nvarchar		= 'Certification Test'
					,@SeasonId					int				= 6					--FK
					,@IsPhysicalRequired		bit				= 1
					,@IsBackgroundCheckRequired	bit				= 0
					,@IsTestRequired			bit				= 0
					,@TestId					int				= 1					--FK
					,@MinimumScoreRequired		decimal(5,2)	= 10.2
					,@IsFitnessTestRequired		bit				= 1
					,@IsClinicRequired			bit				= 1
					,@DueDate					datetime2(7)	= '2023-05-15'
					,@CreatedBy					int				= 11				--FK

		EXECUTE		dbo.Certifications_Insert	 @Name
												,@SeasonId					
												,@IsPhysicalRequired		
												,@IsBackgroundCheckRequired	
												,@IsTestRequired			
												,@TestId					
												,@MinimumScoreRequired		
												,@IsFitnessTestRequired		
												,@IsClinicRequired			
												,@DueDate									
												,@CreatedBy					
												,@Id	OUTPUT

		Select *
		From dbo.Certifications
*/

BEGIN
		INSERT INTO [dbo].[Certifications]
				   ([Name]
				   ,[SeasonId]
				   ,[IsPhysicalRequired]
				   ,[IsBackgroundCheckRequired]
				   ,[IsTestRequired]
				   ,[TestId]
				   ,[MinimumScoreRequired]
				   ,[IsFitnessTestRequired]
				   ,[IsClinicRequired]
				   ,[DueDate]
				   ,[CreatedBy]
				   ,[ModifiedBy])
			 VALUES
				   (@Name
				   ,@SeasonId
				   ,@IsPhysicalRequired
				   ,@IsBackgroundCheckRequired
				   ,@IsTestRequired
				   ,@TestId
				   ,@MinimumScoreRequired
				   ,@IsFitnessTestRequired
				   ,@IsClinicRequired
				   ,@DueDate
				   ,@CreatedBy
				   ,@CreatedBy)
		
			SET @Id = SCOPE_IDENTITY();
END
GO
