using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System;


namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Expense entity
    /// </summary>
    public interface IExpensesService
    {
        /// <summary>
        /// Add new expense into the list of expenses
        /// </summary>
        /// <param name="expenseAddRequest">Expense to add</param>
        /// <returns>Returns the same expense details, alongwith newly generated ExpenseID</returns>
        Task<ExpenseResponse> AddExpense(ExpenseAddRequest? expenseAddRequest);

        /// <summary>
        /// Returns all expenses
        /// </summary>
        /// <returns>Returns a list of objects of ExpenseResponse type.</returns>
        Task<List<ExpenseResponse>> GetAllExpenses();

        /// <summary>
        /// Returns Expense object based on the given expenseID
        /// </summary>
        /// <param name="expenseID">ExpenseID to search</param>
        /// <returns>Returns matching expense object.</returns>
        Task<ExpenseResponse?> GetExpenseByExpenseID(Guid? expenseID);

        /// <summary>
        /// Returns all fields that matches with the given search fields or search string
        /// </summary>
        /// <param name="searchBy">Search field to search</param>
        /// <param name="searchString">Search string to search</param>
        /// <returns>Returns all matching expenses based on the given search fields and search string.</returns>
        Task<List<ExpenseResponse>> GetFilteredExpenses(string searchBy, string? searchString);

        /// <summary>
        /// Updates the specified expense details based on the expense id
        /// </summary>
        /// <param name="expenseUpdateRequest">Expense details to update, including expense id.</param>
        /// <returns>Returns the expense object after updation.</returns>
        Task<ExpenseResponse> UpdateExpense(ExpenseUpdateRequest? expenseUpdateRequest);

        /// <summary>
        /// Deletes an expense based on the given expense id.
        /// </summary>
        /// <param name="expenseID">ExpenseID to delete</param>
        /// <returns>Returns true, if the deletion is successful, otherwise false.</returns>
        Task<bool> DeleteExpense(Guid? expenseID);

        /// <summary>
        /// Returns sorted list of expenses
        /// </summary>
        /// <param name="allExpenses">Represents list of expenses to sort</param>
        /// <param name="sortBy">Name of the property (key), based on which the expenses should be sorted</param>
        /// <param name="sortOrder">ASC or DESC</param>
        /// <returns>Returns sorted persons as PersonResponse list</returns>
        Task<List<ExpenseResponse>> GetSortedExpenses(List<ExpenseResponse> allExpenses, string sortBy, SortOrderOptions sortOrder);
    }
}
