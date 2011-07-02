-- Creating table 'Resources'
CREATE TABLE [dbo].[Resources] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Quantity] float  NOT NULL,
    [QuantityUnits] nvarchar(max)  NOT NULL,
    [Location_Longitude] float  NOT NULL,
    [Location_Latitude] float  NOT NULL,
    [Location_Address] nvarchar(max)  NOT NULL,
    [Project_Id] int  NOT NULL,
    [Organization_Id] int  NOT NULL,
    [Fulfills_Id] bigint  NOT NULL
);


