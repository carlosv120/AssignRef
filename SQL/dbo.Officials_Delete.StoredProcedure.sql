USE [AssignRef]
GO
/****** Object:  StoredProcedure [dbo].[Officials_Delete]    Script Date: 5/5/2023 9:03:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: <Marcela Aburto>
-- Create date: <04/08/2023>
-- Description: <Officials_Delete>
-- Code Reviewer:

-- MODIFIED BY: author
-- MODIFIED DATE:12/1/2020
-- Code Reviewer: Mykhailo Zezul
-- Note: Is Working!
-- =============================================

CREATE proc [dbo].[Officials_Delete]
		@Id int 

as


/* ------- Test Code -------


	Declare @Id int = 10

	Execute [dbo].[Officials_Delete] @Id

	select *
	from dbo.Officials

*/

BEGIN

		Declare @DateModified datetime2(7) = getutcdate()

		UPDATE [dbo].[Officials]
		   SET [DateModified] = @DateModified
			  ,[StatusTypeId] = 2
		 WHERE Id = @Id
	
END
GO
