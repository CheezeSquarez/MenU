using System;
using System.Collections.Generic;

namespace MenU.Models
{

    public partial class AccountType
    {
        public AccountType()
        {
            Accounts = new List<Account>();
        }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public virtual List<Account> Accounts { get; set; }
    }
}
