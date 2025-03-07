USE [PerformRetriveData] GO 
/****** Object:  Table [dbo].[User]    Script Date: 10/10/2024 10:19:48 ******/
SET ANSI_NULLS ON GO

SET QUOTED_IDENTIFIER ON GO

CREATE TABLE [dbo].[User](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](250) NOT NULL
) ON [PRIMARY]
GO
