CREATE TABLE [dbo].[FinancialAttributeConfig]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FinancialAttributeId] INT NOT NULL, 
    [Title] NVARCHAR(500) NOT NULL, 
    [DisplayName] NVARCHAR(500) NULL, 
    CONSTRAINT [FK_FinancialAttributeConfig_FinancialAttributes] FOREIGN KEY ([FinancialAttributeId]) REFERENCES FinancialAttributes(Id)
)
