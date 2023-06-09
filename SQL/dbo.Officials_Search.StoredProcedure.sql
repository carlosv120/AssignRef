USE [AssignRef]
GO
/****** Object:  StoredProcedure [dbo].[Officials_Search]    Script Date: 5/5/2023 9:03:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: <Marcela Aburto>
-- Create date: <04/10/2023>
-- Description: <Officials_Search>
-- Code Reviewer:

-- MODIFIED BY: Marcela Aburto
-- MODIFIED DATE: 04/14/2023
-- Code Reviewer: Mykhailo Zezul
-- Note: Is Working.
-- =============================================


CREATE proc [dbo].[Officials_Search]
					@PageIndex int
					,@PageSize int
					, @Query nvarchar(50)


as
/*

Declare @PageIndex int = 0
	   , @PageSize int = 10
	   , @Query nvarchar(50) = 'john'

Execute [dbo].[Officials_Search]
					@PageIndex
					, @PageSize
					, @Query

*/

BEGIN

			Declare @Offset int = @PageIndex * @PageSize

			SELECT		[o].[Id]
						, [o].[UserId]
						, [u].[FirstName]
						, [u].[LastName]
						, [u].[Mi]
						, [u].[AvatarUrl]
						, [g].[Name] as Gender
						, [u].[Email]
						, [u].[Phone]
						, [o].[PrimaryPositionId]
						, [fp].[Name] as PositionName
						, [fp].[Code] as PositionCode
						, [l].[Id] as LocationId
						, [l].[LineOne] as LocationLineOne
						, [l].[LineTwo] as LocationLineTwo
						, [l].[City] as LocationCity
						, [s].[Id] as StateId
						, [s].[Name] as StateName
						, [s].[Code] as StateCode
						, [l].[Zip] as LocationZipCode
						, [l].[Latitude] as LocationLatitude
						, [l].[Longitude] as LocationLongitude
						, [lt].[Id] as LocationTypeId
						, [lt].[Name] as LocationTypeName
						, [o].[DateCreated]
						, [o].[DateModified]
						, [o].[StatusTypeId]
						, [st].[Name] as StatusTypeName
						, Conference = (
						select [c].[Id]
						, [c].[Name]
						, [c].[Code]
						, [c].[Logo]
						from [dbo].[Conferences] as c inner join [dbo].[ConferenceUsers] as cu
						on c.Id = cu.ConferenceId
						where cu.UserId = o.UserId
						for JSON AUTO
						)
						, TotalCount = COUNT(1) OVER()
								
					From [dbo].[Officials] as o inner join [dbo].[Users] as u
						on o.UserId = u.Id 
						inner join [dbo].[GenderTypes] as g
						on u.GenderId = g.Id
						inner join [dbo].[FieldPositions] as fp
						on fp.Id = o.PrimaryPositionId
						inner join [dbo].[Locations] as l
						on l.Id = o.LocationId
						inner join [dbo].[LocationTypes] as lt
						on l.LocationTypeId = lt.Id
						inner join [dbo].[States] as s
						on l.StateId = s.Id
						inner join [dbo].[StatusTypes] as st
						on o.StatusTypeId = st.Id

					Where [u].[FirstName] LIKE '%' + @Query + '%' or
							[u].[LastName] LIKE '%' + @Query + '%' or
							[fp].[Name] LIKE '%' + @Query + '%' 
					Order by o.Id

					Offset @Offset Rows
					Fetch Next @PageSize Rows Only

END
GO
