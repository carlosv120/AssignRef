USE [AssignRef]
GO
/****** Object:  StoredProcedure [dbo].[Certifications_BatchInsertToResults]    Script Date: 6/14/2023 1:03:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: Carlos Vanegas
-- Create date: 2023-06-12
-- Description: Insert a batch of officials with the same certificationId to dbo.CertificationsResults
-- Code Reviewer: Shane Rivas Martin

-- MODIFIED BY: 
-- MODIFIED DATE: 
-- Code Reviewer: 
-- Note: 
-- =============================================

CREATE PROC [dbo].[Certifications_BatchInsertToResults]
												 @CertificationId	int
												,@UserId			int
												,@BatchUsers		dbo.BatchUsers READONLY

AS

/*
	DECLARE    @CertificationId		int = 109	
			  ,@UserId				int = 33	

	DECLARE	   @BatchUsers			dbo.BatchUsers
			

				INSERT INTO @BatchUsers(Id)
						Values('95')
				INSERT INTO @BatchUsers(Id)
						Values('44')
				

	EXECUTE	dbo.Certifications_BatchInsertToResults
												  @CertificationId
												 ,@UserId
												 ,@BatchUsers

	
	SELECT *
	FROM dbo.CertificationResults
*/

BEGIN

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


	DECLARE @DateModified	datetime2	=	GETUTCDATE()
	UPDATE [dbo].[Certifications]
		  SET 
				 [IsAssigned]	= 1
				,[ModifiedBy]	= @UserId	
				,[DateModified] = @DateModified
		  WHERE Id = @CertificationId


END
GO
