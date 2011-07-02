-- Creating non-clustered index for FOREIGN KEY 'FK_ProjectResource'
CREATE INDEX [IX_FK_ProjectResource]
ON [dbo].[Resources]
    ([Project_Id]);


