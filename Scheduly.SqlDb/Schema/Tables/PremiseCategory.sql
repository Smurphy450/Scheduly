CREATE TABLE [dbo].[PremiseCategory]
(
	[PremiseCategoryID] int not null identity(1,1),
	[Name] nvarchar(50) not null,
	constraint [PK_dbo_PremiseCategory$PremiseCategoryID] primary key clustered (PremiseCategoryID) with (fillfactor = 100),
)
