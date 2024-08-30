using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;

namespace ExpenseTracker.Controllers
{
    [Route("[controller]")]
    public class ExpensesController : Controller
    {
        //private fields
        private readonly IExpensesService _expensesService;
        private readonly ICategoriesService _categoriesService;

        //constructor
        public ExpensesController(IExpensesService expensesService, ICategoriesService categoriesService)
        {
            _expensesService = expensesService;
            _categoriesService = categoriesService;
        }

        [Route("/")]
        [Route("[action]")]
        public async Task<IActionResult> ExpensesIndex(string searchBy, string? searchString, string sortBy = nameof(ExpenseResponse.ExpenseDate), SortOrderOptions sortOrder = SortOrderOptions.DESC)
        {
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(ExpenseResponse.Category), "Category" },
                { nameof(ExpenseResponse.ExpenseDate), "Expense Date" },
            };
            List<ExpenseResponse> expenses = await _expensesService.GetFilteredExpenses(searchBy, searchString);
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            List<ExpenseResponse> sortedExpenses = await _expensesService.GetSortedExpenses(expenses, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder;

            return View(sortedExpenses);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CreateExpense() 
        {
            List<CategoryResponse> categories = await _categoriesService.GetAllCategories();
            ViewBag.Categories = categories.Select(temp =>
                new SelectListItem() { Text = temp.CategoryName, Value = temp.CategoryID.ToString() }
            );
            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateExpense(ExpenseAddRequest expenseAddRequest)
        {
            if (!ModelState.IsValid)
            {
                List<CategoryResponse> countries = await _categoriesService.GetAllCategories();
                ViewBag.Countries = countries.Select(temp =>
                    new SelectListItem() { Text = temp.CategoryName, Value = temp.CategoryID.ToString() }
                );
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToList();
                return View();
            }
            //call the service method
            ExpenseResponse expenseResponse = await _expensesService.AddExpense(expenseAddRequest);

            return RedirectToAction("ExpensesIndex", "Expenses");
        }
    }
}
