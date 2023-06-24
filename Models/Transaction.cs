using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense_Tracker.Models
{
    public class Transaction
    {
        [Key]
        public int? TransactionId { get; set;}

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public int? Amount { get; set; }

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
                return ((Category == null || Category.Type == "Expense")? "- ":"+ ") + Amount.ToString();
            }
        }
    }
}

