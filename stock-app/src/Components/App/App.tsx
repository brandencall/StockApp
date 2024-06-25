import { Route, Routes } from "react-router-dom"
import Home from '../Home/Home'
import StockFinancialPage from "../StockFinancialPage/StockFinancialPage"


function App() {

    return (
        <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/stock/:id" element={<StockFinancialPage />} />
        </Routes>
  )
}

export default App
