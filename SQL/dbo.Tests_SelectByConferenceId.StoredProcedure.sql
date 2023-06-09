USE [AssignRef]
GO
/****** Object:  StoredProcedure [dbo].[Tests_SelectByConferenceId]    Script Date: 6/3/2023 10:28:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--===================================
-- Author: <Carlos Vanegas>
-- Create date: <05-25-2023>
-- Description: Select By Conference Id Procedure with JSON objects and joins using the same format as the Select by Id.
-- Code Reviewer:

-- MODIFIED BY: 
-- MODIFIED DATE: 
-- Code Reviewer:
-- Note:
--===================================

	CREATE proc [dbo].[Tests_SelectByConferenceId]
			@ConferenceId int

	as
/*

	declare @ConferenceId int = 1

	execute dbo.Tests_SelectByConferenceId
			@ConferenceId

*/

	begin

	SELECT t.[id]
			  ,t.[name]
			  ,t.[description]
			  ,t.[StatusId]
			  ,s.[Name]
			  ,t.[TestTypeId] as id
			  ,tt.[name]
			  ,[questions] =	(
								Select	tq.[Id]
										,[QuestionType] =	Json_Query((
																			Select	qt.[Id],
																					qt.[name]
																			From [dbo].[QuestionTypes] as qt
																			Where tq.[questionTypeId] = qt.[id]
																			For Json Path, without_array_wrapper
																		))
										,tq.[question]
										,tq.[helpText]
										, tq.[isRequired]
										,tq.[isMultipleAllowed]
										,tq.[testId]
										,[Status] =	 Json_Query((
																	Select	 s2.[Id],
																			s2.[Name]
																	From [dbo].[StatusTypes] as s2
																	Where tq.[statusId] = s2.[Id]
																	For Json Path, without_array_wrapper
																)) 
										,tq.[sortOrder]
										,[AnswerOption] =	(
																Select	ao.[id],
																		ao.[questionId],
																		ao.[text],
																		ao.[additionalInfo],
																		ao.[value],
																		ao.[IsCorrect]
																From [dbo].[TestQuestionAnswerOptions] as ao
																Where tq.[Id] = ao.[questionId]
																For Json Path
															)
								From [dbo].[TestQuestions] as tq inner join [dbo].[StatusTypes] as s2
									on tq.[statusId] = s2.[id] 
								Where tq.[testId] = t.[id]
								For JSON Path
							)
			  ,t.ConferenceId
			  ,t.[dateCreated]
			  ,t.[dateModified]
			  ,t.[createdBy]
			  ,u.[FirstName]
			  ,u.[LastName]
			  ,u.[Mi]
			  ,u.[AvatarUrl]

		FROM [dbo].[Tests] as t 
			inner join dbo.StatusTypes as s on t.StatusId = s.Id 
			inner join [dbo].[TestTypes] as tt on t.TestTypeId = tt.Id
			inner join dbo.Users as u on t.CreatedBy = u.Id
		Where t.ConferenceId = @ConferenceId


	end

GO
