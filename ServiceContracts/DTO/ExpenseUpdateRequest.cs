using Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    public class ExpenseUpdateRequest
    {
        public Guid ExpenseID { get; set; }
        [Required(ErrorMessage ="Expense value cannot be blank")]
        [Range(1, 100000, ErrorMessage = "Expense value can be minimum 1 and maximum 1 lakh.")]
        public int ExpenseValue { get; set; }
        [Required(ErrorMessage ="Please select the date of expenditure.")]
        public DateTime? ExpenseDate { get; set; }
        public string? ExpenseDescription { get; set; }
        public Guid CategoryID { get; set; }

        /// <summary>
        /// Converts the current object of ExpenseAddRequest into a new object of Expense type.
        /// </summary>
        /// <returns></returns>
        public Expense ToExpense()
        {
            return new Expense()
            {
                ExpenseID = ExpenseID,
                ExpenseValue = ExpenseValue,
                ExpenseDate = ExpenseDate,
                ExpenseDescription = ExpenseDescription,
                CategoryID = CategoryID
            };
        }
    }
}