using System;
using System.Collections.Generic;

namespace FinanceWebApp.Models
{
    public partial class Users
    {
        public Users()
        {
            BuyCar = new HashSet<BuyCar>();
            Cost = new HashSet<Cost>();
            GeneralExpenses = new HashSet<GeneralExpenses>();
            RentBuyProperty = new HashSet<RentBuyProperty>();
            Savings = new HashSet<Savings>();
        }

        public int UsersId { get; set; }
        public string UserFullname { get; set; }
        public string Email { get; set; }
        public string UserPassword { get; set; }
        public string UserPhone { get; set; }

        public virtual ICollection<BuyCar> BuyCar { get; set; }
        public virtual ICollection<Cost> Cost { get; set; }
        public virtual ICollection<GeneralExpenses> GeneralExpenses { get; set; }
        public virtual ICollection<RentBuyProperty> RentBuyProperty { get; set; }
        public virtual ICollection<Savings> Savings { get; set; }
    }
}
