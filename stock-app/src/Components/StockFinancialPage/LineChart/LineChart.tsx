import { useEffect, useRef, useState } from "react";
import styles from './LineChart.module.css'
import { Line } from "react-chartjs-2"
import { FinancialData, FinancialFact } from '../../../types'
import {
    Chart as ChartJS,
    LineElement,
    PointElement,
    LineController,
    CategoryScale,
    LinearScale,
    Title,
    Tooltip,
    Legend,
} from 'chart.js';

ChartJS.register(
    LineElement,
    PointElement,
    LineController,
    CategoryScale,
    LinearScale,
    Title,
    Tooltip,
    Legend
);

interface LineChartProps {
    data: FinancialData[]
}


const financialDataTitles: { [key: string]: string } = {
    "AssetsCurrent": "#26ed44",
    "LiabilitiesCurrent": "#f24430",
    "StockholdersEquity": "#2aaef5"

}

const options = {
    responsive: true,
    maintainAspectRatio: false,
};

function LineChart({ data }: LineChartProps) {

    const chartContainer = useRef<HTMLDivElement>(null);

    function filterData(financialData: FinancialData) {

        const color = financialDataTitles[financialData.title as keyof typeof financialDataTitles];

        if (financialDataTitles[financialData.title] ) {
            return {
                label: financialData.displayName,
                data: financialData.financialFacts.slice(0).reverse().map((fact: FinancialFact) => fact.currencyValue),
                backgroundColor: color,
                borderColor: color
            }
        }

        return null
    }

    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const [chartData, setChartData] = useState <any>({
        labels: data[0].financialFacts.slice(0).reverse().map((fact: FinancialFact) => fact.date),
        datasets: data.map(filterData).filter(dataset => dataset !== null)
    })

    useEffect(() => {
        const handleResize = () => {
            if (chartContainer.current) {
                // Force a re-render on window resize to make sure the chart adjusts correctly
                setChartData({ ...chartData });
            }
        };

        window.addEventListener('resize', handleResize);
        return () => {
            window.removeEventListener('resize', handleResize);
        };
    }, [chartData]);

    return (
        <div ref={chartContainer} className={styles.chart_container}>
            <Line data={chartData} options={options} />
      </div>
  );
}

export default LineChart;