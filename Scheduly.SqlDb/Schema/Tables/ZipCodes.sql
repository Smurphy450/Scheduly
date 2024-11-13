CREATE TABLE [dbo].[ZipCodes]
(
	[ZipCode] int not null,
	[City] nvarchar(100) not null,
	constraint [PK_dbo_ZipCodes$ZipCode] primary key clustered (ZipCode) with (fillfactor = 100),
)
