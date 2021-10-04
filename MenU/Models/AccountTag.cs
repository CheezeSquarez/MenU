using System;
using System.Collections.Generic;

namespace MenU.Models
{
    public partial class AccountTag
    {
        public int AccountId { get; set; }
        public int TagId { get; set; }
        public int PickedNum { get; set; }
        public virtual Account Account { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
