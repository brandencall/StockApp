CREATE TABLE [dbo].[CurrencyPerShareValues]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Value] DECIMAL(18, 4) NOT NULL, 
    [FinancialFactId] INT NOT NULL, 
    CONSTRAINT [FK_CurrencyPerShareValues_FinancialFacts] FOREIGN KEY (FinancialFactId) REFERENCES FinancialFacts(Id)
)
