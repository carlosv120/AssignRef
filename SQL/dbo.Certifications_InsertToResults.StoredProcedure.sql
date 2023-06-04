USE [AssignRef]
GO
/****** Object:  StoredProcedure [dbo].[Certifications_InsertToResults]    Script Date: 6/3/2023 10:28:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROC [dbo].[Certifications_InsertToResults]
												 @CertificationId	int
												,@UserId			int
												,@ConferenceId		int
												,@BatchUsers		dbo.BatchUsers READONLY
												,@Id                int OUTPUT		
AS

/*
	DECLARE    @Id					int = 0 
			  ,@CertificationId		int = 65	
			  ,@UserId				int = 33	
			  ,@ConferenceId		int = 1

	DECLARE	   @Users				dbo.BatchUsers
		
		INSERT INTO	@Users(Id)
				Values('1')
		INSERT INTO @Users(Id)
				Values('3')

	EXECUTE	dbo.Certifications_InsertToResults
												  @CertificationId
												 ,@UserId
												 ,@ConferenceId
												 ,@Users
												 ,@Id				 OUTPUT		
	SELECT *
	FROM dbo.Certifications
	WHERE Id = @CertificationId
	
	SELECT *
	FROM dbo.CertificationResults
	WHERE Id = @Id

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
			  ,CASE WHEN ct.[MinimumScoreRequired]		= NULL	THEN NULL ELSE ct.[MinimumScoreRequired] END
			  ,CASE WHEN ct.[IsFitnessTestRequired]		= 1		THEN 0    ELSE NULL END
			  ,CASE WHEN ct.[IsClinicRequired]			= 1		THEN 0	  ELSE NULL END
			  ,@UserId
			  ,@UserId
			  , (Select bu.Id
					From @BatchUsers as bu)
		  FROM [dbo].[Certifications] AS ct
		  WHERE ct.Id = @CertificationId

		  SET @Id = SCOPE_IDENTITY();


END
GO
