CREATE TABLE [dbo].[FinancialAttributes]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Title] NVARCHAR(500) NOT NULL, 
    [Label] NVARCHAR(1000) NULL, 
    [Description] NVARCHAR(MAX) NULL
)
