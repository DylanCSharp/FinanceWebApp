using System;
using System.Collections.Generic;

namespace FinanceWebApp.Models
{
    public partial class Cost
    {
        public int CostsId { get; set; }
        public int UsersId { get; set; }
        public string NormalExpenses { get; set; }
        public string FinalIncome { get; set; }
        public string PostDeductions { get; set; }
        public string SpendableIncome { get; set; }

        public virtual Users Users { get; set; }
    }
}
