USE [AssignRef]
GO
/****** Object:  StoredProcedure [dbo].[Assignments_Select_ByGameId_Details]    Script Date: 5/10/2023 4:34:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ishimine Stidhum
-- Create date: 4/12/2023
-- Description: dbo.Assignments_Select_ByGameId_Details
--					(Join Games, Locations, Teams, Users, Officials, Positions, AsssignStatus)
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
ALTER proc [dbo].[Assignments_Select_ByGameId_Details]
							@GameId int
AS
/*
		Declare @GameId int = 9

		Execute dbo.Assignments_Select_ByGameId_Details
									@GameId


*/
BEGIN

	SELECT  
			g.Id,
		   gs.Id as GameStatusId,
		   gs.[Name] as GameStatusName,
		   season.Id as SeasonId,
		   season.[Name] as SeasonName,
		   season.[Year] as SeasonYear,
		   g.[Week],
		   c.Id as ConferenceId,
		   c.Logo as ConferenceLogo,
		   c.[Name] as ConferenceName,
		   c.Code as ConferenceCode,
		   ht.Id as HomeTeamId,
		   ht.[Name] as HomeTeamName,
		   ht.Code as HomeTeamCode,
		   ht.Logo as HomeTeamLogo,
		   cht.Logo as HomeTeamConferenceLogo,
		   cht.[Name] as HomeTeamConference,
		   vht.[Name],
		   lht.City,
		   sht.[Code],
		   ht.SiteUrl as HomeTeamUrl,	  
		   HomeTeamMembers = (select tm.[Id], 
									 u.[FirstName], 
									 u.[LastName], 
									 u.[Mi], 
									 u.[AvatarUrl], 
									 u.[Email], 
									 u.[Phone], 
									 tm.[IsPrimaryContact], 
									 tm.[Position] 
									 from [dbo].[Users] as u inner join [dbo].[TeamMembers] as tm
									 on tm.[UserId] = u.[Id]
									 where tm.[TeamId] = ht.[Id]
									for JSON PATH),
		   vt.Id as VistingTeamId,
		   vt.[Name] as VisitingTeamName,
		   vt.Code as VisitingTeamCode,
		   vt.Logo as VisitingTeamLogo,
		   cvt.Logo as VisitingTeamConferenceLogo,
		   cvt.[Name] as VisitingTeamConference,
		   vvt.[Name],
		   lvt.City,
		   svt.[Code],
		   vt.SiteUrl as VisitingTeamUrl,
		   VisitingTeamMembers = (select tm.[Id], 
										 u.[FirstName], 
										 u.[LastName], 
										 u.[Mi], 
										 u.[AvatarUrl], 
										 u.[Email], 
										 u.[Phone], 
										 tm.[IsPrimaryContact], 
										 tm.[Position] 
										 from [dbo].[Users] as u inner join [dbo].[TeamMembers] as tm
										 on tm.[UserId] = u.[Id]
										 where tm.[TeamId] = vt.[Id]
										 for JSON PATH),
		   g.StartTime,
		   g.IsNonConference,
		   v.Id as VenueId,
		   v.[Name] as VenueName,
		   l.Id,
		   lt.Id as LoctionTypeId,
		   lt.[Name] as LoctionTypeName,
		   l.LineOne,
		   l.LineTwo,
		   l.City,
		   s.Id,
		   s.Code as StateCode,
		   s.[Name],
		   l.Zip,
		   l.Latitude,
		   l.Longitude,
		   v.PrimaryImageUrl,
		   g.DateCreated,
		   g.DateModified,
		   g.IsDeleted,
		    createUser.Id as CreateUserId,
		   modifiedUser.Id as ModifiedUserId,
		   Assignments = (SELECT  a.Id
									,a.Fee
									,a.DateCreated
									,a.DateModified
									,a.CreatedBy
									,a.ModifiedBy

			                        ,ast.Id as 'AssignmentType.id'
									,ast.[Name] as 'AssignmentType.Name'
									,fp.Id as 'Position.Id'
									,fp.[Name] as 'Position.Name'
									,fp.Code as 'Position.Code'
									,u.Id as 'User.Id'
									,u.FirstName as 'User.FirstName'
									,u.LastName as 'User.LastName'
									,u.Mi as 'User.Mi'
									,u.AvatarUrl as 'User.AvatarUrl'
									,u.Email as 'User.Email'
									,u.Phone  as 'User.Phone'
									,gt.Id  as 'Gender.Id'
									,gt.[Name] as 'Gender.Name'
									
									,asi.Id as 'AssignmentStatus.Id'
									,asi.[Name] as 'AssignmentStatus.Name'
									
									FROM [dbo].[Assignments] AS a
									inner join dbo.AssignmentTypes as ast
								ON a.AssignmentTypeId = ast.Id
								inner join dbo.FieldPositions as fp
								On a.PositionId = fp.Id
								inner join dbo.Users as u 
								ON a.UserId = u.Id
								inner join dbo.AssignmentStatus as asi
								ON a.AssignmentStatusId = asi.Id
								inner join dbo.GenderTypes as gt
								ON u.GenderId = gt.Id
								WHERE a.GameId = @GameId
								FOR JSON PATH
							)

	   from dbo.Games g
	inner join dbo.GameStatus gs
	on g.GameStatusId = gs.Id
	inner join dbo.Seasons as season
	on g.SeasonId = season.Id
	inner join dbo.Conferences c
	on g.ConferenceId = c.Id
	inner join dbo.Teams ht
	on g.HomeTeamId= ht.Id
	inner join dbo.Teams vt
	on g.VisitingTeamId = vt.Id
	inner join dbo.Venues v
	on g.VenueId = v.Id
	inner join dbo.Locations l
	on v.LocationId = l.Id
	inner join dbo.LocationTypes lt
	on l.LocationTypeId = lt.Id
	inner join dbo.States s
	on l.StateId= s.Id
	inner join Users createUser
	on g.CreatedBy = createUser.Id
	inner join Users modifiedUser
	on g.ModifiedBy = modifiedUser.Id
	inner join dbo.Venues vht
	on ht.MainVenueId = vht.Id
	inner join dbo.Venues vvt
	on vt.MainVenueId = vvt.Id
	inner join dbo.Locations lht
	on ht.LocationId = lht.Id
	inner join dbo.Locations lvt
	on vt.LocationId = lvt.Id
	inner join dbo.States sht
	on lht.StateId = sht.Id
	inner join dbo.States svt
	on lvt.StateId = svt.Id
	inner join dbo.Conferences cht
	on ht.ConferenceId = cht.Id
	inner join dbo.Conferences cvt
	on vt.ConferenceId = cvt.Id
	    WHERE g.Id = @GameId

END
