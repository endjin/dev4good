/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

if exists (select 1 from information_schema.Tables where table_name = 'Questions' and TABLE_SCHEMA = 'dbo')
begin
	print 'question table found'
	truncate table dbo.Questions;
	insert into dbo.Questions (QuestionText, HelpText) values ('This is the first question', 'This is the help text');
	insert into dbo.Questions (QuestionText, HelpText) values ('This is the second question', 'This is the help text');
	insert into dbo.Questions (QuestionText, HelpText) values ('This is the third question', 'This is the help text');
end