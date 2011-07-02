-- Creating foreign key on [Resources_Id] in table 'TagResource'
ALTER TABLE [dbo].[TagResource]
ADD CONSTRAINT [FK_TagResource_Resource]
    FOREIGN KEY ([Resources_Id])
    REFERENCES [dbo].[Resources]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;


