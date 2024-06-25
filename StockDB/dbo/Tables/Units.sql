CREATE TABLE [dbo].[Units]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Type] NVARCHAR(20) NOT NULL, 
    [FinancialTableId] INT NOT NULL, 
    CONSTRAINT [FK_Units_FinancialTables] FOREIGN KEY (FinancialTableId) REFERENCES FinancialTables(Id)
)
