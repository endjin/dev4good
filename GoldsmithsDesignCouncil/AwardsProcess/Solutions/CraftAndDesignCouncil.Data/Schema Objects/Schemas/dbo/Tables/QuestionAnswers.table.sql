CREATE TABLE [dbo].[QuestionAnswers]
(
	QuestionAnswerId int NOT NULL identity (1,1) primary key, 
	AnswerText nvarchar(1000) not null,
	QuestionId int not null,
	ApplicationFormId int not null
)
