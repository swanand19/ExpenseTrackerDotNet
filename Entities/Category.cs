using System;
using System.ComponentModel.DataAnnotations;


namespace Entities
{
    public class Category
    {
        [Key]
        public Guid CategoryID { get; set; }

        public string? CategoryName { get; set; }
        public virtual ICollection<Expense>? Expenses { get; set; }
    }
}
