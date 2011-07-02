-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [AdminOrganization_Id] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [FK_OrganizationProject]
    FOREIGN KEY ([AdminOrganization_Id])
    REFERENCES [dbo].[Organizations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;


