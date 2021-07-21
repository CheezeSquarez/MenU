using System;
using System.Collections.Generic;
using System.Text;

namespace MenU.Models
{
    public class Account
    {
        public Account() { }
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
    }
}
