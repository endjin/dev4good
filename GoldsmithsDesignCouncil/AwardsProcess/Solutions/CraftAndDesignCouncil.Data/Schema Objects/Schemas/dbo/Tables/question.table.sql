CREATE TABLE [dbo].[question]
(
	Id int NOT NULL identity(1,1),
	LongText NVarChar(500) Not NULL,
	HelpText NVarChar(500) not null
)
