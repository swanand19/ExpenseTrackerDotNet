using Entities;
using System;
using System.ComponentModel.DataAnnotations;


namespace ServiceContracts.DTO
{
    public class CategoryUpdateRequest
    {
        [Key]
        public Guid CategoryID { get; set; }

        [Required(ErrorMessage = "Category name can't be blank")]
        public string? CategoryName { get; set; }

        /// <summary>
        /// Reprents the dto class that contains the category details to update
        /// </summary>
        public class CategoryAddRequest
        {
            [Required(ErrorMessage ="Category ID can't be blank")]
            public Guid CategoryID { get; set; }

            [Required(ErrorMessage ="Category Name can't be blank")]
            public string? CategoryName { get; set; }

            /// <summary>
            /// Converts the current object of CategoryAddRequest into a new object of Person type.
            /// </summary>
            /// <returns></returns>
            public Category ToCategory()
            {
                return new Category()
                {
                    CategoryID = CategoryID,
                    CategoryName = CategoryName
                };
            }
        }
    }
}
