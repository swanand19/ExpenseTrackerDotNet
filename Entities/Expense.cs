using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Expense
    {
        [Key]
        public Guid ExpenseID { get; set; }
        [Range(1, 100000, ErrorMessage = "Expense value can be minimum 1 and maximum 1 lakh.")]
        public int ExpenseValue { get; set; }
        public DateTime? ExpenseDate { get; set; }
        [StringLength(150)]
        public string? ExpenseDescription { get; set; }

        public Guid CategoryID { get; set; }

        [ForeignKey("CategoryID")]
        public virtual Category? Category { get; set; }
    }
}
