USE [AssignRef]
GO
/****** Object:  StoredProcedure [dbo].[Certifications_Delete]    Script Date: 5/16/2023 6:46:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Carlos Vanegas
-- Create date: 2023-05-05
-- Description: Delete Procedure for Certifications. Proc doesn't actually delete anything, it updates IsActive to 0 (false)
-- Code Reviewer: Ashante Jackson

-- MODIFIED BY: 
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================


CREATE PROC	[dbo].[Certifications_Delete]
										 @Id int
										,@ModifiedBy int
AS

/*
	DECLARE  @Id			int		= 17
			,@ModifiedBy	int		= 11
	
		SELECT	IsActive
		FROM	dbo.Certifications
		WHERE	Id = @Id
	
	EXECUTE	dbo.Certifications_Delete
										 @Id
										,@ModifiedBy
		SELECT	IsActive
		FROM	dbo.Certifications
		WHERE	Id = @Id
*/

BEGIN
		DECLARE @DateModified	datetime2	=	GETUTCDATE()

		UPDATE [dbo].[Certifications]
		   SET 
				 [IsActive]		= 0
				,[ModifiedBy]	= @ModifiedBy	
				,[DateModified] = @DateModified
		 WHERE Id = @Id

END
GO
