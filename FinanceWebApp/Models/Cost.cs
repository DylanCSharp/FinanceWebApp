using System;
using System.Collections.Generic;

namespace FinanceWebApp.Models
{
    public partial class Cost
    {
        public int CostsId { get; set; }
        public int UsersId { get; set; }
        public decimal NormalExpenses { get; set; }
        public decimal FinalIncome { get; set; }
        public decimal PostDeductions { get; set; }
        public decimal SpendableIncome { get; set; }

        public virtual Users Users { get; set; }
    }
}
