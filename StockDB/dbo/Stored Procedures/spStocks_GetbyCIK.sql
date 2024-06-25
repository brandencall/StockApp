CREATE PROCEDURE [dbo].[spStocks_GetbyCIK]
	@cik char(10)
AS
begin
	set nocount on;
	
	select top 1 [Id], [CIK], [Ticker], [Name]
	from dbo.Stocks 
	where CIK = @cik
end