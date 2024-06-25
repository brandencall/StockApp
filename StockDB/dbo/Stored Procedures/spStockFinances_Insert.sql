CREATE PROCEDURE [dbo].[spStockFinances_Insert]
	@stockId int,
	@financialAttributeId int,
	@financialFactsId int
AS
begin
	set nocount on;

	insert into dbo.StockFinances (StockId, FinancialAttributeId, FinancialFactId)
	values (@stockId, @financialAttributeId, @financialFactsId)
end
