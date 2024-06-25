CREATE PROCEDURE [dbo].[spCurrencyValues_Insert]
	@financialFactId int,
	@value money
AS
begin
	set nocount on;

	insert into dbo.CurrencyValues (FinancialFactId, Value)
	values (@financialFactId, @value)
end
