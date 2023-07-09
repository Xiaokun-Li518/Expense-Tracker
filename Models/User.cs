using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Expense_Tracker.Models
{
    public class User : IdentityUser
    {
        // New relationships
        public ICollection<Category> Categories { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
