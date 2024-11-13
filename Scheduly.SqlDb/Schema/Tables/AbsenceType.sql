CREATE TABLE [dbo].[AbsenceType]
(
	[AbsenceTypeID] int not null identity(1,1),
	[Name] nvarchar(50) not null,
	[WageFactor] decimal(9,4),
	[MustBeApproved] bit,
	constraint [PK_dbo_AbsenceType$AbsenceTypeID] primary key clustered (AbsenceTypeID) with (fillfactor = 100),
)
