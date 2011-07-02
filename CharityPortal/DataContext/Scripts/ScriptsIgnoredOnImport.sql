
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 07/02/2011 16:21:29
-- Generated from EDMX file: D:\trev\Dev\dev4good\CharityPortal\CharityPortal.Data\DataContext.edmx
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
IF OBJECT_ID(N'[dbo].[FK_ResourceResource]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Resources] DROP CONSTRAINT [FK_ResourceResource];
GO
IF OBJECT_ID(N'[dbo].[FK_TagResource_Tag]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TagResource] DROP CONSTRAINT [FK_TagResource_Tag];
GO
IF OBJECT_ID(N'[dbo].[FK_TagResource_Resource]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TagResource] DROP CONSTRAINT [FK_TagResource_Resource];
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
IF OBJECT_ID(N'[dbo].[TagResource]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TagResource];
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------

GO
