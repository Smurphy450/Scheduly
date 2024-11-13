CREATE TABLE [dbo].[Profiles]
(
	[ProfileID] int not null identity(1,1),
	[UserID] int,
	[FirstName] nvarchar(255) not null,
	[LastName] nvarchar(100) not null,
	[Address] nvarchar(200),
	[ZipCode] int,
	[PhoneNumber] nvarchar(16),
	[Admin] bit,
	constraint [PK_dbo_Profiles$ProfileID] primary key clustered (ProfileID) with (fillfactor = 100),
	constraint [FK_dbo_User$Profiles_UserID] foreign key (UserID) references dbo.[User](UserID) on delete cascade,
	constraint [FK_dbo_ZipCodes$Profiles_ZipCode] foreign key (ZipCode) references dbo.[ZipCodes](ZipCode) on delete cascade,
)
