USE [AssignRef]
GO
/****** Object:  StoredProcedure [dbo].[Seasons_Select_ById]    Script Date: 6/3/2023 10:28:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: Jalen Bates
-- Create date: 4/13/2023
-- Description: Select Seasons By ID
-- Code Reviewer:Ishimine Stidhum

-- MODIFIED BY: 
-- MODIFIED DATE:
-- Code Reviewer:Ishimine Stidhum
-- Note:functioning
-- =============================================

CREATE PROCEDURE [dbo].[Seasons_Select_ById]
    @Id INT


/*------Test Code-------

	declare @Id int = 1
	execute dbo.Seasons_Select_ById @Id


*/
AS
BEGIN
  SELECT s.[Id]
		,s.[Name]
		,s.[Year]
		,s.[ConferenceId]
		,c.[Name] AS ConferenceName
		,c.[Logo] AS ConferenceLogo
		,c.[Code] AS ConferenceCode
		,s.[StatusTypeId]
		,st.[Name] as StatusName
		,s.[Weeks]
		,s.[DateCreated]
		,s.[DateModified]
    FROM [dbo].[Seasons] s
    JOIN [dbo].[Conferences] c ON s.[ConferenceId] = c.[Id]
    JOIN [dbo].[StatusTypes] st ON s.[StatusTypeId] = st.[Id]
    WHERE s.[Id] = @Id
END
GO
