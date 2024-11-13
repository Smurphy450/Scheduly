CREATE TABLE [dbo].[Premises]
(
	[PremisID] int not null identity(1,1),
	[Name] nvarchar(50) not null,
	[Size] nvarchar(10),
	[Description] nvarchar(max),
	[MustBeApproved] datetimeoffset(2),
	constraint [PK_dbo_Premises$PremisID] primary key clustered (PremisID) with (fillfactor = 100),
)
