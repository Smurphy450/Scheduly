CREATE TABLE [dbo].[Resources]
(
	[ResourceID] int not null identity(1,1),
	[Name] nvarchar(50) not null,
	[Amount] int,
	[Description] nvarchar(max),
	[MustBeApproved] bit,
	constraint [PK_dbo_Resources$ResourceID] primary key clustered (ResourceID) with (fillfactor = 100),
)
