CREATE DATABASE bugtracker ON 
( NAME = N'PrimaryFileName', FILENAME = N'C:\prj\Listma\trunk\Samples\BugTracker\BugTracker\App_Data\bugtracker.mdf')
 LOG ON ( NAME = N'PrimaryLogFileName', FILENAME = N'C:\prj\Listma\trunk\Samples\BugTracker\BugTracker\App_Data\bugtracker_log.ldf')
GO

use bugtracker

CREATE TABLE [Project] (
	[Id] [int] NOT NULL ,
	[Name] [nvarchar] (100)  NOT NULL ,
	[State] [nvarchar] (50)  NULL ,
	[StartDate] [datetime] NULL 
) 
GO

CREATE TABLE [ProjectTeam] (
	[Id] [int] IDENTITY (1, 1) NOT NULL ,
	[ProjectId] [int] NOT NULL ,
	[UserId] [int] NOT NULL ,
	[RoleId] [int] NOT NULL 
) 
GO

CREATE TABLE [Role] (
	[Id] [int] IDENTITY (1, 1) NOT NULL ,
	[Name] [nvarchar] (50)  NOT NULL 
) 
GO

CREATE TABLE [User] (
	[Id] [int] IDENTITY (1, 1) NOT NULL ,
	[Name] [nvarchar] (100)  NOT NULL ,
	[Email] [nvarchar] (100)  NOT NULL ,
	[Pwdhash] [nvarchar] (100)  NULL 
) 
GO

CREATE TABLE [Issue] (
	[Id] [int] IDENTITY (1, 1) NOT NULL ,
	[Short] [nvarchar] (200)  NOT NULL ,
	[Description] [ntext]  NULL ,
	[State] [nvarchar] (50)  NOT NULL ,
	[CreateDate] [datetime] NULL ,
	[OwnerId] [int] NOT NULL ,
	[AccignedTo] [int] NULL ,
	[StatechartId] [nvarchar] (50)  NULL ,
	[ProjectId] [int] NOT NULL 
)  
GO

ALTER TABLE [ProjectTeam] ADD 
	CONSTRAINT [PK_ProjectTeam] PRIMARY KEY  CLUSTERED 
	(
		[Id]
	)   
GO

ALTER TABLE [Role] ADD 
	CONSTRAINT [PK_Role] PRIMARY KEY  CLUSTERED 
	(
		[Id]
	)   
GO

ALTER TABLE [User] ADD 
	CONSTRAINT [PK_User] PRIMARY KEY  CLUSTERED 
	(
		[Id]
	)   
GO

ALTER TABLE [Issue] ADD 
	CONSTRAINT [PK_Issue] PRIMARY KEY  CLUSTERED 
	(
		[Id]
	)   
GO

ALTER TABLE [Project] ADD 
	CONSTRAINT [FK_Project_ProjectTeam] FOREIGN KEY 
	(
		[Id]
	) REFERENCES [ProjectTeam] (
		[Id]
	) ON DELETE CASCADE ,
	CONSTRAINT [FK_Project_Issue] FOREIGN KEY 
	(
		[Id]
	) REFERENCES [Issue] (
		[Id]
	)
GO

ALTER TABLE [ProjectTeam] ADD 
	CONSTRAINT [FK_ProjectTeam_Role] FOREIGN KEY 
	(
		[RoleId]
	) REFERENCES [Role] (
		[Id]
	),
	CONSTRAINT [FK_ProjectTeam_User] FOREIGN KEY 
	(
		[UserId]
	) REFERENCES [User] (
		[Id]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

ALTER TABLE [Issue] ADD 
	CONSTRAINT [FK_Issue_User] FOREIGN KEY 
	(
		[OwnerId]
	) REFERENCES [User] (
		[Id]
	),
	CONSTRAINT [FK_Issue_User1] FOREIGN KEY 
	(
		[AccignedTo]
	) REFERENCES [User] (
		[Id]
	) NOT FOR REPLICATION 
GO

alter table [Issue] nocheck constraint [FK_Issue_User1]
GO

