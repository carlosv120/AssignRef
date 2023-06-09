USE [AssignRef]
GO
/****** Object:  Table [dbo].[Officials]    Script Date: 5/5/2023 9:03:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Officials](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[PrimaryPositionId] [int] NOT NULL,
	[LocationId] [int] NOT NULL,
	[DateCreated] [datetime2](7) NOT NULL,
	[DateModified] [datetime2](7) NOT NULL,
	[StatusTypeId] [int] NOT NULL,
 CONSTRAINT [PK__Official__3214EC0753094E41] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Officials] ADD  CONSTRAINT [DF__Officials__DateC__1F98B2C1]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Officials] ADD  CONSTRAINT [DF__Officials__DateM__208CD6FA]  DEFAULT (getutcdate()) FOR [DateModified]
GO
ALTER TABLE [dbo].[Officials] ADD  CONSTRAINT [DF__Officials__Statu__2180FB33]  DEFAULT ((1)) FOR [StatusTypeId]
GO
ALTER TABLE [dbo].[Officials]  WITH CHECK ADD  CONSTRAINT [FK__Officials__Locat__1EA48E88] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Locations] ([Id])
GO
ALTER TABLE [dbo].[Officials] CHECK CONSTRAINT [FK__Officials__Locat__1EA48E88]
GO
ALTER TABLE [dbo].[Officials]  WITH CHECK ADD  CONSTRAINT [FK__Officials__Prima__1DB06A4F] FOREIGN KEY([PrimaryPositionId])
REFERENCES [dbo].[FieldPositions] ([Id])
GO
ALTER TABLE [dbo].[Officials] CHECK CONSTRAINT [FK__Officials__Prima__1DB06A4F]
GO
ALTER TABLE [dbo].[Officials]  WITH CHECK ADD  CONSTRAINT [FK__Officials__Statu__22751F6C] FOREIGN KEY([StatusTypeId])
REFERENCES [dbo].[StatusTypes] ([Id])
GO
ALTER TABLE [dbo].[Officials] CHECK CONSTRAINT [FK__Officials__Statu__22751F6C]
GO
ALTER TABLE [dbo].[Officials]  WITH CHECK ADD  CONSTRAINT [FK__Officials__UserI__1CBC4616] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Officials] CHECK CONSTRAINT [FK__Officials__UserI__1CBC4616]
GO
