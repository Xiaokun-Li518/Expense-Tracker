using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
namespace Expense_Tracker.Models
{
    public class User : IdentityUser
    {
        // New relationships
        [JsonIgnore]
        public ICollection<Category> Categories { get; set; }

        [JsonIgnore]
        public ICollection<Transaction> Transactions { get; set; }
    }
}
