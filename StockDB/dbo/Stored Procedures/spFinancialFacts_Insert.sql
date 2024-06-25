CREATE PROCEDURE [dbo].[spFinancialFacts_Insert]
	@unitId int,
	@startDate date,
	@endDate date,
	@fiscalYear nchar(4),
	@fiscalPeriod nchar(2),
	@form nvarchar(15),
	@filed date,
	@frame nvarchar(9)
AS
begin 
	set nocount on;

	insert into dbo.FinancialFacts (UnitId, StartDate, EndDate, FiscalYear, FiscalPeriod, Form, Filed, Frame)
	values (@unitId, @startDate, @endDate, @fiscalYear, @fiscalPeriod, @form, @filed, @frame)

	select SCOPE_IDENTITY() as Id;
end
