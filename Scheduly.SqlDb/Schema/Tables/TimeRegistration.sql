CREATE TABLE [dbo].[TimeRegistration]
(
	[TimeID] int not null identity(1,1),
	[UserID] int not null,
	[Start] datetimeoffset(2) not null constraint [DF_dbo_TimeRegistration$Start] default (sysdatetimeoffset()),
	[End] datetimeoffset(2) constraint [DF_dbo_TimeRegistration$End] default (sysdatetimeoffset()),
	constraint [PK_dbo_TimeRegistration$TimeID] primary key clustered (TimeID) with (fillfactor = 100),
	constraint [FK_dbo_TimeRegistration$Bookings_UserID] foreign key (UserID) references dbo.[User](UserID) on delete cascade,
)
