export interface Stock {
    id: number,
    cik: string,
    ticker: string,
    name: string
}

export interface FinancialFact {
    date: string,
    currencyValue: number
}

export interface FinancialData {
    title: string,
    label: string,
    displayName: string,
    financialFacts: FinancialFact[]
}