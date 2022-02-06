using System;
using System.Collections.Generic;

namespace MenU.Models
{
    public partial class Account
    {
        public Account()
        {
            AccountAuthTokens = new List<AccountAuthToken>();
            AccountTags = new List<AccountTag>();
            Restaurants = new List<Restaurant>();
            Reviews = new List<Review>();
            AccountId = 0;
            Username = "";
            FirstName = "";
            LastName = "";
            Email = "";
            DateOfBirth = new DateTime();
            AccountType = 0;
            ProfilePicture = "";
            Pass = "";
            AccountStatus = 0;
            Salt = "";
            Iterations = 0;
            AccountStatusNavigation = new ObjectStatus();
            AccountTypeNavigation = new AccountType();
        }

        public int AccountId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int AccountType { get; set; }
        public string ProfilePicture { get; set; }
        public string Pass { get; set; }
        public int AccountStatus { get; set; }
        public string Salt { get; set; }
        public int Iterations { get; set; }
        public virtual ObjectStatus AccountStatusNavigation { get; set; }
        public virtual AccountType AccountTypeNavigation { get; set; }
        public virtual List<AccountAuthToken> AccountAuthTokens { get; set; }
        public virtual List<AccountTag> AccountTags { get; set; }
        public virtual List<Restaurant> Restaurants { get; set; }
        public virtual List<Review> Reviews { get; set; }
    }
}
