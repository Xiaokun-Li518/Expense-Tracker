using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense_Tracker.Models 
{
    public class Category 
    {
        [Key]
        public int CategoryId { get; set; }

        // New user relation here
        public string? UserId { get; set; } // String to match IdentityUser Id
        public User? User { get; set; }

        [Column(TypeName= "varchar(50)")]
        [Required(ErrorMessage = "Title is Required")]
        public string? Title { get; set; }

        [Column(TypeName= "varchar(5)")]
        public string Icon { get; set; } = "";

        [Column(TypeName= "varchar(10)")]
        public string Type { get; set; } = "Expense";

        [NotMapped]
        public string TitleWithIcon 
        {
            get
            {
                return this.Icon + " " + this.Title;
            }
        }
    }
}
