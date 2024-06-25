using DataAccessLibrary.Databases;
using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAccessLibrary.Enums;

namespace DataAccessLibrary
{
    public static class Mapping
    {
        public static Dictionary<UnitType, FinancialValueType> UnitsToFinancialValues = new Dictionary<UnitType, FinancialValueType>()
        {
            { UnitType.USD, FinancialValueType.CurrencyValue },
            { UnitType.USDPerShares, FinancialValueType.CurrencyPerShareValue },
            { UnitType.Shares, FinancialValueType.ShareValue },
            { UnitType.TWD, FinancialValueType.CurrencyValue },
            { UnitType.TWDPerShares, FinancialValueType.CurrencyPerShareValue },
            { UnitType.Pure, FinancialValueType.CurrencyPerShareValue },
            { UnitType.EUR, FinancialValueType.CurrencyValue },
            { UnitType.EURPerShares, FinancialValueType.CurrencyPerShareValue },
            { UnitType.CNY, FinancialValueType.CurrencyValue },
        };

        public static Dictionary<UnitType, string> UnitTypesToString = new Dictionary<UnitType, string>()
        {
            { UnitType.USD, "USD" },
            { UnitType.USDPerShares, "USD/shares" },
            { UnitType.Shares, "shares" },
            { UnitType.TWD, "TWD" },
            { UnitType.TWDPerShares, "TWD/shares" },
            { UnitType.Pure, "pure" },
            { UnitType.EUR, "EUR" },
            { UnitType.EURPerShares, "EUR/shares" },
            { UnitType.CNY, "CNY" },
        };

        public static Dictionary<FactsType, string> FactTypesToString = new Dictionary<FactsType, string>()
        {
            { FactsType.us_gaap, "us-gaap" },
            { FactsType.ifrs_full, "ifrs-full" },
            { FactsType.dei, "dei" },
            { FactsType.srt, "srt" },
            { FactsType.invest, "invest" }
        };
    }
}
