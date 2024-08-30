using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using System;

namespace Services
{
    public class ExpensesService : IExpensesService
    {
        //private fields
        private readonly ApplicationDbContext _db;
        private readonly ICategoriesService _categoriesService;
        //constructor
        public ExpensesService(ApplicationDbContext db, ICategoriesService categoriesService)
        {
            _db = db;
            _categoriesService = categoriesService;
        }


        public async Task<ExpenseResponse> AddExpense(ExpenseAddRequest? expenseAddRequest)
        {
            //check if ExpenseAddRequest is not null
            if(expenseAddRequest == null) { throw new ArgumentNullException(nameof(expenseAddRequest)); }

            //validate ExpenseValue 
            if(expenseAddRequest.ExpenseValue == null) { throw new ArgumentException(nameof(expenseAddRequest.ExpenseValue)); }

            //Model validation
            ValidationHelper.ModelValidation(expenseAddRequest);

            //convert ExpenseAddRequest to Expense type 
            Expense expense = expenseAddRequest.ToExpense();

            //generate ExpenseID
            expense.ExpenseID = Guid.NewGuid();

            //add expense object to Expenses list
            _db.Expenses.Add(expense);
            await _db.SaveChangesAsync();

            return expense.ToExpenseResponse();
        }

        public async Task<bool> DeleteExpense(Guid? expenseID)
        {
            if(expenseID == null) { throw new ArgumentNullException(nameof(expenseID)); }
            Expense? expense = await _db.Expenses.FirstOrDefaultAsync(temp => temp.ExpenseID == expenseID);
            if (expense == null) { return false; }
            _db.Expenses.Remove(_db.Expenses.First(temp => temp.ExpenseID == expenseID));
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<ExpenseResponse>> GetAllExpenses()
        {
            var expenses = await _db.Expenses.Include("Category").ToListAsync();
            return expenses.Select(expense => expense.ToExpenseResponse()).ToList();
        }

        public async Task<ExpenseResponse?> GetExpenseByExpenseID(Guid? expenseID)
        {
            if (expenseID == null) { return null; }
            Expense? expense_response_from_list = await _db.Expenses.FirstOrDefaultAsync(temp => temp.ExpenseID == expenseID);
            if(expense_response_from_list == null) { return null; }
            return expense_response_from_list.ToExpenseResponse();
        }

        public async Task<List<ExpenseResponse>> GetFilteredExpenses(string searchBy, string? searchString)
        {
            List<ExpenseResponse> allExpenses = await GetAllExpenses();
            List<ExpenseResponse> matchingExpenses = allExpenses;
            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
                return matchingExpenses;

            switch (searchBy)
            {
                case nameof(ExpenseResponse.ExpenseDate):

                    matchingExpenses = allExpenses.Where(temp =>
                    (temp.ExpenseDate != null) ?
                    temp.ExpenseDate.Value.ToString("dd MM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(ExpenseResponse.Category):
                    matchingExpenses = allExpenses.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Category) ?
                    temp.Category.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                default: matchingExpenses = allExpenses; break;
            }
            return matchingExpenses;
        }

        public async Task<List<ExpenseResponse>> GetSortedExpenses(List<ExpenseResponse> allExpenses, string sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
                return allExpenses;

            List<ExpenseResponse> sortedExpenses = (sortBy, sortOrder)
                switch
            {
                (nameof(ExpenseResponse.ExpenseDate), SortOrderOptions.ASC) =>
                    allExpenses.OrderBy(temp => temp.ExpenseDate).ToList(),
                (nameof(ExpenseResponse.ExpenseDate), SortOrderOptions.DESC) =>
                    allExpenses.OrderByDescending(temp => temp.ExpenseDate).ToList(),

                _ => allExpenses
            };

            return sortedExpenses;
        }

        public async Task<ExpenseResponse> UpdateExpense(ExpenseUpdateRequest? expenseUpdateRequest)
        {
            if(expenseUpdateRequest == null) throw new ArgumentNullException(nameof(Expense));

            //validation
            ValidationHelper.ModelValidation(expenseUpdateRequest);

            //get matching expense object to update
            Expense? matchingExpense = await _db.Expenses
                            .FirstOrDefaultAsync(temp=>temp.ExpenseID == expenseUpdateRequest.ExpenseID);
            if(matchingExpense == null)
            {
                throw new ArgumentException("Given expense id doesen't exist");
            }

            //update all details
            matchingExpense.ExpenseValue = expenseUpdateRequest.ExpenseValue;
            matchingExpense.ExpenseDate = expenseUpdateRequest.ExpenseDate;
            matchingExpense.ExpenseDescription = expenseUpdateRequest.ExpenseDescription;
            matchingExpense.CategoryID = expenseUpdateRequest.CategoryID;

            await _db.SaveChangesAsync(); //update

            return matchingExpense.ToExpenseResponse();
        }
    }
}
