import { useEffect, useState } from 'react';
import { useLocation } from 'react-router-dom';
import FinancialTable from './FinancialTable/FinancialTable';
import styles from './StockFinancialPage.module.css'
import SearchBar from '../SearchBar/SearchBar';
import ToggleSwitch from '../ToggleSwitch/ToggleSwitch';
import LineChart from './LineChart/LineChart';
import { FinancialData } from '../../types'
import { fetchStockAnnualData, fetchStockQuarterlyData } from '../../api/StockService'

function StockFinancialPage() {

    const { state } = useLocation();
    const [annualData, setAnnualData] = useState<FinancialData[] | null>(null)
    const [quarterlyData, setQuarterlyData] = useState<FinancialData[] | null>(null)
    const [showAnnualData, setShowAnnualData] = useState<boolean>(true)
    const [showQuarterlyData, setShowQuarterlyData] = useState<boolean>(false)
    const [isAbbreviated, setIsAbbreviated] = useState<boolean>(true);

    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    function fetchData(state: any) {
        try {
            fetchStockAnnualData(setAnnualData, state.id, state.ticker)
            fetchStockQuarterlyData(setQuarterlyData, state.id, state.ticker)
        } catch (error) {
            console.error('Error fetching data:', error)
        }
    }

    useEffect(() => {
        setAnnualData(null)
        setQuarterlyData(null)
        setShowAnnualData(true)
        setShowQuarterlyData(false)
        setIsAbbreviated(true)

        fetchData(state)
    }, [state])

    const showDataAnnual = () => {
        setShowAnnualData(true)
        setShowQuarterlyData(false)
    }

    const showDataQuarterly = () => {
        setShowQuarterlyData(true)
        setShowAnnualData(false)
    }

    const handleSwitch = (data: boolean) => {
        setIsAbbreviated(data)
    }

    return (
        <>
            {
                annualData === null || quarterlyData === null
                ?
                <p>Loading...</p>                
                :
                annualData.length === 0 && quarterlyData.length === 0
                ?
                <p>No data :(</p>
                :
                (
                <>
                    <div className={styles.container}>
                        <div className={styles.top_container}>
                            <div className={styles.company_info_contianer}>
                                <div className={styles.name}>{state.name}</div>
                                <div className={styles.ticker}>{state.ticker}</div>
                                <div className={styles.cik}>CIK: {state.cik}</div>
                            </div>
                                <div className={styles.searchbar_contianer}>
                                <SearchBar />
                            </div> 
                        </div>
                        <LineChart data={annualData} />
                        <div className={styles.toggle_button_container}>
                            <div className={styles.toggle_container}>
                                <label className={styles.toggle_label}>Abbreviate</label>
                                <ToggleSwitch handleClick={handleSwitch} />
                                {isAbbreviated === false ?
                                    <div className={styles.abbreviated_comment}>(All numbers in thousands)</div>
                                    : null}
                            </div>
                            <div>
                                <button
                                    className={
                                        `${styles.table_button} ${showAnnualData === true ? styles.table_button_active : null}`
                                    }
                                    onClick={showDataAnnual}
                                >
                                    Annual
                                </button>
                                <button
                                    className={
                                        `${styles.table_button} ${showQuarterlyData === true ? styles.table_button_active : null}`
                                    }
                                    onClick={showDataQuarterly}>
                                    Quarterly
                                </button>
                            </div>
                        </div>
                        <div>
                            {showAnnualData && <FinancialTable data={annualData} isAbbreviated={isAbbreviated} />}
                            {showQuarterlyData && <FinancialTable data={quarterlyData} isAbbreviated={isAbbreviated} />}      
                        </div>
                    </div>
                </>  
            )}
        </>

  );
}

export default StockFinancialPage;