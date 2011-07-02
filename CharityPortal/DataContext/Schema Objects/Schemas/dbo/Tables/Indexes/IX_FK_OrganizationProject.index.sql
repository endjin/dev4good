-- Creating non-clustered index for FOREIGN KEY 'FK_OrganizationProject'
CREATE INDEX [IX_FK_OrganizationProject]
ON [dbo].[Projects]
    ([AdminOrganization_Id]);


