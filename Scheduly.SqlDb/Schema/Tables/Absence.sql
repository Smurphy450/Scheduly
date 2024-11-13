CREATE TABLE [dbo].[Absence]
(
	[AbsenceID] int not null identity(1,1),
	[AbsenceTypeID] int not null,
	[UserID] int not null,
	[Start] datetimeoffset(2) not null constraint [DF_dbo_Absence$Start] default (sysdatetimeoffset()),
	[End] datetimeoffset(2) constraint [DF_dbo_Absence$End] default (sysdatetimeoffset()),
	[Description] nvarchar(max),
	[Approved] bit,
	constraint [PK_dbo_Absence$AbsenceID] primary key clustered (AbsenceID) with (fillfactor = 100),
	constraint [FK_dbo_User$Absence_UserID] foreign key (UserID) references dbo.[User](UserID) on delete cascade,
	constraint [FK_dbo_AbsenceType$Absence_AbsenceTypeID] foreign key (AbsenceTypeID) references dbo.[AbsenceType](AbsenceTypeID) on delete cascade,
)
