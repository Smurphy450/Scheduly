CREATE TABLE [dbo].[AdminSettings]
(
	[SettingsID] int not null identity(1,1),
	[Name] nvarchar(50) not null,
	[Enabled] bit,
	constraint [PK_dbo_AdminSettings$SettingsID] primary key clustered (SettingsID) with (fillfactor = 100),
)
