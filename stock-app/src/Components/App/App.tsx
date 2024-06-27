import { Route, Routes } from "react-router-dom"
import Home from '../Home/Home'
import StockFinancialPage from "../StockFinancialPage/StockFinancialPage"
import Header from "../Header/Header"
import About from "../About/About"


function App() {

    return (
        <>
        <Header />
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/About" element={<About />} />
                <Route path="/stock/:id" element={<StockFinancialPage />} />
            </Routes>
        </>
  )
}

export default App
