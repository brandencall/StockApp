CREATE TABLE [dbo].[CurrencyValues]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Value] MONEY NOT NULL, 
    [FinancialFactId] INT NOT NULL, 
    CONSTRAINT [FK_CurrencyValues_FinancialFacts] FOREIGN KEY (FinancialFactId) REFERENCES FinancialFacts(Id)
)
