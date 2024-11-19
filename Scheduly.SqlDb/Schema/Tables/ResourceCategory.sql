CREATE TABLE [dbo].[ResourceCategory]
(
	[CategoryID] int not null identity(1,1),
	[Name] nvarchar(50) not null,
	constraint [PK_dbo_ResourceCategory$CategoryID] primary key clustered (CategoryID) with (fillfactor = 100),
)
