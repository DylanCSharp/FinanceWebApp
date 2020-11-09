using System;
using System.Collections.Generic;

namespace FinanceWebApp.Models
{
    public partial class RentBuyProperty
    {
        public int IdRentBuy { get; set; }
        public int UsersId { get; set; }
        public decimal? RentMonthly { get; set; }
        public decimal? PropertyPurchase { get; set; }
        public decimal? PropertyDeposit { get; set; }
        public int? PropertyInterest { get; set; }
        public int? PropertyMonthsrepay { get; set; }
        public decimal? TotalHomeRepayment { get; set; }
        public decimal? MonthlyHomeRepayment { get; set; }

        public virtual Users Users { get; set; }
    }
}
