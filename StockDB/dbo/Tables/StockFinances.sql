CREATE TABLE [dbo].[StockFinances]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [StockId] INT NOT NULL, 
    [FinancialAttributeId] INT NOT NULL, 
    [FinancialFactId] INT NOT NULL, 
    CONSTRAINT [FK_StockFinances_Stocks] FOREIGN KEY ([StockId]) REFERENCES Stocks(Id), 
    CONSTRAINT [FK_StockFinances_FinacialAttributes] FOREIGN KEY (FinancialAttributeId) REFERENCES FinancialAttributes(Id), 
    CONSTRAINT [FK_StockFinances_FinancialFacts] FOREIGN KEY (FinancialFactId) REFERENCES FinancialFacts(Id)
)
