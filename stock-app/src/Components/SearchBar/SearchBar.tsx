import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom'
import styles from './SearchBar.module.css'
import { fetchStockList } from '../../api/StockService'
import { Stock } from '../../types'


function SearchBar() {

    const numberOfRows: number = 10;

    const [value, setValue] = useState<string>("");
    const [stockList, setStockList] = useState<Stock[]>([])
    const navigate = useNavigate();

    function fetchData() {
        try {
            fetchStockList(setStockList)
        } catch (error) {
            console.error('Error fetching data:', error)
        }
    }

    useEffect(() => {
        fetchData();
    }, [])

    const onChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setValue(e.target.value)
    }

    const onSearch = (searchTerm: Stock) => {
        if (searchTerm !== null) {
            const id: number = searchTerm.id
            const ticker: string = searchTerm.ticker
            const name: string = searchTerm.name
            const cik: string = searchTerm.cik
            navigate(`/stock/${ticker}`, {
                state: { id: `${id}`, ticker: `${ticker}`, name: `${name}`, cik: `${cik}` }
            });
        }        
    }

    const searchStockList = (stock: Stock) => {
        const searchTerm: string = value.toLowerCase();
        const name: string = stock.name.toLowerCase();
        const ticker: string = stock.ticker.toLowerCase();
        const cik: string = stock.cik.toLowerCase();


        return searchTerm
            && (name.includes(searchTerm)
            || ticker.includes(searchTerm)
            || cik.includes(searchTerm))
            && name !== searchTerm;
    }

    const highlightMatch = (text: string, searchTerm: string, isCIK: boolean) => {
        const parts = text.split(new RegExp(`(${searchTerm})`, 'gi'));
        return (
            <>
                {parts.map((part, index) =>
                    part.toLowerCase() === searchTerm.toLowerCase() ? (
                        <span key={index} className={
                            `${styles.span_matched_search} ${isCIK === true ? styles.span_matched_search_cik : ''}`
                            }
                            
                        >
                            {part}
                        </span>
                    ) : (
                            <span key={index} className={`${isCIK === true ? styles.span_matched_search_cik : ''}`}>{part}</span>
                    )
                )}
            </>
        );
    }

    const filteredStockList: Stock[] = stockList.filter(item => searchStockList(item)).slice(0, numberOfRows)
    const listLength: number = filteredStockList.length

    return (
        <>
            <div >
                <div className={styles.search_input_container}>
                    <input type="text"
                        value={value}
                        onChange={onChange}
                        placeholder="Search for a Stock"
                    />
                    <div className={styles.dropdown}>
                        {
                            filteredStockList.map((item, index) => (
                                <div
                                    className={
                                        `${styles.dropdown_row} 
                                         ${index === listLength - 1 ? styles.dropdown_row_last : ''}`
                                    }
                                    key={item.id}
                                    onClick={() => onSearch(item)}
                                >
                                    <div className={styles.dropdown_row_name_ticker}>
                                        {highlightMatch(item.name, value, false)} {' ('}{highlightMatch(item.ticker, value, false)}{')'} 
                                    </div>
                                    <div className={styles.dropdown_row_cik} >
                                        {'CIK: '}{highlightMatch(item.cik, value, true)}
                                    </div>
                                </div>
                            ))
                        }
                    </div>
                </div>
            </div>
        </>
        
    );
}

export default SearchBar;