CREATE DATABASE [DB_Webgallery]
GO

USE [DB_Webgallery]
GO

CREATE TABLE [dbo].[Admin](
	[Id] [int] IDENTITY(1000,1) NOT NULL,
	[AdminName] [nvarchar](100) NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](100) NOT NULL
	CONSTRAINT [PK_Admin] PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Customer](
	[Id] [int] IDENTITY(5000,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](250) NOT NULL,
	[Phoneno] [nvarchar](25) NULL,
	[Attach] [nvarchar](50) NULL,
	[Size] [nvarchar](50) NULL,
	[Pack] [nvarchar](50) NULL,
	[Message] [nvarchar](MAX) NULL
	CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[MainImage] (
	[Id] [int] IDENTITY(100,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[IsActive] [bit] NOT NULL DEFAULT 1
	CONSTRAINT [PK_MainImage] PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[SubImage](
	[Id] [int] IDENTITY(100,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[MainImageId] [int] NOT NULL
	CONSTRAINT [PK_SubImage] PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SubImage]     
ADD CONSTRAINT FK_MainImage_ImageId FOREIGN KEY (MainImageId)     
    REFERENCES [dbo].[MainImage] (Id)     
 
GO

ALTER TABLE [dbo].[MainImage] 
ADD CustomerId int NOT NULL DEFAULT(0)
GO

ALTER TABLE [dbo].[MainImage]     
ADD CONSTRAINT FK_MainImage_CustomerId FOREIGN KEY (CustomerId)     
    REFERENCES [dbo].[Customer] (Id)     
 
GO

INSERT INTO [Admin] (AdminName, UserName, [Password]) VALUES ('Admin', 'admin', 'admin123#')
GO

CREATE TABLE [dbo].[Feedback] (
	[Id] [int] IDENTITY(1000,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Comment] NVARCHAR(MAX) NULL,
	[Date] DATETIME2 not null,
	CONSTRAINT [PK_Feedback] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO