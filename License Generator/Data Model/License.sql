USE [License]
GO

/****** Object:  Table [dbo].[Customer]    Script Date: 02/22/2012 20:09:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Customer](
	[Address] [nvarchar](256) NULL,
	[City] [nvarchar](128) NULL,
	[Country] [nvarchar](8) NULL,
	[CustomerId] [uniqueidentifier] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
	[Email] [nvarchar](128) NULL,
	[FirstName] [nvarchar](128) NULL,
	[LastName] [nvarchar](128) NULL,
	[Phone] [nvarchar](64) NULL,
	[PostalCode] [nvarchar](32) NULL,
	[Province] [nvarchar](64) NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[License]    Script Date: 02/22/2012 20:10:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[License](
	[DateCreated] [datetime] NOT NULL,
	[CustomerId] [uniqueidentifier] NOT NULL,
	[LicenseId] [uniqueidentifier] NOT NULL,
	[LicenseType] [int] NOT NULL,
	[ProductId] [uniqueidentifier] NOT NULL,
	[SerialNumber] [int] NOT NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[Product]    Script Date: 02/22/2012 20:10:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Product](
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
	[Description] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[ProductId] [uniqueidentifier] NOT NULL
) ON [PRIMARY]

GO
