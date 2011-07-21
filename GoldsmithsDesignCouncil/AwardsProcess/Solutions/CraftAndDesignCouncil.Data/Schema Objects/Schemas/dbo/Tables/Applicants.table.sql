CREATE TABLE [dbo].[Applicants]
(
	ApplicantId int NOT NULL identity(1,1) primary key,
	FirstName NVarChar(255) NULL,
	Lastname NVarChar(255) NULL,
	Email NVarChar(500) NOT NULL,
	Password VarChar(50) NOT NULL,
	AddressId int  NULL, 
	ModifiedDate DateTime NOT NULL
)
