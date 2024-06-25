CREATE TABLE [dbo].[FinancialFacts]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UnitId] INT NOT NULL, 
    [StartDate] DATE NULL, 
    [EndDate] DATE NULL, 
    [FiscalYear] NCHAR(4) NULL, 
    [FiscalPeriod] NCHAR(2) NULL, 
    [Form] NVARCHAR(15) NULL, 
    [Filed] DATE NULL, 
    [Frame] NVARCHAR(9) NULL, 
    CONSTRAINT [FK_FinancialFacts_Units] FOREIGN KEY ([UnitId]) REFERENCES Units(Id)
)
