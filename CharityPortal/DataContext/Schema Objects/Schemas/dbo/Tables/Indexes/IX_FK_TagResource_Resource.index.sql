-- Creating non-clustered index for FOREIGN KEY 'FK_TagResource_Resource'
CREATE INDEX [IX_FK_TagResource_Resource]
ON [dbo].[TagResource]
    ([Resources_Id]);


