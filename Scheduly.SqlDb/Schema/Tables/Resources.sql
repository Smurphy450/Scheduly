CREATE TABLE [dbo].[Resources]
(
	[ResourceID] int not null identity(1,1),
	[CategoryID] int not null,
	[Name] nvarchar(50) not null,
	[Amount] int,
	[Description] nvarchar(max),
	[MustBeApproved] bit,
	constraint [PK_dbo_Resources$ResourceID] primary key clustered (ResourceID) with (fillfactor = 100),
	constraint [FK_dbo_ResourceCategory$Resources_CategoryID] foreign key (CategoryID) references dbo.[ResourceCategory](CategoryID) on delete cascade,
)
