using System;
using System.Collections.Generic;

namespace MenU.Models
{

    public partial class AccountAuthToken
    {
        public int AccountId { get; set; }
        public string AuthToken { get; set; }
        public virtual Account Account { get; set; }
    }
}
