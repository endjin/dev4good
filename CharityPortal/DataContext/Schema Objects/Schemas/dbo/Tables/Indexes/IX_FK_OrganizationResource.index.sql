-- Creating non-clustered index for FOREIGN KEY 'FK_OrganizationResource'
CREATE INDEX [IX_FK_OrganizationResource]
ON [dbo].[Resources]
    ([Organization_Id]);


