## **Description**:
This is a full stack application created in React TS, C# and MicroSoft SQL Server. The purpose of this application is to view a companies financial data overtime. 
![image](https://github.com/brandencall/StockApp/assets/54908229/43546adc-7514-468c-8b04-e77565e7b265)
![image](https://github.com/brandencall/StockApp/assets/54908229/8944a34c-1e6a-41c0-b4bf-1cae3067de49)

## **Database Design**:
The database was created in Microsoft SQL server. Below is the design of the database: 

![image](https://github.com/brandencall/StockApp/assets/54908229/3b3cf600-180f-4ef7-844d-aeb5f0a6e8d1)

#### **Stocks Table**:
This table stores all of the stocks that the SEC EDGAR API has.. It only has 4 columns: Id, CIK, Ticker, Name.

#### **Financial Attributes Table**:
This table contains all of the financial attributes. Some companies have the same financial attributes but others have unquie ones. Having a seperate table for them makes it so that
the table is able to scale overtime.

#### **Financial Attributes Config Table**:
This table only has a short list of the financial attribtes that I want exposed to the frontend. Have a table for this makes it so there is only a single change that needs to happen
for more financial attributes to be displayed.

#### **Units Table**:
This table contains all of the different financial units. For example, USD is a currency value and USD/shares is a currency per shares value. This table helps us determine what kind
of financial fact we have.

#### **Financial Facts Table**:
This table contains all of the different financial facts that we have. For instance, it will have when this fact was filed and it will tell us what Unit to use.

#### **Values Tables**:
These are the 3 values tables: Currency Values, Shares Values, and Currency Per Shares Values. These tables contain a value for a particular financial fact. We do not have a single 
Values table because they inherently represent different things.

#### **Stock Finances Tables**:
Finally, this table conbines the Stock table, Financial Attributes table and the Financial Facts table.

## **DataAccessLibrary**:
This library is used to insert and read data from the database. It uses Dapper instead of EntityFrame work so that custom stored proceedures are able to run agains the database.
The SqlDataAccess class has a LoadData method and a SaveData method that uses Dapper to run the sql commands. The SQLData class is what is exposed to the API and Data Insert Script.
This class contains methods for both the API and the insert script.

## **DataInsertScript**:
This script is used to populate the database with financial data. The database is populated mostly with data from the SEC EDGAR API. This is Apple's data: https://data.sec.gov/api/xbrl/companyfacts/CIK0000320193.json.
This script also called an FMP API for market cap data: https://financialmodelingprep.com/api/v3/historical-market-capitalization/.

## **StockApi**:
This API servers stock data to the frontend React app. There are three different contollers: StocksController, StockAnnualController, StockQuarterlyController. The StocksController
is used to get a list of the stocks in the database. StockAnnualController gets annual data for a particular stock. StockQuarterlyController gets quarterly data for a particular stock.

## **stock-app**:
This is the front end application that is created in React Typescript. It calls the StockController on launch to get the list of stocks. This is then saved to local storage so that it
does not have to call the database for this information again. When searching for a stock it will fetch both the annual and quartly data asynchronously and display it with a char
and a table.
