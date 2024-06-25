CREATE PROCEDURE [dbo].[spFinancialDataApi_GetQuarterly]
	@stockId int
AS
begin
	set nocount on;

	SELECT 
		fa.Title,
		fa.Label,
		fac.DisplayName,
		ff.Frame,
		cv.Value AS CurrencyValue
	FROM
		dbo.StockFinances sf
		INNER JOIN dbo.Stocks s ON sf.StockId = s.Id
		INNER JOIN dbo.FinancialAttributes fa ON sf.FinancialAttributeId = fa.Id
		INNER JOIN dbo.FinancialFacts ff ON sf.FinancialFactId = ff.Id
		INNER JOIN dbo.Units u ON ff.UnitId = u.Id
		LEFT JOIN dbo.CurrencyValues cv ON sf.FinancialFactId = cv.FinancialFactId
		LEFT JOIN dbo.ShareValues sv ON sf.FinancialFactId = sv.FinancialFactId
		LEFT JOIN dbo.CurrencyPerShareValues cpsv ON sf.FinancialFactId = cpsv.FinancialFactId
		INNER JOIN dbo.FinancialAttributeConfig fac on fac.FinancialAttributeId = fa.Id
	WHERE 
		s.Id = @stockId
		AND u.Type = 'USD'
		AND Frame IS NOT NULL
	ORDER BY 
		fa.Title, FiscalYear DESC, Frame DESC;

end
