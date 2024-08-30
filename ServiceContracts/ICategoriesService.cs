using ServiceContracts.DTO;
using System;

namespace ServiceContracts
{
    public interface ICategoriesService
    {
        /// <summary>
        /// Adds a category object to the list of categories
        /// </summary>
        /// <param name="categoryRequest">Parameter that represents category object to be added</param>
        /// <returns>returns category object as CategoryResponse</returns>
        Task<CategoryResponse> AddCategory(CategoryAddRequest? categoryRequest);

        /// <summary>
        /// Returns all categories from the list
        /// </summary>
        /// <returns>All categories from the list as list of CategoryResponse</returns>
        Task<List<CategoryResponse>> GetAllCategories();

        /// <summary>
        /// Returns a category object based on the given id
        /// </summary>
        /// <param name="categoryID">Category ID we want to search</param>
        /// <returns>Matching category as CategoryResponse object</returns>
        Task<CategoryResponse?> GetCategoryByCategoryID(Guid? categoryID);

        /// <summary>
        /// Deletes a category based on the given category id
        /// </summary>
        /// <param name="categoryID">Category ID to delete</param>
        /// <returns>Returns true if deletion is successfull, otherwise flase</returns>
        Task<bool> DeletePerson(Guid? categoryID);
    }
}
