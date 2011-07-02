-- Creating foreign key on [Fulfills_Id] in table 'Resources'
ALTER TABLE [dbo].[Resources]
ADD CONSTRAINT [FK_ResourceResource]
    FOREIGN KEY ([Fulfills_Id])
    REFERENCES [dbo].[Resources]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;


