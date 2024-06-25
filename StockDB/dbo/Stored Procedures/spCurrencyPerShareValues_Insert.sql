CREATE PROCEDURE [dbo].[spCurrencyPerShareValues_Insert]
	@financialFactId int,
	@value decimal(18,4)
AS
begin
	set nocount on;

	insert into dbo.CurrencyPerShareValues (FinancialFactId, Value)
	values (@financialFactId, @value)
end
