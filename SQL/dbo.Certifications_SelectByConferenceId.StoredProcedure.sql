USE [AssignRef]
GO
/****** Object:  StoredProcedure [dbo].[Certifications_SelectByConferenceId]    Script Date: 6/3/2023 10:28:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Carlos Vanegas
-- Create date: 2023-05-29
-- Description: Selecting certifications by conferenceId
-- Code Reviewer: Shane Rivas Martin

-- MODIFIED BY: 
-- MODIFIED DATE: 
-- Code Reviewer:
-- Note: 
-- =============================================

CREATE PROC [dbo].[Certifications_SelectByConferenceId]
												 @PageIndex		int
												,@PageSize		int
												,@ConferenceId	int
AS

/*

	DECLARE   @PageIndex	int = 0	
			 ,@PageSize		int	= 2	
			 ,@SeasonId		int	= 1
			

	EXECUTE	dbo.Certifications_SelectByConferenceId
												 @PageIndex	
												,@PageSize	
												,@SeasonId	

	SELECT	*
	FROM	dbo.Certifications

*/

BEGIN
		DECLARE	@Offset	int	=	@PageIndex*@PageSize

		SELECT	 ct.[Id]
				,ct.[Name]
					,s.Id
					,s.[Name]
					,s.[Year]
						,cf.[id]
						,cf.[Name]
						,cf.[Code]
						,cf.[Logo]
				,ct.IsPhysicalRequired
				,ct.IsBackgroundCheckRequired
				,ct.IsTestRequired
					,ts.Id
					,ts.[Name]
				,ct.MinimumScoreRequired
				,ct.IsFitnessTestRequired
				,ct.IsClinicRequired
				,ct.DueDate
				,ct.IsActive
					,uc.Id			AS CreatedById
					,uc.FirstName
					,uc.LastName
					,uc.Mi
					,uc.AvatarUrl
					,um.Id			AS ModifiedById
					,um.FirstName
					,um.LastName
					,um.Mi
					,um.AvatarUrl
				,ct.DateCreated
				,ct.DateModified
				,TotalCount = COUNT(1) OVER()
					
		FROM	dbo.Certifications as ct inner join dbo.Seasons as s
		ON		ct.SeasonId = s.Id

				inner join dbo.Conferences as cf
		ON		s.ConferenceId = cf.Id

				left join dbo.Tests as ts
		ON		ct.TestId = ts.Id
				
				inner join dbo.Users as uc
		ON		ct.CreatedBy = uc.Id
				
				inner join dbo.Users as um
		ON		ct.ModifiedBy = um.Id

		WHERE (cf.Id = @ConferenceId and ct.IsActive = 1)
		ORDER BY	ct.SeasonId
		OFFSET		@Offset ROWS
		FETCH NEXT	@PageSize ROWS ONLY
END
GO
