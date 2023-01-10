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
    [MarketId] int NOT NULL,
    [StartDate] datetime2 NOT NULL,
    [Value] decimal(20,10) NOT NULL,
    [EndDate] datetime2 NOT NULL,
    [CreatedUtc] datetime2 NOT NULL,
    [CreatedBy] nvarchar(128) NOT NULL,
    [ModifiedUtc] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(128) NOT NULL,
    CONSTRAINT [PK_TickSize] PRIMARY KEY CLUSTERED ([MarketId], [StartDate]),
    CONSTRAINT [FK_TickSize_FutureMarket_MarketId] FOREIGN KEY ([MarketId]) REFERENCES [test].[FutureMarket] ([Id]) ON DELETE CASCADE
);
GO


