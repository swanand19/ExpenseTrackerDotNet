using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;

namespace ExpenseTracker.Controllers
{
    [Route("[controller]")]
    public class CategoriesController : Controller
    {
        //private fields
        private readonly ICategoriesService _categoriesService;

        //constructor
        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [Route("[action]")]
        public async Task<IActionResult> CategoriesIndex()
        {
            List<CategoryResponse> categoryResponsesList = await _categoriesService.GetAllCategories();
            return View(categoryResponsesList);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateCategory(CategoryAddRequest categoryAddRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToList();
                return View();
            }
            //call the service method
            CategoryResponse categoryResponse = await _categoriesService.AddCategory(categoryAddRequest);

            return RedirectToAction("CategoriesIndex", "Categories");
        }

        [HttpGet]
        [Route("[action]/{categoryID}")]
        public async Task<IActionResult> DeleteCategory(Guid categoryID)
        {
            CategoryResponse? categoryResponse = await _categoriesService.GetCategoryByCategoryID(categoryID);
            if (categoryResponse == null)
            {
                return RedirectToAction("Index");
            }
            return View(categoryResponse);
        }

        [HttpPost]
        [Route("[action]/{categoryID}")]
        public async Task<IActionResult> DeleteCategory(CategoryUpdateRequest categoryUpdateRequest)
        {
            CategoryResponse? categoryResponse = await _categoriesService.GetCategoryByCategoryID(categoryUpdateRequest.CategoryID);
            if (categoryResponse == null) { return RedirectToAction("Index"); }
            await _categoriesService.DeleteCategory(categoryUpdateRequest.CategoryID);
            return RedirectToAction("CategoriesIndex");
        }
    }
}
