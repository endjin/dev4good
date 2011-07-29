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


if exists (select 1 from information_schema.Tables where table_name = 'QuestionSection' and TABLE_SCHEMA = 'dbo')
begin
	print 'ApplicationFormSection table found'
	truncate table dbo.ApplicationFormSection;
	set identity_insert dbo.ApplicationFormSection off
	insert into dbo.ApplicationFormSection(ApplicationFormSectionId, Title, OrderingKey) values (1, 'Craft Section', 1);
	insert into dbo.ApplicationFormSection(ApplicationFormSectionId, Title, OrderingKey) values (2, 'Applicant level', 2);
	insert into dbo.ApplicationFormSection(ApplicationFormSectionId, Title, OrderingKey) values (3, 'Trainee details', 3);  --only if trainee
	insert into dbo.ApplicationFormSection(ApplicationFormSectionId, Title, OrderingKey) values (4, 'Student', 4);
	insert into dbo.ApplicationFormSection(ApplicationFormSectionId, Title, OrderingKey) values (5, 'Bursary & Scholarship', 5); --only if student = yes
	set identity_insert dbo.ApplicationFormSection off
end

begin
	print 'question table found'
	truncate table dbo.Questions;
	insert into dbo.Questions (QuestionText, HelpText, ApplicationFormSectionId) values ('Please select your craft section', 'Please select the craft section you think best describes your skills', 1);
	insert into dbo.Questions (QuestionText, HelpText, ApplicationFormSectionId) values ('Please select your level', 'Please select your level. If you are currently studying or a recent graduate or under the age of 30 please selet "apprentice/trainee".  If you are older than 30 please select "senior"', 2);
	insert into dbo.Questions (QuestionText, HelpText, ApplicationFormSectionId) values ('Age', 'Please enter your age as it will be on the day of judging', 3);
	insert into dbo.Questions (QuestionText, HelpText, ApplicationFormSectionId) values ('Master', 'Please enter the name of the craftsman by whom you were taught', 3);
	insert into dbo.Questions (QuestionText, HelpText, ApplicationFormSectionId) values ('Are you a student', 'Please select either yes or no', 4);
	insert into dbo.Questions (QuestionText, HelpText, ApplicationFormSectionId) values ('The Gil Packard Post Graduate Bursary (£1000)', 'Please select yes or no',5);
	insert into dbo.Questions (QuestionText, HelpText, ApplicationFormSectionId) values ('Gem-A Diamond Scholarship', 'Please select either yes or no', 5);
	 
end