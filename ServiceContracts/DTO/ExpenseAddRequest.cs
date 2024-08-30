using Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceContracts.DTO
{
    public class ExpenseAddRequest
    {
        [Required(ErrorMessage ="Please eneter expense amount.")]
        [Range(1, 100000, ErrorMessage = "Expense value can be minimum 1 and maximum 1 lakh.")]
        public int ExpenseValue { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ExpenseDate { get; set; }
        public string? ExpenseDescription { get; set; }
        [Required(ErrorMessage ="Please select a category.")]
        public Guid CategoryID { get; set; }

        public Expense ToExpense()
        {
            return new Expense()
            {
                ExpenseValue = ExpenseValue,
                ExpenseDate = ExpenseDate,
                ExpenseDescription = ExpenseDescription,
                CategoryID = CategoryID
            };
        }
    }
}
