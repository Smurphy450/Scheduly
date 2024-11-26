CREATE TABLE [dbo].[Bookings]
(
	[BookingsID] int not null identity(1,1),
	[UserID] int not null,
	[PremiseID] int,
	[ResourceID] int,
	[Start] datetimeoffset(2) not null constraint [DF_dbo_Bookings$Start] default (sysdatetimeoffset()),
	[End] datetimeoffset(2) constraint [DF_dbo_Bookings$End] default (sysdatetimeoffset()),
	[Approved] bit,
	constraint [PK_dbo_Bookings$BookingsID] primary key clustered (BookingsID) with (fillfactor = 100),
	constraint [FK_dbo_User$Bookings_UserID] foreign key (UserID) references dbo.[User](UserID) on delete cascade,
	constraint [FK_dbo_Premises$Bookings_PremiseID] foreign key (PremiseID) references dbo.[Premises](PremiseID) on delete cascade,
	constraint [FK_dbo_Resources$Bookings_ResourceID] foreign key (ResourceID) references dbo.[Resources](ResourceID) on delete cascade,
)
