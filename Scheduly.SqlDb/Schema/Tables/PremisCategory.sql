CREATE TABLE [dbo].[PremisCategory]
(
	[PremisCategoryID] int not null identity(1,1),
	[Name] nvarchar(50) not null,
	constraint [PK_dbo_PremisCategory$PremisCategoryID] primary key clustered (PremisCategoryID) with (fillfactor = 100),
)
