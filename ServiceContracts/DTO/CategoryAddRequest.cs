using Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    public class CategoryAddRequest
    {
        [Required(ErrorMessage ="Category name can't be blank")]
        public string? CategoryName { get; set; }

        public Category ToCategory()
        {
            return new Category()
            {
                CategoryName = CategoryName
            };
        }
    } 
}