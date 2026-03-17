SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[UserList](
	--[SOLineKey] [int] NOT NULL,
	--[QtyProduced] [decimal](15, 0) NOT NULL,
	--[Status] [varchar](15) NOT NULL,
	--[CompletionDate] [datetime] NULL,
	--[PriorityCode] [smallint] NOT NULL,
	[Username] [varchar](30) NOT NULL,
	[Password] [varchar](30) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT  [UserList] VALUES ('a','a')

--SELECT FormulaID, FormulaName, FormulaType, EnteredDate, EnteredDate FROM Formulas WHERE IsInactive
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[Formulas](
	--[SOLineKey] [int] NOT NULL,
	--[QtyProduced] [decimal](15, 0) NOT NULL,
	--[Status] [varchar](15) NOT NULL,
	--[CompletionDate] [datetime] NULL,
	--[PriorityCode] [smallint] NOT NULL,
	[FormulaID] [varchar](30) NOT NULL,
	[FormulaName] [varchar](30) NOT NULL,
	[FormulaType] [varchar](30) NOT NULL,
	[EnteredDate] [date],
	[IsInactive] smallint NOT NULL
PRIMARY KEY CLUSTERED 
(
	[FormulaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [Formulas] VALUES ('AAA','Aaaa','Typ',GETDATE(),1)
INSERT [Formulas] VALUES ('BB','Bbbb','Typ',GETDATE(),1)