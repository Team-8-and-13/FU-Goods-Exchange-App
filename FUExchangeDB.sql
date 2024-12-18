USE [FUExchangeDB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 15/10/2024 11:36:27 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 15/10/2024 11:36:27 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[LastUpdatedBy] [nvarchar](max) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[CreatedTime] [datetimeoffset](7) NOT NULL,
	[LastUpdatedTime] [datetimeoffset](7) NOT NULL,
	[DeletedTime] [datetimeoffset](7) NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 15/10/2024 11:36:27 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [uniqueidentifier] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[LastUpdatedBy] [nvarchar](max) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[CreatedTime] [datetimeoffset](7) NOT NULL,
	[LastUpdatedTime] [datetimeoffset](7) NOT NULL,
	[DeletedTime] [datetimeoffset](7) NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 15/10/2024 11:36:27 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[LastUpdatedBy] [nvarchar](max) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[CreatedTime] [datetimeoffset](7) NOT NULL,
	[LastUpdatedTime] [datetimeoffset](7) NOT NULL,
	[DeletedTime] [datetimeoffset](7) NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 15/10/2024 11:36:27 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[LastUpdatedBy] [nvarchar](max) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[CreatedTime] [datetimeoffset](7) NOT NULL,
	[LastUpdatedTime] [datetimeoffset](7) NOT NULL,
	[DeletedTime] [datetimeoffset](7) NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 15/10/2024 11:36:27 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[LastUpdatedBy] [nvarchar](max) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[CreatedTime] [datetimeoffset](7) NOT NULL,
	[LastUpdatedTime] [datetimeoffset](7) NOT NULL,
	[DeletedTime] [datetimeoffset](7) NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 15/10/2024 11:36:27 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [uniqueidentifier] NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[UserInfoId] [nvarchar](450) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[LastUpdatedBy] [nvarchar](max) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[CreatedTime] [datetimeoffset](7) NOT NULL,
	[LastUpdatedTime] [datetimeoffset](7) NOT NULL,
	[DeletedTime] [datetimeoffset](7) NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 15/10/2024 11:36:27 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [uniqueidentifier] NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[LastUpdatedBy] [nvarchar](max) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[CreatedTime] [datetimeoffset](7) NOT NULL,
	[LastUpdatedTime] [datetimeoffset](7) NOT NULL,
	[DeletedTime] [datetimeoffset](7) NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Bans]    Script Date: 15/10/2024 11:36:27 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bans](
	[Id] [nvarchar](450) NOT NULL,
	[ReportId] [nvarchar](450) NOT NULL,
	[Expires] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[LastUpdatedBy] [nvarchar](max) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[CreatedTime] [datetime2](7) NOT NULL,
	[LastUpdatedTime] [datetime2](7) NOT NULL,
	[DeletedTime] [datetime2](7) NULL,
 CONSTRAINT [PK_Bans] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 15/10/2024 11:36:27 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[LastUpdatedBy] [nvarchar](max) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[CreatedTime] [datetime2](7) NOT NULL,
	[LastUpdatedTime] [datetime2](7) NOT NULL,
	[DeletedTime] [datetime2](7) NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comments]    Script Date: 15/10/2024 11:36:27 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[Id] [nvarchar](450) NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[RepCmtId] [nvarchar](max) NULL,
	[ProductId] [nvarchar](450) NOT NULL,
	[CommentText] [nvarchar](max) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[LastUpdatedBy] [nvarchar](max) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[CreatedTime] [datetime2](7) NOT NULL,
	[LastUpdatedTime] [datetime2](7) NOT NULL,
	[DeletedTime] [datetime2](7) NULL,
	[CommentArea] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Exchanges]    Script Date: 15/10/2024 11:36:27 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Exchanges](
	[Id] [nvarchar](450) NOT NULL,
	[ProductId] [nvarchar](450) NOT NULL,
	[BuyerId] [uniqueidentifier] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[LastUpdatedBy] [nvarchar](max) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[CreatedTime] [datetime2](7) NOT NULL,
	[LastUpdatedTime] [datetime2](7) NOT NULL,
	[DeletedTime] [datetime2](7) NULL,
 CONSTRAINT [PK_Exchanges] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notifications]    Script Date: 15/10/2024 11:36:27 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notifications](
	[Id] [nvarchar](450) NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[ProductId] [nvarchar](450) NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[LastUpdatedBy] [nvarchar](max) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[CreatedTime] [datetime2](7) NOT NULL,
	[LastUpdatedTime] [datetime2](7) NOT NULL,
	[DeletedTime] [datetime2](7) NULL,
 CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductImages]    Script Date: 15/10/2024 11:36:27 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductImages](
	[Id] [nvarchar](450) NOT NULL,
	[ProductId] [nvarchar](450) NOT NULL,
	[Image] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[LastUpdatedBy] [nvarchar](max) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[CreatedTime] [datetime2](7) NOT NULL,
	[LastUpdatedTime] [datetime2](7) NOT NULL,
	[DeletedTime] [datetime2](7) NULL,
 CONSTRAINT [PK_ProductImages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 15/10/2024 11:36:27 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[Id] [nvarchar](450) NOT NULL,
	[SellerId] [uniqueidentifier] NOT NULL,
	[CategoryId] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Price] [float] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Image] [nvarchar](max) NULL,
	[NumberOfStar] [float] NOT NULL,
	[Approve] [bit] NOT NULL,
	[Sold] [bit] NOT NULL,
	[Rating] [int] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[LastUpdatedBy] [nvarchar](max) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[CreatedTime] [datetime2](7) NOT NULL,
	[LastUpdatedTime] [datetime2](7) NOT NULL,
	[DeletedTime] [datetime2](7) NULL,
	[TotalStar] [int] NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reports]    Script Date: 15/10/2024 11:36:27 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reports](
	[Id] [nvarchar](450) NOT NULL,
	[UserId] [uniqueidentifier] NULL,
	[Reason] [nvarchar](max) NOT NULL,
	[Status] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[LastUpdatedBy] [nvarchar](max) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[CreatedTime] [datetime2](7) NOT NULL,
	[LastUpdatedTime] [datetime2](7) NOT NULL,
	[DeletedTime] [datetime2](7) NULL,
 CONSTRAINT [PK_Reports] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserInfos]    Script Date: 15/10/2024 11:36:27 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserInfos](
	[Id] [nvarchar](450) NOT NULL,
	[FullName] [nvarchar](max) NOT NULL,
	[BankAccount] [nvarchar](max) NULL,
	[BankAccountName] [nvarchar](max) NULL,
	[Bank] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[LastUpdatedBy] [nvarchar](max) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[CreatedTime] [datetime2](7) NOT NULL,
	[LastUpdatedTime] [datetime2](7) NOT NULL,
	[DeletedTime] [datetime2](7) NULL,
 CONSTRAINT [PK_UserInfos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241008151428_init', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241011060059_check', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241015021245_addCommentArea', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241015033251_totalStar', N'8.0.8')
GO
INSERT [dbo].[AspNetRoles] ([Id], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'3690590a-dd36-457c-3120-08dce7aca610', NULL, NULL, NULL, CAST(N'2024-10-08T22:19:48.5972542+00:00' AS DateTimeOffset), CAST(N'2024-10-08T22:19:48.5972542+00:00' AS DateTimeOffset), NULL, N'Admin', N'ADMIN', NULL)
INSERT [dbo].[AspNetRoles] ([Id], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'557095d1-2e87-4474-3121-08dce7aca610', NULL, NULL, NULL, CAST(N'2024-10-08T22:19:48.7419516+00:00' AS DateTimeOffset), CAST(N'2024-10-08T22:19:48.7419516+00:00' AS DateTimeOffset), NULL, N'UserPolicy', N'USERPOLICY', NULL)
INSERT [dbo].[AspNetRoles] ([Id], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'26838c62-4c9b-4bb2-3122-08dce7aca610', NULL, NULL, NULL, CAST(N'2024-10-08T22:19:48.7512474+00:00' AS DateTimeOffset), CAST(N'2024-10-08T22:19:48.7512474+00:00' AS DateTimeOffset), NULL, N'Moderator', N'MODERATOR', NULL)
INSERT [dbo].[AspNetRoles] ([Id], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'a5bd593f-eeea-48c1-bb3c-08dce9b7dc3b', NULL, NULL, NULL, CAST(N'2024-10-11T12:45:06.2778092+00:00' AS DateTimeOffset), CAST(N'2024-10-11T12:45:06.2778092+00:00' AS DateTimeOffset), NULL, N'User', N'USER', NULL)
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (N'7c0023d9-c1c5-49fc-4240-08dce7aca63c', N'3690590a-dd36-457c-3120-08dce7aca610', NULL, NULL, NULL, CAST(N'2024-10-08T22:19:49.0959645+00:00' AS DateTimeOffset), CAST(N'2024-10-08T22:19:49.0959645+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (N'56244908-ee29-49df-4241-08dce7aca63c', N'3690590a-dd36-457c-3120-08dce7aca610', NULL, NULL, NULL, CAST(N'2024-10-08T22:19:49.2380616+00:00' AS DateTimeOffset), CAST(N'2024-10-08T22:19:49.2380616+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (N'd47648e9-59ea-4139-4242-08dce7aca63c', N'26838c62-4c9b-4bb2-3122-08dce7aca610', NULL, NULL, NULL, CAST(N'2024-10-08T22:19:49.3600549+00:00' AS DateTimeOffset), CAST(N'2024-10-08T22:19:49.3600549+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (N'ce031a93-5da6-4291-8865-08dce82db53b', N'557095d1-2e87-4474-3121-08dce7aca610', NULL, NULL, NULL, CAST(N'2024-10-09T13:43:39.3418880+00:00' AS DateTimeOffset), CAST(N'2024-10-09T13:43:39.3418880+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (N'bde39811-47f5-420b-8866-08dce82db53b', N'557095d1-2e87-4474-3121-08dce7aca610', NULL, NULL, NULL, CAST(N'2024-10-09T13:50:52.6775431+00:00' AS DateTimeOffset), CAST(N'2024-10-09T13:50:52.6775431+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (N'8cfff614-d7db-4331-efde-08dce833e592', N'557095d1-2e87-4474-3121-08dce7aca610', NULL, NULL, NULL, CAST(N'2024-10-09T14:27:57.4579299+00:00' AS DateTimeOffset), CAST(N'2024-10-09T14:27:57.4579299+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[AspNetUsers] ([Id], [Password], [UserInfoId], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'7c0023d9-c1c5-49fc-4240-08dce7aca63c', N'', N'85e2dfe0d7d045c2b52918cf5c80b726', NULL, NULL, NULL, CAST(N'2024-10-08T22:19:48.7702710+00:00' AS DateTimeOffset), CAST(N'2024-10-08T22:19:48.7702710+00:00' AS DateTimeOffset), NULL, N'admin', N'ADMIN', N'admin@example.com', N'ADMIN@EXAMPLE.COM', 0, N'AQAAAAIAAYagAAAAEFgCG6X8B7KfqEkNBundBnEgxxDLHjbSrLMhl7Fv7TCzeNk/R+Wo6bygQ89CNM2wgA==', N'DZ5RT6A756GVJI6RHCT4E6SI2BVFJOOJ', N'f0603772-af36-48a7-8159-caffd786f075', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [Password], [UserInfoId], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'56244908-ee29-49df-4241-08dce7aca63c', N'', N'9bf62230453f4aee92c0b96b8d7662e9', NULL, NULL, NULL, CAST(N'2024-10-08T22:19:49.1517578+00:00' AS DateTimeOffset), CAST(N'2024-10-08T22:19:49.1517578+00:00' AS DateTimeOffset), NULL, N'anotherAdmin', N'ANOTHERADMIN', N'anotherAdmin@example.com', N'ANOTHERADMIN@EXAMPLE.COM', 0, N'AQAAAAIAAYagAAAAEBPut/Idt7VfAsaWXZzawkvy6RKAaPeDAbrcH+pJ4XJOCjCosPM25CBDIUIZgj9dsw==', N'Y3YX7CVQT25RD7KR5DMHWMQCIDAY2JVF', N'dfca2561-9f2f-4160-88e4-c381ee19b372', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [Password], [UserInfoId], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'd47648e9-59ea-4139-4242-08dce7aca63c', N'', N'80d003951c734c08a07e77dd357134da', NULL, NULL, NULL, CAST(N'2024-10-08T22:19:49.2526818+00:00' AS DateTimeOffset), CAST(N'2024-10-08T22:19:49.2526818+00:00' AS DateTimeOffset), NULL, N'moderator', N'MODERATOR', N'moderator@example.com', N'MODERATOR@EXAMPLE.COM', 0, N'AQAAAAIAAYagAAAAEMoI9QyfIBT/Wd6RH4lgycxx3wSlXNFHr1zknFJAdP0sZdm3tfmWq0+57YtAlGuB+w==', N'TS4UJOKVQWTCTEPKKHK5YAQ567FMNKOZ', N'1d24fcec-400a-460a-9f88-10ca96518154', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [Password], [UserInfoId], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'ce031a93-5da6-4291-8865-08dce82db53b', N'', N'71de8dbed8e740a9ad0e97316216f470', NULL, NULL, NULL, CAST(N'2024-10-09T13:43:39.0281387+00:00' AS DateTimeOffset), CAST(N'2024-10-09T13:43:39.0281387+00:00' AS DateTimeOffset), NULL, N'VanThang', N'VANTHANG', N'thangvanle@gmail.com', N'THANGVANLE@GMAIL.COM', 0, N'AQAAAAIAAYagAAAAEGEMdtqTD0jBc5o0VuHunOtxSEEMirpzV2m0TcDnWlW6rcHIA/zh1FjFwbSx+cBXrg==', N'5BCFHYSDGDZ6X2VN5KR3OYU56QRAABTS', N'f28402cb-b16b-42fd-aad1-2aa8b0880a95', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [Password], [UserInfoId], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'bde39811-47f5-420b-8866-08dce82db53b', N'', N'1ca1e70d03e345389f168efeed17db49', NULL, NULL, NULL, CAST(N'2024-10-09T13:50:52.5755527+00:00' AS DateTimeOffset), CAST(N'2024-10-09T13:50:52.5755527+00:00' AS DateTimeOffset), NULL, N'Thang', N'THANG', N'Thang@gmail.com', N'THANG@GMAIL.COM', 0, N'AQAAAAIAAYagAAAAEJC8NixbeECXcnIfD5qyBNtMS30CyWOp01/R52aC9MdWYAeCvVloaHc+wRZpiyVbLg==', N'57EY64YAR5YV25R26YXYNZYF3TQARQ6F', N'd2be2e7e-5f2e-48a3-9af5-60424f1e3875', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [Password], [UserInfoId], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'8cfff614-d7db-4331-efde-08dce833e592', N'', N'bb252b8668a048c182693c1a0a812040', NULL, NULL, NULL, CAST(N'2024-10-09T14:27:57.1015694+00:00' AS DateTimeOffset), CAST(N'2024-10-09T14:27:57.1015694+00:00' AS DateTimeOffset), NULL, N'Thinh', N'THINH', N'Thinh@gmail.com', N'THINH@GMAIL.COM', 0, N'AQAAAAIAAYagAAAAENm9wcTcfpWtVVIq2lcD+ClNAiQdoPlx0gCWtCH871xwA94HKKS71AiYUx+k8jG3xw==', N'4SVQULAEOLXMHZWSHAQC3KPQ7RMGHAGH', N'9cade9e1-e761-40a2-8290-d431e4874540', NULL, 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[Categories] ([Id], [Name], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (N'ddfce03389404c89a0e0bdeaf2e57b55', N'Book', N'7c0023d9-c1c5-49fc-4240-08dce7aca63c', NULL, NULL, CAST(N'2024-10-09T13:45:01.7133680' AS DateTime2), CAST(N'2024-10-09T13:45:01.7133680' AS DateTime2), NULL)
GO
INSERT [dbo].[Comments] ([Id], [UserId], [RepCmtId], [ProductId], [CommentText], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [CommentArea]) VALUES (N'5843e54ddb0743f6b0a73582f53e7711', N'ce031a93-5da6-4291-8865-08dce82db53b', N'5843e54ddb0743f6b0a73582f53e7711', N'08ce3ca52684476f92bb25b237aa989b', N'Nhanh tay keo het !!!', N'VanThang', NULL, NULL, CAST(N'2024-10-15T09:16:53.4698188' AS DateTime2), CAST(N'2024-10-15T09:16:53.4698188' AS DateTime2), NULL, N'5843e54ddb0743f6b0a73582f53e7711')
INSERT [dbo].[Comments] ([Id], [UserId], [RepCmtId], [ProductId], [CommentText], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [CommentArea]) VALUES (N'5e4b1cbb9a5e4d18b6e0dbcfb02d405d', N'ce031a93-5da6-4291-8865-08dce82db53b', N'5e4b1cbb9a5e4d18b6e0dbcfb02d405d', N'08ce3ca52684476f92bb25b237aa989b', N'Gia re bat ngo !!!', N'VanThang', NULL, NULL, CAST(N'2024-10-15T09:15:16.4807333' AS DateTime2), CAST(N'2024-10-15T09:15:16.4807333' AS DateTime2), NULL, N'5e4b1cbb9a5e4d18b6e0dbcfb02d405d')
INSERT [dbo].[Comments] ([Id], [UserId], [RepCmtId], [ProductId], [CommentText], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [CommentArea]) VALUES (N'9d1c61e9bcd5415eb42760eb9fdb2b33', N'ce031a93-5da6-4291-8865-08dce82db53b', N'f7ae29246c6e4a499b50871cdcac88b0', N'08ce3ca52684476f92bb25b237aa989b', N'Bạn có thắc mắc gì về sản phẩm ?', N'VanThang', NULL, NULL, CAST(N'2024-10-15T11:11:00.7790755' AS DateTime2), CAST(N'2024-10-15T11:11:00.7790755' AS DateTime2), NULL, N'5843e54ddb0743f6b0a73582f53e7711')
INSERT [dbo].[Comments] ([Id], [UserId], [RepCmtId], [ProductId], [CommentText], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [CommentArea]) VALUES (N'f7ae29246c6e4a499b50871cdcac88b0', N'8cfff614-d7db-4331-efde-08dce833e592', N'5843e54ddb0743f6b0a73582f53e7711', N'08ce3ca52684476f92bb25b237aa989b', N'???', N'Thinh', NULL, NULL, CAST(N'2024-10-15T09:40:49.8558137' AS DateTime2), CAST(N'2024-10-15T09:40:49.8558137' AS DateTime2), NULL, N'5843e54ddb0743f6b0a73582f53e7711')
INSERT [dbo].[Comments] ([Id], [UserId], [RepCmtId], [ProductId], [CommentText], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [CommentArea]) VALUES (N'fda19dd7d137463bba4a2d022c9ed1fe', N'8cfff614-d7db-4331-efde-08dce833e592', N'5e4b1cbb9a5e4d18b6e0dbcfb02d405d', N'08ce3ca52684476f92bb25b237aa989b', N'Con giam gia khong', N'Thinh', NULL, NULL, CAST(N'2024-10-15T10:03:09.2223428' AS DateTime2), CAST(N'2024-10-15T10:03:09.2223428' AS DateTime2), NULL, N'5e4b1cbb9a5e4d18b6e0dbcfb02d405d')
GO
INSERT [dbo].[Exchanges] ([Id], [ProductId], [BuyerId], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (N'8a148023622b4735943f84fb1ad341a4', N'2a965233dbe945e8ba40d619ba540881', N'8cfff614-d7db-4331-efde-08dce833e592', N'ce031a93-5da6-4291-8865-08dce82db53b', NULL, NULL, CAST(N'2024-10-15T08:49:14.1293066' AS DateTime2), CAST(N'2024-10-15T08:49:14.1293066' AS DateTime2), NULL)
GO
INSERT [dbo].[Notifications] ([Id], [UserId], [ProductId], [Content], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (N'3785d3be590e4b9093c8db5fcac38830', N'ce031a93-5da6-4291-8865-08dce82db53b', N'08ce3ca52684476f92bb25b237aa989b', N'Thinh đã phản hồi bình luận của bạn.', NULL, NULL, NULL, CAST(N'2024-10-15T10:03:09.0641347' AS DateTime2), CAST(N'2024-10-15T10:03:09.0641347' AS DateTime2), NULL)
INSERT [dbo].[Notifications] ([Id], [UserId], [ProductId], [Content], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (N'8efa3e42f628476da3a74066882261f8', N'ce031a93-5da6-4291-8865-08dce82db53b', N'2a965233dbe945e8ba40d619ba540881', N'Thinh đã bình luận vào sản phẩm Cuon theo chieu gio của bạn.', NULL, NULL, NULL, CAST(N'2024-10-14T22:11:27.4693773' AS DateTime2), CAST(N'2024-10-14T22:11:27.4693773' AS DateTime2), NULL)
INSERT [dbo].[Notifications] ([Id], [UserId], [ProductId], [Content], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (N'b99c9a8463114265b7429bb65aee9d24', N'ce031a93-5da6-4291-8865-08dce82db53b', N'08ce3ca52684476f92bb25b237aa989b', N'Thinh đã phản hồi bình luận của VanThang', NULL, NULL, NULL, CAST(N'2024-10-15T09:40:49.8515521' AS DateTime2), CAST(N'2024-10-15T09:40:49.8515521' AS DateTime2), NULL)
INSERT [dbo].[Notifications] ([Id], [UserId], [ProductId], [Content], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (N'c6444008a467488f80d2a9e96b21972b', N'ce031a93-5da6-4291-8865-08dce82db53b', N'08ce3ca52684476f92bb25b237aa989b', N'Thinh đã phản hồi bình luận của bạn.', NULL, NULL, NULL, CAST(N'2024-10-15T09:40:49.7139811' AS DateTime2), CAST(N'2024-10-15T09:40:49.7139811' AS DateTime2), NULL)
INSERT [dbo].[Notifications] ([Id], [UserId], [ProductId], [Content], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (N'f57c73cc320849c38f2f3253cd453592', N'8cfff614-d7db-4331-efde-08dce833e592', N'08ce3ca52684476f92bb25b237aa989b', N'VanThang đã phản hồi bình luận của bạn.', NULL, NULL, NULL, CAST(N'2024-10-15T11:11:00.6188893' AS DateTime2), CAST(N'2024-10-15T11:11:00.6188893' AS DateTime2), NULL)
GO
INSERT [dbo].[Products] ([Id], [SellerId], [CategoryId], [Name], [Price], [Description], [Image], [NumberOfStar], [Approve], [Sold], [Rating], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [TotalStar]) VALUES (N'08ce3ca52684476f92bb25b237aa989b', N'ce031a93-5da6-4291-8865-08dce82db53b', N'ddfce03389404c89a0e0bdeaf2e57b55', N'The Lord Of The Rings', 350000, N'like new 90%, still have box', NULL, 2.7, 1, 0, 6, N'VanThang', NULL, NULL, CAST(N'2024-10-09T13:47:36.9977540' AS DateTime2), CAST(N'2024-10-09T13:47:36.9977540' AS DateTime2), NULL, 16)
INSERT [dbo].[Products] ([Id], [SellerId], [CategoryId], [Name], [Price], [Description], [Image], [NumberOfStar], [Approve], [Sold], [Rating], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [TotalStar]) VALUES (N'2a965233dbe945e8ba40d619ba540881', N'ce031a93-5da6-4291-8865-08dce82db53b', N'ddfce03389404c89a0e0bdeaf2e57b55', N'Cuon theo chieu gio', 120000, N'Tac gia Khong Biet', NULL, 0, 1, 1, 0, N'VanThang', NULL, NULL, CAST(N'2024-10-14T22:08:46.2440012' AS DateTime2), CAST(N'2024-10-14T22:08:46.2440012' AS DateTime2), NULL, 0)
GO
INSERT [dbo].[UserInfos] ([Id], [FullName], [BankAccount], [BankAccountName], [Bank], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (N'1ca1e70d03e345389f168efeed17db49', N'Nguyen Van Thang', NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2024-10-09T13:50:52.5755628' AS DateTime2), CAST(N'2024-10-09T13:50:52.5755628' AS DateTime2), NULL)
INSERT [dbo].[UserInfos] ([Id], [FullName], [BankAccount], [BankAccountName], [Bank], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (N'71de8dbed8e740a9ad0e97316216f470', N'Le Van Thang', NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2024-10-09T13:43:39.0281460' AS DateTime2), CAST(N'2024-10-09T13:43:39.0281460' AS DateTime2), NULL)
INSERT [dbo].[UserInfos] ([Id], [FullName], [BankAccount], [BankAccountName], [Bank], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (N'80d003951c734c08a07e77dd357134da', N'Moderator', NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2024-10-08T22:19:49.2526853' AS DateTime2), CAST(N'2024-10-08T22:19:49.2526853' AS DateTime2), NULL)
INSERT [dbo].[UserInfos] ([Id], [FullName], [BankAccount], [BankAccountName], [Bank], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (N'85e2dfe0d7d045c2b52918cf5c80b726', N'Administrator', NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2024-10-08T22:19:48.7706282' AS DateTime2), CAST(N'2024-10-08T22:19:48.7706282' AS DateTime2), NULL)
INSERT [dbo].[UserInfos] ([Id], [FullName], [BankAccount], [BankAccountName], [Bank], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (N'9bf62230453f4aee92c0b96b8d7662e9', N'Administrator2', NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2024-10-08T22:19:49.1517611' AS DateTime2), CAST(N'2024-10-08T22:19:49.1517611' AS DateTime2), NULL)
INSERT [dbo].[UserInfos] ([Id], [FullName], [BankAccount], [BankAccountName], [Bank], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (N'bb252b8668a048c182693c1a0a812040', N'Tran Huu Thinh', NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2024-10-09T14:27:57.1015757' AS DateTime2), CAST(N'2024-10-09T14:27:57.1015757' AS DateTime2), NULL)
GO
ALTER TABLE [dbo].[Comments] ADD  DEFAULT (N'') FOR [CommentArea]
GO
ALTER TABLE [dbo].[Notifications] ADD  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [UserId]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((0)) FOR [TotalStar]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUsers]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUsers_UserInfos_UserInfoId] FOREIGN KEY([UserInfoId])
REFERENCES [dbo].[UserInfos] ([Id])
GO
ALTER TABLE [dbo].[AspNetUsers] CHECK CONSTRAINT [FK_AspNetUsers_UserInfos_UserInfoId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Bans]  WITH CHECK ADD  CONSTRAINT [FK_Bans_Reports_ReportId] FOREIGN KEY([ReportId])
REFERENCES [dbo].[Reports] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Bans] CHECK CONSTRAINT [FK_Bans_Reports_ReportId]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Products_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Products_ProductId]
GO
ALTER TABLE [dbo].[Exchanges]  WITH CHECK ADD  CONSTRAINT [FK_Exchanges_AspNetUsers_BuyerId] FOREIGN KEY([BuyerId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Exchanges] CHECK CONSTRAINT [FK_Exchanges_AspNetUsers_BuyerId]
GO
ALTER TABLE [dbo].[Exchanges]  WITH CHECK ADD  CONSTRAINT [FK_Exchanges_Products_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
GO
ALTER TABLE [dbo].[Exchanges] CHECK CONSTRAINT [FK_Exchanges_Products_ProductId]
GO
ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD  CONSTRAINT [FK_Notifications_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Notifications] CHECK CONSTRAINT [FK_Notifications_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD  CONSTRAINT [FK_Notifications_Products_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Notifications] CHECK CONSTRAINT [FK_Notifications_Products_ProductId]
GO
ALTER TABLE [dbo].[ProductImages]  WITH CHECK ADD  CONSTRAINT [FK_ProductImages_Products_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductImages] CHECK CONSTRAINT [FK_ProductImages_Products_ProductId]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_AspNetUsers_SellerId] FOREIGN KEY([SellerId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_AspNetUsers_SellerId]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Categories_CategoryId] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Categories_CategoryId]
GO
ALTER TABLE [dbo].[Reports]  WITH CHECK ADD  CONSTRAINT [FK_Reports_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Reports] CHECK CONSTRAINT [FK_Reports_AspNetUsers_UserId]
GO
