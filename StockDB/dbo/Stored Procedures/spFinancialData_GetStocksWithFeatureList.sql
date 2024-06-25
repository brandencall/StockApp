﻿CREATE PROCEDURE [dbo].[spFinancialData_GetStocksWithFeatureList]
	@year char(4),
    @labelYear char(4)
AS
begin
	set nocount on;

	WITH MarketCap2024 AS (
    SELECT
        s.CIK,
        s.Ticker,
        ff.FiscalYear,
        ff.EndDate,
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
    WHERE
        ff.FiscalYear = @labelYear
        AND fa.Title = 'MarketCapitalization'
    ), 
    DistinctStocks AS (
        SELECT
            s.Id,
            s.CIK,
            s.Ticker,
            s.Name
        FROM
            dbo.StockFinances sf
            INNER JOIN dbo.Stocks s ON sf.StockId = s.Id
            INNER JOIN dbo.FinancialAttributes fa ON sf.FinancialAttributeId = fa.Id
            INNER JOIN dbo.FinancialFacts ff ON sf.FinancialFactId = ff.Id
        WHERE
            fa.Title IN (
                'RevenueFromContractWithCustomerExcludingAssessedTax', 'NetIncomeLoss', 'OperatingIncomeLoss', 'AccountsReceivableNetCurrent', 'AssetsCurrent', 'LiabilitiesCurrent',
			    'StockholdersEquity', 'CashAndCashEquivalentsAtCarryingValue'
            )
            AND (ff.Form = '10-K' OR ff.Form = '20-F' OR ff.Form = '10-K/A' OR ff.Form IS NULL)
            AND ff.EndDate LIKE '%' + @year + '%'
            AND ff.FiscalYear = @year
        GROUP BY
            s.Id,
            s.CIK,
            s.Ticker,
            s.Name
        HAVING
            COUNT(DISTINCT fa.Title) = 8 -- Number of fa.Title values in the IN list minus MarketCapitalization
    )

    SELECT
        ds.CIK,
        ds.Ticker,
        ff.FiscalYear,
        MAX(CASE WHEN fa.Title = 'MarketCapitalization' THEN m.CurrencyValue END) AS NextYearMarketCap,
         MAX(CASE WHEN fa.Title = 'RevenueFromContractWithCustomerExcludingAssessedTax' THEN cv.Value END) AS NetSales,
        MAX(CASE WHEN fa.Title = 'NetIncomeLoss' THEN cv.Value END) AS NetIncomeLoss,
	    MAX(CASE WHEN fa.Title = 'OperatingIncomeLoss' THEN cv.Value END) AS OperatingIncomeLoss,
	    MAX(CASE WHEN fa.Title = 'AccountsReceivableNetCurrent' THEN cv.Value END) AS AccountsReceivableNetCurrent,
	    MAX(CASE WHEN fa.Title = 'AssetsCurrent' THEN cv.Value END) AS AssetsCurrent,
	    MAX(CASE WHEN fa.Title = 'LiabilitiesCurrent' THEN cv.Value END) AS LiabilitiesCurrent,
	    MAX(CASE WHEN fa.Title = 'StockholdersEquity' THEN cv.Value END) AS StockholdersEquity,
	    MAX(CASE WHEN fa.Title = 'CashAndCashEquivalentsAtCarryingValue' THEN cv.Value END) AS CashAndCashEquivalentsAtCarryingValue
    FROM
        DistinctStocks ds
        INNER JOIN dbo.StockFinances sf ON sf.StockId = ds.Id
        INNER JOIN dbo.FinancialAttributes fa ON sf.FinancialAttributeId = fa.Id
        INNER JOIN dbo.FinancialFacts ff ON sf.FinancialFactId = ff.Id
        INNER JOIN dbo.Units u ON ff.UnitId = u.Id
        LEFT JOIN dbo.CurrencyValues cv ON sf.FinancialFactId = cv.FinancialFactId
        LEFT JOIN MarketCap2024 m ON ds.Ticker = m.Ticker
    WHERE
        fa.Title IN (
            'MarketCapitalization','RevenueFromContractWithCustomerExcludingAssessedTax', 'NetIncomeLoss', 'OperatingIncomeLoss', 'AccountsReceivableNetCurrent', 'AssetsCurrent', 'LiabilitiesCurrent',
			'StockholdersEquity', 'CashAndCashEquivalentsAtCarryingValue'
        )
        AND (ff.Form = '10-K' OR ff.Form = '20-F' OR ff.Form = '10-K/A' OR ff.Form IS NULL)
        AND ff.EndDate LIKE '%' + @year + '%'
        AND ff.FiscalYear = @year
    GROUP BY
        ds.CIK,
        ds.Ticker,
        ff.FiscalYear
    Having
	    MAX(CASE WHEN fa.Title = 'MarketCapitalization' THEN m.CurrencyValue END) IS NOT NULL
    ORDER BY
        ds.Ticker;
end
