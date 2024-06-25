CREATE PROCEDURE [dbo].[spFinancialDataApi_GetAnnual]
	@stockId int
AS
begin
	set nocount on;

	WITH RankedFinances AS (
    SELECT 
        s.Ticker,
        s.Name,
        fa.Title,
        fa.Label,
        ff.FiscalYear,
        fac.DisplayName,
        cv.Value AS CurrencyValue,
        ROW_NUMBER() OVER (
            PARTITION BY fa.Title, ff.FiscalYear 
            ORDER BY ff.EndDate DESC
        ) AS RowNum
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
		AND (ff.Form LIKE '%10-K%' OR ff.Form LIKE '%20-F%')
        AND u.Type = 'USD'
)
SELECT 
    Ticker,
    Name,
    Title,
    Label,
    DisplayName,
    FiscalYear,
    CurrencyValue
FROM 
    RankedFinances
WHERE 
    RowNum = 1
ORDER BY 
    Title, FiscalYear DESC;

end
