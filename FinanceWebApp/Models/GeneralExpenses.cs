using System;
using System.Collections.Generic;

namespace FinanceWebApp.Models
{
    public partial class GeneralExpenses
    {
        public int IdExpenses { get; set; }
        public int UsersId { get; set; }
        public decimal GrossIncome { get; set; }
        public decimal TaxDeducted { get; set; }
        public decimal Groceries { get; set; }
        public decimal WaterLights { get; set; }
        public decimal Travel { get; set; }
        public decimal Phone { get; set; }
        public decimal Other { get; set; }

        public virtual Users Users { get; set; }
    }
}
