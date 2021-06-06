CREATE TABLE [dbo].[TableSector] (
    [Id]   INT           NOT NULL,
    [name] NVARCHAR (50) NOT NULL,
    [note] NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[TableDepartment] (
    [Id]   INT           NOT NULL,
    [name] NVARCHAR (50) NOT NULL,
    [note] NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[TableCurrency] (
    [code] NVARCHAR (10) NOT NULL,
    [name] NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([code] ASC)
);

CREATE TABLE [dbo].[TableExchange] (
    [code] NVARCHAR (10) NOT NULL,
    [name] NVARCHAR (50) NOT NULL,
    [note] NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([code] ASC)
);

CREATE TABLE [dbo].[TableStock] (
    [ticker]       NVARCHAR (10)  NOT NULL,
    [sectorId]     INT           NULL,
    [departmentId] INT           NULL,
    [name]         NVARCHAR (50) NOT NULL,
    [lot]          INT           DEFAULT ((1)) NOT NULL,
    [currencyCode] NVARCHAR (10) NOT NULL,
    [exchangeCode] NVARCHAR (10) NOT NULL,
    [note]         NVARCHAR (50) NULL,
    [pe]           DECIMAL (18)  NULL,
    [ps]           DECIMAL (18)  NULL,
    [debtEq]       DECIMAL (18)  NULL,
    [price]        DECIMAL (18)  NULL,
    [priceW52High] DECIMAL (18)  NULL,
    [priceW52Low]  DECIMAL (18)  NULL,
    PRIMARY KEY CLUSTERED ([ticker] ASC),
    FOREIGN KEY (sectorId) REFERENCES TableSector(Id),
    FOREIGN KEY (departmentId) REFERENCES TableDepartment(Id),
    FOREIGN KEY (currencyCode) REFERENCES TableCurrency(code),
    FOREIGN KEY (exchangeCode) REFERENCES TableExchange(code)
);

CREATE TABLE [dbo].[TableDividends] (
    [ticker]              NVARCHAR (10) NOT NULL,
    [dateCutoff]          DATE          NOT NULL,
    [dateRegisterClosing] DATE          NULL,
    [datePay]             DATE          NULL,
    [amount]              FLOAT (53)    NULL,
    [profitability]       FLOAT (53)    NULL,
    PRIMARY KEY CLUSTERED ([ticker] ASC, [dateCutoff] ASC),
    FOREIGN KEY (ticker) REFERENCES TableStock(ticker)
);

CREATE TABLE [dbo].[TableUser] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [login]    NVARCHAR (30) NOT NULL,
    [password] NVARCHAR (30) NULL,
    [nikname]  NVARCHAR (15) NOT NULL,
    [email]    NVARCHAR (30) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[TablePortfolio] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [userId]    INT           NOT NULL,
    [ticker]    NVARCHAR (10) NOT NULL,
    [count]     INT           NOT NULL,
    [dateBue]   DATE          NOT NULL,
    [priceBue]  MONEY         NOT NULL,
    [dateSell]  DATE          NULL,
    [priceSell] MONEY         NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY (userId) REFERENCES TableUser(Id),
    FOREIGN KEY (ticker) REFERENCES TableStock(ticker)
);
