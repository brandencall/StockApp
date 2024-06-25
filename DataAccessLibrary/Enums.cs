using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary
{

    //Something doesn't smell right. Need to fix this. Will probably need to move UnitTypeMapper somewhere else.
    public static class Enums
    {
        public enum UnitType
        {
            USD,
            USDPerShares,
            Shares,
            TWD,
            TWDPerShares,
            Pure,
            EUR,
            EURPerShares,
            CNY,
            CNYPerShares
        }

        public enum FinancialValueType
        {
            CurrencyPerShareValue,
            CurrencyValue,
            ShareValue
        }

        public enum FactsType
        {
            us_gaap,
            ifrs_full,
            dei,
            srt,
            invest
        }
    }
}
