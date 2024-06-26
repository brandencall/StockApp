﻿/*
Deployment script for StockDB

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "StockDB"
:setvar DefaultFilePrefix "StockDB"
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'Altering Procedure [dbo].[spFinancialData_GetStocksWithFeatureList]...';


GO
ALTER PROCEDURE [dbo].[spFinancialData_GetStocksWithFeatureList]
	@year char(4)
AS
begin
	set nocount on;
	
WITH DistinctStocks AS (
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
            'MarketCapitalization','Assets', 'NetCashProvidedByUsedInFinancingActivities', 'NetCashProvidedByUsedInOperatingActivities', 'LiabilitiesAndStockholdersEquity',
            'NetIncomeLoss', 'StockholdersEquity', 'RetainedEarningsAccumulatedDeficit', 'NetCashProvidedByUsedInInvestingActivities',
            'CashAndCashEquivalentsAtCarryingValue', 'Liabilities'
        )
        AND (ff.Form = '10-K' OR ff.Form IS NULL)
        AND ff.EndDate LIKE '%' + @year + '%'
		AND ff.FiscalYear = @year
    GROUP BY
		s.Id,
        s.CIK,
        s.Ticker,
        s.Name
    HAVING
        COUNT(DISTINCT fa.Title) = 11 -- Number of fa.Title values in the IN list
)
SELECT
    ds.CIK,
    ds.Ticker,
	ff.FiscalYear,
	MAX(CASE WHEN fa.Title = 'MarketCapitalization' THEN cv.Value END) AS MarketCapitalization,
	MAX(CASE WHEN fa.Title = 'Assets' THEN cv.Value END) AS Assets,
    MAX(CASE WHEN fa.Title = 'NetCashProvidedByUsedInFinancingActivities' THEN cv.Value END) AS NetCashProvidedByFinancingActivities,
    MAX(CASE WHEN fa.Title = 'NetCashProvidedByUsedInOperatingActivities' THEN cv.Value END) AS NetCashProvidedByOperatingActivities,
    MAX(CASE WHEN fa.Title = 'LiabilitiesAndStockholdersEquity' THEN cv.Value END) AS LiabilitiesAndStockholdersEquity,
    MAX(CASE WHEN fa.Title = 'NetIncomeLoss' THEN cv.Value END) AS NetIncomeLoss,
    MAX(CASE WHEN fa.Title = 'StockholdersEquity' THEN cv.Value END) AS StockholdersEquity,
    MAX(CASE WHEN fa.Title = 'RetainedEarningsAccumulatedDeficit' THEN cv.Value END) AS RetainedEarningsAccumulatedDeficit,
    MAX(CASE WHEN fa.Title = 'NetCashProvidedByUsedInInvestingActivities' THEN cv.Value END) AS NetCashProvidedByInvestingActivities,
    MAX(CASE WHEN fa.Title = 'CashAndCashEquivalentsAtCarryingValue' THEN cv.Value END) AS CashAndCashEquivalentsAtCarryingValue,
    MAX(CASE WHEN fa.Title = 'Liabilities' THEN cv.Value END) AS Liabilities
FROM
    DistinctStocks ds
    INNER JOIN dbo.StockFinances sf ON sf.StockId = ds.Id
    INNER JOIN dbo.FinancialAttributes fa ON sf.FinancialAttributeId = fa.Id
    INNER JOIN dbo.FinancialFacts ff ON sf.FinancialFactId = ff.Id
    INNER JOIN dbo.Units u ON ff.UnitId = u.Id
    LEFT JOIN dbo.CurrencyValues cv ON sf.FinancialFactId = cv.FinancialFactId
    LEFT JOIN dbo.ShareValues sv ON sf.FinancialFactId = sv.FinancialFactId
    LEFT JOIN dbo.CurrencyPerShareValues cpsv ON sf.FinancialFactId = cpsv.FinancialFactId
WHERE
    fa.Title IN (
        'MarketCapitalization','Assets', 'NetCashProvidedByUsedInFinancingActivities', 'NetCashProvidedByUsedInOperatingActivities', 'LiabilitiesAndStockholdersEquity',
        'NetIncomeLoss', 'StockholdersEquity', 'RetainedEarningsAccumulatedDeficit', 'NetCashProvidedByUsedInInvestingActivities',
        'CashAndCashEquivalentsAtCarryingValue', 'Liabilities'
    )
    AND (ff.Form = '10-K' OR ff.Form IS NULL)
    AND ff.EndDate LIKE '%' + @year + '%'
	AND ff.FiscalYear = @year
GROUP BY
    ds.CIK,
    ds.Ticker,
	ff.FiscalYear
ORDER BY
    ds.Ticker;
end
GO
PRINT N'Update complete.';


GO
