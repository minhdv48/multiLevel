USE [MultiLevel]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 3/28/2023 11:54:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](250) NULL,
	[Password] [varchar](2000) NULL,
	[ProfileId] [int] NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Invoice]    Script Date: 3/28/2023 11:54:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Invoice](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Path] [nvarchar](500) NULL,
	[ProfileId] [int] NULL,
	[DateCreated] [datetime] NULL,
 CONSTRAINT [PK_Invoice] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Profile]    Script Date: 3/28/2023 11:54:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Profile](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](500) NULL,
	[Address] [nvarchar](2500) NULL,
	[CityId] [int] NULL,
	[DistrictId] [int] NULL,
	[WardId] [int] NULL,
	[CodeRefer] [varchar](50) NULL,
	[IDCard] [varchar](50) NULL,
	[CardVerifyBy] [nvarchar](500) NULL,
	[CardVerifyDate] [datetime] NULL,
	[PathInvoice] [nvarchar](500) NULL,
	[DateJoin] [datetime] NULL,
	[DateVerify] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
	[Phone] [varchar](50) NULL,
	[Email] [varchar](500) NULL,
	[Levels] [int] NULL,
	[ParentId] [int] NULL,
	[ReferBy] [varchar](50) NULL,
 CONSTRAINT [PK_Profile] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Setting]    Script Date: 3/28/2023 11:54:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Setting](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RootValue] [decimal](18, 2) NULL,
	[Profit] [decimal](18, 2) NULL,
 CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TreeLevel]    Script Date: 3/28/2023 11:54:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TreeLevel](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ProfileId] [int] NULL,
	[Levels] [int] NULL,
	[Value] [decimal](18, 2) NULL,
	[ParentId] [int] NULL,
	[Benefit] [decimal](18, 2) NULL,
 CONSTRAINT [PK_TreeLevel] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Account] ON 
GO
INSERT [dbo].[Account] ([Id], [UserName], [Password], [ProfileId]) VALUES (1, N'0987689448', N'8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92', 1)
GO
SET IDENTITY_INSERT [dbo].[Account] OFF
GO
SET IDENTITY_INSERT [dbo].[Profile] ON 
GO
INSERT [dbo].[Profile] ([Id], [FullName], [Address], [CityId], [DistrictId], [WardId], [CodeRefer], [IDCard], [CardVerifyBy], [CardVerifyDate], [PathInvoice], [DateJoin], [DateVerify], [ModifiedDate], [Phone], [Email], [Levels], [ParentId], [ReferBy]) VALUES (1, N'VAN MINH DUONG', N'Duong Noi ha dong', NULL, NULL, NULL, N'QSJS111', N'0123456789', N'Ha Noi', CAST(N'2023-03-03T00:00:00.000' AS DateTime), NULL, CAST(N'2023-03-27T22:42:30.580' AS DateTime), NULL, NULL, N'0987689448', NULL, 0, 0, NULL)
GO
SET IDENTITY_INSERT [dbo].[Profile] OFF