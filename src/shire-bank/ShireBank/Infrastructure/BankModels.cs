﻿using System.ComponentModel.DataAnnotations;

namespace ShireBankService.Infrastructure
{
    public class Account
    {
       [Key]
       public int Id { get; set; }
       public string FirstName { get; set; }
       public string LastName { get; set;}
       public DateTime CreatedAt { get; set;}
       public DateTime? ClosedAt { get; set; } 
       public float DebtLimit { get; set; }
       public float Ballance { get; set; }
       public virtual ICollection<AccountOperation> AccountOperations { get; set; }
    }

    public class AccountOperation
    { 
        public int Id { get; set; }
        public int AccountId { get; set; }
        public float Ammount { get; set; }
        public string ActionType { get; set; }
        public DateTime OperatedAt { get; set;}
        public virtual Account Account { get; set; }
    }
}
