IF SCHEMA_ID(N'test') IS NULL EXEC(N'CREATE SCHEMA [test];');
GO


CREATE TABLE [test].[FutureMarket] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(256) NOT NULL,
    [ContractSize] decimal(20,10) NOT NULL,
    CONSTRAINT [PK_FutureMarket] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [test].[JsonData] (
    [Id] int NOT NULL IDENTITY,
    [Rule] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_JsonData] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [test].[TickSize] (
    [StartDate] datetime2 NOT NULL,
    [MarketId] int NOT NULL,
    [Value] decimal(20,10) NOT NULL,
    [EndDate] datetime2 NOT NULL,
    CONSTRAINT [PK_TickSize] PRIMARY KEY NONCLUSTERED ([MarketId], [StartDate]),
    CONSTRAINT [FK_TickSize_FutureMarket_MarketId] FOREIGN KEY ([MarketId]) REFERENCES [test].[FutureMarket] ([Id]) ON DELETE CASCADE
);
GO


CREATE CLUSTERED INDEX [IX_TickSize_MarketId_StartDate_EndDate] ON [test].[TickSize] ([MarketId], [StartDate], [EndDate]);
GO



