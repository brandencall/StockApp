CREATE PROCEDURE [dbo].[spStocks_Insert]
	@cik char(10),
	@ticker nvarchar(20),
	@name nvarchar(500)
AS
begin
	set nocount on;
	if not exists (select 1 from dbo.Stocks where CIK = @cik)
	begin
		insert into dbo.Stocks (CIK, Ticker, Name)
		values (@cik, @ticker, @name);
	end
end
