IF SCHEMA_ID(N'sam') IS NULL EXEC(N'CREATE SCHEMA [sam];');
GO


CREATE TABLE [sam].[EntityInADiffNameSpace] (
    [Id] int NOT NULL IDENTITY,
    CONSTRAINT [PK_EntityInADiffNameSpace] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [sam].[Market] (
    [Id] int NOT NULL IDENTITY,
    [Name] varchar(128) NOT NULL,
    CONSTRAINT [PK_Market] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [sam].[MyData] (
    [Id] int NOT NULL IDENTITY,
    [Property] varchar(max) NOT NULL,
    CONSTRAINT [PK_MyData] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [sam].[ContractSpec] (
    [Id] int NOT NULL IDENTITY,
    [MarketId] int NOT NULL,
    [Value] decimal(20,10) NOT NULL,
    [StartDate] datetime2 NOT NULL,
    [EndDate] datetime2 NOT NULL,
    CONSTRAINT [PK_ContractSpec] PRIMARY KEY NONCLUSTERED ([Id]),
    CONSTRAINT [FK_ContractSpec_Market_MarketId] FOREIGN KEY ([MarketId]) REFERENCES [sam].[Market] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [sam].[SpreadSpec] (
    [Id] int NOT NULL IDENTITY,
    [MarketId] int NOT NULL,
    [Value] decimal(20,10) NOT NULL,
    [StartDate] date NOT NULL,
    [EndDate] date NOT NULL,
    CONSTRAINT [PK_SpreadSpec] PRIMARY KEY NONCLUSTERED ([Id]),
    CONSTRAINT [FK_SpreadSpec_Market_MarketId] FOREIGN KEY ([MarketId]) REFERENCES [sam].[Market] ([Id]) ON DELETE CASCADE
);
GO


CREATE UNIQUE CLUSTERED INDEX [IX_ContractSpec_MarketId_StartDate] ON [sam].[ContractSpec] ([MarketId], [StartDate]);
GO


CREATE UNIQUE CLUSTERED INDEX [IX_SpreadSpec_MarketId_StartDate] ON [sam].[SpreadSpec] ([MarketId], [StartDate]);
GO



