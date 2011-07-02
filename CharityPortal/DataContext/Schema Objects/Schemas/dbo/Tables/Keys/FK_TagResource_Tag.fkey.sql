-- Creating foreign key on [Tag_Id] in table 'TagResource'
ALTER TABLE [dbo].[TagResource]
ADD CONSTRAINT [FK_TagResource_Tag]
    FOREIGN KEY ([Tag_Id])
    REFERENCES [dbo].[Tags]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;


