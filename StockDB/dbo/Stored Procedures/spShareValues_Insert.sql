CREATE PROCEDURE [dbo].[spShareValues_Insert]
	@financialFactId int,
	@value bigint
AS
begin
	set nocount on;

	insert into dbo.ShareValues (FinancialFactId, Value)
	values (@financialFactId, @value)
end