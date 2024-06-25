CREATE PROCEDURE [dbo].[spStocks_GetAllStocks]

AS
begin
	set nocount on;
	
	select [Id], [CIK], [Ticker], [Name] from dbo.Stocks
end