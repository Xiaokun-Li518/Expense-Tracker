using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
namespace Expense_Tracker.Models
{
    public class Transaction
    {
        [Key]
        public int? TransactionId { get; set;}



        // New user relation here
        public string? UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Amount should be greater than 0.")]
        public int? Amount { get; set; } = 0;

        [Column(TypeName= "varchar(75)")]
        public string? Note { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public string? CategoryTitleWithIcon
        {
            get
            {
                return Category == null? "":Category.TitleWithIcon;
            }
        }

        [NotMapped]
        public string? FormattedAmount 
        {
            get
            {
                return ((Category == null || Category.Type == "Expense")? "- ":"+ ") + "$"+Amount.ToString();
            }
        }
    }
}

