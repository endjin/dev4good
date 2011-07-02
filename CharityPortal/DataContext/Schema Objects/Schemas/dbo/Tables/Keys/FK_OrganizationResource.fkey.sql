-- Creating foreign key on [Organization_Id] in table 'Resources'
ALTER TABLE [dbo].[Resources]
ADD CONSTRAINT [FK_OrganizationResource]
    FOREIGN KEY ([Organization_Id])
    REFERENCES [dbo].[Organizations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;


