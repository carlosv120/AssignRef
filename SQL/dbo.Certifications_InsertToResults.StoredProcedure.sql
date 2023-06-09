USE [AssignRef]
GO
/****** Object:  StoredProcedure [dbo].[Certifications_InsertToResults]    Script Date: 6/7/2023 3:29:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Carlos Vanegas
-- Create date: 2023-06-06
-- Description: Insert a group of officials with the same certificationId to dbo.CertificationsResults
-- Code Reviewer: Jeremiah Francis

-- MODIFIED BY: 
-- MODIFIED DATE: 
-- Code Reviewer: 
-- Note: 
-- =============================================

CREATE PROC [dbo].[Certifications_InsertToResults]
												 @CertificationId	int
												,@UserId			int
												,@ConferenceId		int

AS

/*
	DECLARE    @CertificationId		int = 65	
			  ,@UserId				int = 33	
			  ,@ConferenceId		int = 3

	EXECUTE	dbo.Certifications_InsertToResults
												  @CertificationId
												 ,@UserId
												 ,@ConferenceId
	SELECT *
	FROM dbo.Certifications
	WHERE Id = @CertificationId
	
	SELECT *
	FROM dbo.CertificationResults
*/

BEGIN

		DECLARE	   @BatchUsers				dbo.BatchUsers

		INSERT INTO	@BatchUsers(Id)
				
				SELECT  [o].[Id]				
				FROM [dbo].[Officials] AS o 
				
				INNER JOIN [dbo].[ConferenceUsers] AS cu
				ON o.UserId = cu.UserId
				INNER JOIN [dbo].[Conferences] as c
				ON cu.ConferenceId = c.Id		
				
				WHERE c.Id = @ConferenceId


		INSERT INTO [dbo].[CertificationResults]
				([CertificationId]
				,[IsPhysicalCompleted]
				,[IsBackgroundCheckCompleted]
				,[IsTestCompleted]
				,[TestInstanceId]
				,[Score]
				,[IsFitnessTestCompleted]
				,[IsClinicAttended]
				,[CreatedBy]
				,[ModifiedBy]
				,[UserId])

		SELECT ct.[Id]
			  ,CASE WHEN ct.[IsPhysicalRequired]		= 1		THEN 0    ELSE NULL END
			  ,CASE WHEN ct.[IsBackgroundCheckRequired] = 1		THEN 0	  ELSE NULL END
			  ,CASE WHEN ct.[IsTestRequired]			= 1		THEN 0	  ELSE NULL END
			  ,CASE WHEN ct.[TestId]					= NULL	THEN NULL ELSE ct.[TestId] END
			  ,NULL
			  ,CASE WHEN ct.[IsFitnessTestRequired]		= 1		THEN 0    ELSE NULL END
			  ,CASE WHEN ct.[IsClinicRequired]			= 1		THEN 0	  ELSE NULL END
			  ,@UserId
			  ,@UserId
			  ,U.Id
			 
		  FROM [dbo].[Certifications] AS ct
		  CROSS JOIN @BatchUsers as U 
		  WHERE ct.Id = @CertificationId


END
GO
