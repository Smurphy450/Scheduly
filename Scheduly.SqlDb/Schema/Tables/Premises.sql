CREATE TABLE [dbo].[Premises]
(
	[PremisID] int not null identity(1,1),
	[PremisCategoryID] int not null,
	[Name] nvarchar(50) not null,
	[Size] nvarchar(10),
	[Description] nvarchar(max),
	[MustBeApproved] bit,
	constraint [PK_dbo_Premises$PremisID] primary key clustered (PremisID) with (fillfactor = 100),
	constraint [FK_dbo_PremisCategory$Premises_PremisCategoryID] foreign key (PremisCategoryID) references dbo.[PremisCategory](PremisCategoryID) on delete cascade,
)
