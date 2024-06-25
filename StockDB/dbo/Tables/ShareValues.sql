CREATE TABLE [dbo].[ShareValues]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Value] BIGINT NOT NULL, 
    [FinancialFactId] INT NOT NULL, 
    CONSTRAINT [FK_ShareValues_FinancialFacts] FOREIGN KEY (FinancialFactId) REFERENCES FinancialFacts(Id)
)
