-- Creating primary key on [Tag_Id], [Resources_Id] in table 'TagResource'
ALTER TABLE [dbo].[TagResource]
ADD CONSTRAINT [PK_TagResource]
    PRIMARY KEY NONCLUSTERED ([Tag_Id], [Resources_Id] ASC);


