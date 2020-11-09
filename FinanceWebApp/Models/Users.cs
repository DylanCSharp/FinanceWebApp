using System;
using System.Collections.Generic;

namespace FinanceWebApp.Models
{
    public partial class Users
    {
        public int UsersId { get; set; }
        public string UserFullname { get; set; }
        public string Email { get; set; }
        public string UserPassword { get; set; }
        public string UserPhone { get; set; }
    }
}
