CREATE TABLE [dbo].[SchedulyLogging]
(
	[LogID] int not null identity(1,1),
	[UserID] int not null,
	[Action] nvarchar(20) not null,
	[AffectedData] nvarchar(max) not null,
	constraint [PK_dbo_SchedulyLogging$LogID] primary key clustered (LogID) with (fillfactor = 100),
	constraint [FK_dbo_User$SchedulyLogging_UserID] foreign key (UserID) references dbo.[User](UserID) on delete cascade,
)
