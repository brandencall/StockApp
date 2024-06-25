CREATE PROCEDURE [dbo].[spFinancialFact_Delete]
	@financialFactId int
AS
begin
	set nocount on;

	delete from dbo.FinancialFacts where Id = @financialFactId
end
