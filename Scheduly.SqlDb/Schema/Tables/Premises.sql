CREATE TABLE [dbo].[Premises]
(
	[PremiseID] int not null identity(1,1),
	[PremiseCategoryID] int not null,
	[Name] nvarchar(50) not null,
	[Size] nvarchar(10),
	[Description] nvarchar(max),
	[MustBeApproved] bit,
	constraint [PK_dbo_Premises$PremisID] primary key clustered (PremiseID) with (fillfactor = 100),
	constraint [FK_dbo_PremiseCategory$Premises_PremiseCategoryID] foreign key (PremiseCategoryID) references dbo.[PremiseCategory](PremiseCategoryID) on delete cascade,
)
