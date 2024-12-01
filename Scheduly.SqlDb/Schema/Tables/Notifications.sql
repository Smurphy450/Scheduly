CREATE TABLE [dbo].[Notifications]
(
	[NotificationID] int not null identity(1,1),
	[UserID] int not null,
	[SMS] bit null,
	[Email] bit null,
	[Message] nvarchar(255),
	constraint [PK_dbo_Notifications$NotificationID] primary key clustered (NotificationID) with (fillfactor = 100),
	constraint [FK_dbo_User$Notifications_UserID] foreign key (UserID) references dbo.[User](UserID) on delete cascade,
)
