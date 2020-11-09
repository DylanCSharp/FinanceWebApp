using System;
using System.Collections.Generic;

namespace FinanceWebApp.Models
{
    public partial class BuyCar
    {
        public int IdCar { get; set; }
        public int UsersId { get; set; }
        public string CarMake { get; set; }
        public decimal? CarPurchase { get; set; }
        public decimal? CarDeposit { get; set; }
        public int? CarInterest { get; set; }
        public decimal? CarInsurance { get; set; }
        public decimal? TotalCarRepayment { get; set; }
        public decimal? MonthlyCarRepayment { get; set; }

        public virtual Users Users { get; set; }
    }
}
