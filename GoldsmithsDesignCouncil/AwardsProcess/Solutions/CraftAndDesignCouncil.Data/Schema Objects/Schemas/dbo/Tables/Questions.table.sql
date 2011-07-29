CREATE TABLE [dbo].[Questions]
(
	QuestionId int NOT NULL identity(1,1), 
	QuestionText NVarChar(500) not NULL,
	HelpText NVarChar(500) Null,
	ApplicationFormSectionId int not null
)
