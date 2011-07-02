-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Projects'
CREATE TABLE [dbo].[Projects] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [IsActive] bit  NOT NULL,
    [Location_Longitude] float  NOT NULL,
    [Location_Latitude] float  NOT NULL,
    [Location_Address] nvarchar(max)  NOT NULL,
    [AdminOrganization_Id] int  NOT NULL
);


