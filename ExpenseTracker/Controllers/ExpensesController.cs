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
        public async Task<IActionResult> ExpensesIndex(string searchBy, string? searchString, string sortBy = nameof(ExpenseResponse.ExpenseDate), SortOrderOptions sortOrder = SortOrderOptions.DESC, DateTime? selectedDate = null, int monthOffset=0)
        {
            //default to current month's expenses
            DateTime currentMonth = DateTime.Today.AddMonths(monthOffset);
            DateTime startOfMonth = new DateTime(currentMonth.Year, currentMonth.Month, 1);
            DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            List<ExpenseResponse> expenses = await _expensesService.GetAllExpenses();

            if (selectedDate == null)
            {
                //fetch expesnses for current month if no specific date is selected
                expenses = await _expensesService.GetFilteredExpenses(searchBy, searchString);
                expenses = expenses.Where(e => e.ExpenseDate >= startOfMonth && e.ExpenseDate <= endOfMonth).ToList();
                ViewBag.SelectedMonth = startOfMonth.ToString("MMM");
                ViewBag.MonthOffset = monthOffset; // Keep track of the month offset
            }
            else
            {
                //fetch expenses for selected date
                expenses = await _expensesService.GetFilteredExpenses(searchBy, searchString);
                expenses = expenses.Where(e => e.ExpenseDate?.Date == selectedDate.Value.Date).ToList();
                ViewBag.SelectedDate = selectedDate?.ToString("yyyy-MM-dd");
            }

            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(ExpenseResponse.Category), "Category" },
                { nameof(ExpenseResponse.ExpenseDate), "Expense Date" },
            }; 

            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;


            List<ExpenseResponse> sortedExpenses = await _expensesService.GetSortedExpenses(expenses, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder;

            //calculate the sum of expenses
            decimal totalExpense = expenses.Sum(e => e.ExpenseValue);
            ViewBag.TotalExpense = totalExpense;

            // Calendar view variables
            int daysInMonth = DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);
            int firstDayOfWeek = (int)new DateTime(currentMonth.Year, currentMonth.Month, 1).DayOfWeek;
            int weeksInMonth = (int)Math.Ceiling((daysInMonth + firstDayOfWeek) / 7.0);

            ViewBag.DaysInMonth = daysInMonth;
            ViewBag.FirstDayOfWeek = firstDayOfWeek;
            ViewBag.WeeksInMonth = weeksInMonth;

            var dailyExpenses = new Dictionary<int, decimal>();
            for(int day = 1; day <= daysInMonth; day++)
            {
                DateTime date = new DateTime(currentMonth.Year, currentMonth.Month, day);
                decimal totalForDay = expenses.Where(e => e.ExpenseDate?.Date == date).Sum(e => e.ExpenseValue);
                dailyExpenses[day] = totalForDay;
            }
            // Pass the daily expenses dictionary to the view
            ViewBag.DailyExpenses = dailyExpenses;

            return View(sortedExpenses);  //specifying the list view explicitly
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

        [HttpGet]
        [Route("[action]/{expenseID}")]
        public async Task<IActionResult> EditExpense(Guid expenseID)
        {
            ExpenseResponse? expenseResponse = await _expensesService.GetExpenseByExpenseID(expenseID);
            if(expenseResponse == null) { return RedirectToAction("ExpensesIndex"); }
            ExpenseUpdateRequest expenseUpdateRequest = expenseResponse.ToExpenseUpdateRequest();
            List<CategoryResponse> categories = await _categoriesService.GetAllCategories();
            ViewBag.Categories = categories.Select(temp => new SelectListItem() { Text = temp.CategoryName,
                Value = temp.CategoryID.ToString() });

            return View(expenseUpdateRequest);
        }

        [HttpPost]
        [Route("[action]/{expenseID}")]
        public async Task<IActionResult> EditExpense(ExpenseUpdateRequest expenseUpdateRequest)
        {
            ExpenseResponse? expenseResponse = await _expensesService.GetExpenseByExpenseID(expenseUpdateRequest.ExpenseID);
            if(expenseResponse == null) { return RedirectToAction("ExpensesIndex"); }
            if (ModelState.IsValid)
            {
                ExpenseResponse updateExpense = await _expensesService.UpdateExpense(expenseUpdateRequest);
                return RedirectToAction("ExpensesIndex");
            }
            else
            {
                List<CategoryResponse> categories = await _categoriesService.GetAllCategories();
                ViewBag.Categories = categories.Select(temp =>
                    new SelectListItem() { Text = temp.CategoryName, Value = temp.CategoryID.ToString() }
                );

                ViewBag.Errors =
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(expenseResponse.ToExpenseUpdateRequest());
            }
        }


        [HttpGet]
        [Route("[action]/{expenseID}")]
        public async Task<IActionResult> DeleteExpense(Guid expenseID)
        {
            ExpenseResponse? expenseResponse = await _expensesService.GetExpenseByExpenseID(expenseID);
            if (expenseResponse == null)
            {
                return RedirectToAction("ExpensesIndex");
            }
            return View(expenseResponse);
        }

        [HttpPost]
        [Route("[action]/{expenseID}")]
        public async Task<IActionResult> DeleteExpense(ExpenseUpdateRequest expenseUpdateRequest)
        {
            ExpenseResponse? expenseResponse  = await _expensesService.GetExpenseByExpenseID(expenseUpdateRequest.ExpenseID);
            if (expenseResponse == null) { return RedirectToAction("ExpensesIndex"); }
            await _expensesService.DeleteExpense(expenseUpdateRequest.ExpenseID);
            return RedirectToAction("ExpensesIndex");
        }
    }
}
