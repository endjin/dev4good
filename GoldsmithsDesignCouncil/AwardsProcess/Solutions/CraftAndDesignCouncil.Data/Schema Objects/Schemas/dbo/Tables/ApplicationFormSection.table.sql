CREATE TABLE [dbo].[ApplicationFormSection]
(
	ApplicationFormSectionId int NOT NULL identity (1,1) primary key, 
	Title NVarChar(255) not null,
	OrderingKey int not null,
	NotRequiredIfQuestionId int,
	NotRequiredIfAnswer NVarchar(255)
)
