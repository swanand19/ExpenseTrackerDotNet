using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using System;

namespace Services
{
    public class CategoriesService : ICategoriesService
    {
        //private fields
        private readonly ApplicationDbContext _db;

        //constructor
        public CategoriesService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<CategoryResponse> AddCategory(CategoryAddRequest? categoryRequest)
        {
            //validation : CategoryRequest can't be null
            if(categoryRequest == null)
            { throw new ArgumentNullException(nameof(categoryRequest)); }
            
            //validation : CategoryName is null
            if(categoryRequest.CategoryName == null)
            { throw new ArgumentException(nameof(categoryRequest.CategoryName)); }

            //validation : if category name is duplicate
            if (await _db.Categories
                .CountAsync(temp => temp.CategoryName == categoryRequest.CategoryName) > 0) 
                { throw new ArgumentException("Given category name already exists"); }

            //convert object from CategoryRequest to Category type
            Category category = categoryRequest.ToCategory();

            //generate guid for Category (CategoryID)
            category.CategoryID = Guid.NewGuid();

            //add category object into _db
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();

            return category.ToCategoryResponse();
        }

        public async Task<bool> DeleteCategory(Guid? categoryID)
        {
            if (categoryID == null) { throw new ArgumentNullException(nameof(categoryID)); }
            Category? category = await _db.Categories
                                        .FirstOrDefaultAsync(temp => temp.CategoryID == categoryID);
            if(category == null) return false;
            _db.Categories.Remove(_db.Categories.First(temp => temp.CategoryID == categoryID));
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<List<CategoryResponse>> GetAllCategories()
        {
            return await _db.Categories.Select(category => category.ToCategoryResponse()).ToListAsync();
        }

        public async Task<CategoryResponse?> GetCategoryByCategoryID(Guid? categoryID)
        {
            if (categoryID == null) { return null; }

            Category? category_response_from_list = await _db.Categories.FirstOrDefaultAsync(temp => temp.CategoryID == categoryID);
            if(category_response_from_list == null) { return null; }
            return category_response_from_list.ToCategoryResponse();
        }
    }
}