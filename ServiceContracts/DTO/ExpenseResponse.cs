using Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceContracts.DTO
{
    public class ExpenseResponse
    {
        public Guid ExpenseID { get; set; }

        public int ExpenseValue { get; set; }
        public DateTime? ExpenseDate { get; set; }
        public string? ExpenseDescription { get; set; }
        public Guid CategoryID { get; set; }
        public string? Category { get; set; }

        public ExpenseUpdateRequest ToExpenseUpdateRequest()
        {
            return new ExpenseUpdateRequest()
            {
                ExpenseID = ExpenseID,
                ExpenseValue = ExpenseValue,
                ExpenseDate = ExpenseDate,
                ExpenseDescription = ExpenseDescription,
                CategoryID = CategoryID
            };
        }
    }

    public static class ExpenseExtensions
    {
        public static ExpenseResponse ToExpenseResponse(this Expense expense)
        {
            return new ExpenseResponse()
            {
                ExpenseID = expense.ExpenseID,
                ExpenseValue = expense.ExpenseValue,
                ExpenseDate = expense.ExpenseDate,
                ExpenseDescription = expense.ExpenseDescription,
                CategoryID = expense.CategoryID,
                Category = expense.Category?.CategoryName
            };
        }
    }
}