import { Stock, FinancialData } from '../types'
import { setSessionStorageItem } from '../utils/sessionStorageUtils'

const baseApi: string = "https://localhost:7292/api/"

export async function fetchStockList(setStockList: (newState: Stock[]) => void) {

    if (localStorage.getItem('stockList') !== null) {
        const stockList = JSON.parse(localStorage.getItem('stockList') || '{}')
        setStockList(stockList)
    } else {

        fetch(`${baseApi}Stocks`)
            .then(response => {

                if (!response.ok) {
                    console.log(response.statusText);
                }
                return response.json();
            })
            .then(json => {
                localStorage.setItem('stockList', JSON.stringify(json))
                setStockList(json)
            })
            .catch(error => console.error(error))
    }
}


export async function fetchStockAnnualData(setAnnualData: (newState: FinancialData[]) => void, stockId: number, ticker: string) {

    const sessionStorageKey: string = ticker + '_annualData'

    if (sessionStorage.getItem(sessionStorageKey) !== null) {
        const annualData = JSON.parse(sessionStorage.getItem(sessionStorageKey) || '{}')
        setAnnualData(annualData)
    } else {

        fetch(`${baseApi}StockAnnual/${stockId}`)
            .then(response => {

                if (!response.ok) {
                    console.log(response.statusText);
                }
                return response.json();
            })
            .then(json => {
                setSessionStorageItem(sessionStorageKey, json)
                setAnnualData(json)
            })
            .catch(error => console.error(error))
    } 
}

export async function fetchStockQuarterlyData(setQuarterlyData: (newState: FinancialData[]) => void, stockId: number, ticker: string) {

    const sessionStorageKey: string = ticker + '_quarterlyData'

    if (sessionStorage.getItem(sessionStorageKey) !== null) {
        const quarterlyData = JSON.parse(sessionStorage.getItem(sessionStorageKey) || '{}')
        setQuarterlyData(quarterlyData)
    } else {
        fetch(`${baseApi}StockQuarterly/${stockId}`)
            .then(response => {

                if (!response.ok) {
                    console.log(response.statusText);
                }
                return response.json();
            })
            .then(json => {
                setSessionStorageItem(sessionStorageKey, json)
                setQuarterlyData(json)
            })
            .catch(error => console.error(error))
    }
   
}