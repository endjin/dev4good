
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 07/03/2011 14:07:50
-- Generated from EDMX file: C:\dev4good\CharityPortal\CharityPortal.Data\DataContext.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [charityportal];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_OrganizationProject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_OrganizationProject];
GO
IF OBJECT_ID(N'[dbo].[FK_ProjectResource]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Resources] DROP CONSTRAINT [FK_ProjectResource];
GO
IF OBJECT_ID(N'[dbo].[FK_OrganizationResource]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Resources] DROP CONSTRAINT [FK_OrganizationResource];
GO
IF OBJECT_ID(N'[dbo].[FK_TagResource_Tag]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TagResource] DROP CONSTRAINT [FK_TagResource_Tag];
GO
IF OBJECT_ID(N'[dbo].[FK_TagResource_Resource]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TagResource] DROP CONSTRAINT [FK_TagResource_Resource];
GO
IF OBJECT_ID(N'[dbo].[FK_ResourceResource]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Resources] DROP CONSTRAINT [FK_ResourceResource];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Projects]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Projects];
GO
IF OBJECT_ID(N'[dbo].[Resources]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Resources];
GO
IF OBJECT_ID(N'[dbo].[Organizations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Organizations];
GO
IF OBJECT_ID(N'[dbo].[Tags]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tags];
GO
IF OBJECT_ID(N'[dbo].[AppSettings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AppSettings];
GO
IF OBJECT_ID(N'[dbo].[TagResource]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TagResource];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Projects'
CREATE TABLE [dbo].[Projects] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(255)  NOT NULL,
    [Description] nvarchar(255)  NOT NULL,
    [IsActive] bit  NOT NULL,
    [Location_Longitude] float  NOT NULL,
    [Location_Latitude] float  NOT NULL,
    [Location_Address] nvarchar(max)  NOT NULL,
    [AdminOrganization_Id] int  NULL
);
GO

-- Creating table 'Resources'
CREATE TABLE [dbo].[Resources] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(255)  NOT NULL,
    [Description] nvarchar(255)  NOT NULL,
    [Quantity] float  NOT NULL,
    [QuantityUnits] nvarchar(255)  NOT NULL,
    [Location_Longitude] float  NOT NULL,
    [Location_Latitude] float  NOT NULL,
    [Location_Address] nvarchar(max)  NOT NULL,
    [ImageUrl] nvarchar(255)  NULL,
    [Project_Id] int  NULL,
    [Organization_Id] int  NULL,
    [FulfilledBy_Id] bigint  NULL
);
GO

-- Creating table 'Organizations'
CREATE TABLE [dbo].[Organizations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(255)  NOT NULL,
    [ContactEmail] nvarchar(255)  NOT NULL
);
GO

-- Creating table 'Tags'
CREATE TABLE [dbo].[Tags] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(255)  NOT NULL
);
GO

-- Creating table 'AppSettings'
CREATE TABLE [dbo].[AppSettings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(255)  NOT NULL,
    [Value] nvarchar(255)  NOT NULL
);
GO

-- Creating table 'TagResource'
CREATE TABLE [dbo].[TagResource] (
    [Tags_Id] int  NOT NULL,
    [Resources_Id] bigint  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [PK_Projects]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Resources'
ALTER TABLE [dbo].[Resources]
ADD CONSTRAINT [PK_Resources]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Organizations'
ALTER TABLE [dbo].[Organizations]
ADD CONSTRAINT [PK_Organizations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Tags'
ALTER TABLE [dbo].[Tags]
ADD CONSTRAINT [PK_Tags]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AppSettings'
ALTER TABLE [dbo].[AppSettings]
ADD CONSTRAINT [PK_AppSettings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Tags_Id], [Resources_Id] in table 'TagResource'
ALTER TABLE [dbo].[TagResource]
ADD CONSTRAINT [PK_TagResource]
    PRIMARY KEY NONCLUSTERED ([Tags_Id], [Resources_Id] ASC);
GO

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

-- Creating non-clustered index for FOREIGN KEY 'FK_OrganizationProject'
CREATE INDEX [IX_FK_OrganizationProject]
ON [dbo].[Projects]
    ([AdminOrganization_Id]);
GO

-- Creating foreign key on [Project_Id] in table 'Resources'
ALTER TABLE [dbo].[Resources]
ADD CONSTRAINT [FK_ProjectResource]
    FOREIGN KEY ([Project_Id])
    REFERENCES [dbo].[Projects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProjectResource'
CREATE INDEX [IX_FK_ProjectResource]
ON [dbo].[Resources]
    ([Project_Id]);
GO

-- Creating foreign key on [Organization_Id] in table 'Resources'
ALTER TABLE [dbo].[Resources]
ADD CONSTRAINT [FK_OrganizationResource]
    FOREIGN KEY ([Organization_Id])
    REFERENCES [dbo].[Organizations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_OrganizationResource'
CREATE INDEX [IX_FK_OrganizationResource]
ON [dbo].[Resources]
    ([Organization_Id]);
GO

-- Creating foreign key on [Tags_Id] in table 'TagResource'
ALTER TABLE [dbo].[TagResource]
ADD CONSTRAINT [FK_TagResource_Tag]
    FOREIGN KEY ([Tags_Id])
    REFERENCES [dbo].[Tags]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Resources_Id] in table 'TagResource'
ALTER TABLE [dbo].[TagResource]
ADD CONSTRAINT [FK_TagResource_Resource]
    FOREIGN KEY ([Resources_Id])
    REFERENCES [dbo].[Resources]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TagResource_Resource'
CREATE INDEX [IX_FK_TagResource_Resource]
ON [dbo].[TagResource]
    ([Resources_Id]);
GO

-- Creating foreign key on [FulfilledBy_Id] in table 'Resources'
ALTER TABLE [dbo].[Resources]
ADD CONSTRAINT [FK_ResourceResource]
    FOREIGN KEY ([FulfilledBy_Id])
    REFERENCES [dbo].[Resources]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ResourceResource'
CREATE INDEX [IX_FK_ResourceResource]
ON [dbo].[Resources]
    ([FulfilledBy_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------