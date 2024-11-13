CREATE TABLE [dbo].[User]
(
	[UserID] int not null identity(1,1),
	[Username] nvarchar(50) not null,
	[PasswordHash] nvarchar(255) not null,
	[Email] nvarchar(100) not null,
	constraint [PK_dbo_User$UserID] primary key clustered (UserID) with (fillfactor = 100),
)
