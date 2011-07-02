-- Creating non-clustered index for FOREIGN KEY 'FK_ResourceResource'
CREATE INDEX [IX_FK_ResourceResource]
ON [dbo].[Resources]
    ([Fulfills_Id]);


