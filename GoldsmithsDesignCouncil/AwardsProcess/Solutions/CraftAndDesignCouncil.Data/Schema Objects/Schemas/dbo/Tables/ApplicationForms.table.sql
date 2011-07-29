CREATE TABLE [dbo].[ApplicationForms]
(
	ApplicationFormId int NOT NULL identity(1,1) primary key,
	ApplicantId int null,
	StartedOn datetime Not Null
)
