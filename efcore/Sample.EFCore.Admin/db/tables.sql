IF SCHEMA_ID(N'sam') IS NULL EXEC(N'CREATE SCHEMA [sam];');
GO


CREATE TABLE [sam].[Data2] (
    [Id] int NOT NULL IDENTITY,
    [Name] varchar(900) NOT NULL,
    [Property] varchar(max) NULL,
    [PeriodEnd] datetime2 GENERATED ALWAYS AS ROW END HIDDEN NOT NULL,
    [PeriodStart] datetime2 GENERATED ALWAYS AS ROW START HIDDEN NOT NULL,
    CONSTRAINT [PK_Data2] PRIMARY KEY ([Id]),
    CONSTRAINT [AK_Data2_Name] UNIQUE ([Name]),
    PERIOD FOR SYSTEM_TIME([PeriodStart], [PeriodEnd])
) WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [sam].[Data2History]));
GO


CREATE TABLE [sam].[Data3] (
    [Id] int NOT NULL IDENTITY,
    [Name] varchar(900) NOT NULL,
    [ArrayProperty] varchar(max) NULL,
    [PeriodEnd] datetime2 GENERATED ALWAYS AS ROW END HIDDEN NOT NULL,
    [PeriodStart] datetime2 GENERATED ALWAYS AS ROW START HIDDEN NOT NULL,
    CONSTRAINT [PK_Data3] PRIMARY KEY ([Id]),
    CONSTRAINT [AK_Data3_Name] UNIQUE ([Name]),
    PERIOD FOR SYSTEM_TIME([PeriodStart], [PeriodEnd])
) WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [sam].[Data3History]));
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
    [Property] varchar(max) NULL,
    [ArrayProperty] varchar(max) NULL,
    [Text] varchar(max) NULL,
    [Date] date NOT NULL,
    [DateTime] datetime2 NOT NULL,
    [UtcTimeStamp] datetime2 NOT NULL,
    [Int] int NOT NULL,
    [Decimal] decimal(18,6) NOT NULL,
    [Bool] bit NOT NULL,
    [Double] float NOT NULL,
    [Float] real NOT NULL,
    [Guid] uniqueidentifier NOT NULL,
    [ModifiedBy] varchar(max) NOT NULL,
    [ModifiedUtc] datetime2 NOT NULL,
    [CreatedBy] varchar(max) NOT NULL,
    [CreatedUtc] datetime2 NOT NULL,
    CONSTRAINT [PK_MyData] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [sam].[ContractSpec] (
    [Id] int NOT NULL IDENTITY,
    [MarketId] int NOT NULL,
    [Value] decimal(20,10) NOT NULL,
    [StartDate] date NOT NULL,
    [EndDate] date NOT NULL,
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



