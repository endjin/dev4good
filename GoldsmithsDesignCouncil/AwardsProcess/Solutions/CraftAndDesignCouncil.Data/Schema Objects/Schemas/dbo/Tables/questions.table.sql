CREATE TABLE [dbo].[questions]
(
	id int NOT NULL identity(0,1), 
	LongText NvarChar(500) not null,
	HelpText NvarChar(500) not null

)
