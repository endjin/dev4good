CREATE TABLE [dbo].[Addresses]
(
	AddressId int not null identity(1,1) primary key,
	AddressLine1 Nvarchar(255) not null,
	AddressLine2 Nvarchar(255) null,
	City Nvarchar(60) not null,
	County Nvarchar(60) not null,
	Postcode varchar(12) not null,
	ModifiedDate datetime not null
)
