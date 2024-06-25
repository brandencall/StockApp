import { FinancialData, FinancialFact } from '../../../types'
import { abbreviateNumber, formatNumberWithCommas } from '../../../utils/numberUtils';
import styles from './FinancialTable.module.css'

interface FinancialTableProps {
    data: FinancialData[],
    isAbbreviated: boolean
}

function FinancialTable({ data, isAbbreviated }: FinancialTableProps) {

    const dates = [...new Set(data.flatMap((item: FinancialData) => item.financialFacts.map((fact: FinancialFact) => fact.date)))];

    const formatNumber = (currencyValue: number) => {

        if (isAbbreviated === true) {
            return abbreviateNumber(currencyValue)
        }
        else {
            return formatNumberWithCommas(currencyValue)
        }
    }  

    return (
        <table className={styles.table} >
            <thead>
                <tr>
                    <th>Financial Attribute</th>
                    {dates.map(date => (
                        <th key={date}>{date}</th>
                    ))}
                </tr>
            </thead>
            <tbody>
                {data.map(item => (
                    <tr key={item.displayName}>
                        <td className={styles.attribute_name}>{item.displayName}</td>
                        {dates.map(date => {
                            const fact = item.financialFacts.find(f => f.date === date);
                            return <td key={date}>{fact ? formatNumber(fact.currencyValue) : 'N/A'}</td>;
                        })}
                    </tr>
                ))}
            </tbody>
        </table>
    );
}

export default FinancialTable;