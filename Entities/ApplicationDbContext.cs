using Microsoft.EntityFrameworkCore;
using System;

namespace Entities
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Expense> Expenses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
               .ToTable("Categories")
               .HasData(
                   new Category
                   {
                       CategoryID = Guid.Parse("48109215-A887-4FEA-A7D2-76C43271530F"),
                       CategoryName = "Transportation"
                   },
                   new Category
                   {
                       CategoryID = Guid.Parse("24336D36-D18A-4181-9067-F7CF4E952D93"),
                       CategoryName = "Food"
                   },
                   new Category
                   {
                       CategoryID = Guid.Parse("61CBA2C6-F9DC-4BB0-A61C-456C447F3AFC"),
                       CategoryName = "Clothing"
                   },
                   new Category
                   {
                       CategoryID = Guid.Parse("A32AE27A-EA00-4CF0-AB4A-4A3BB74B9147"),
                       CategoryName = "Shopping"
                   }
               );

            modelBuilder.Entity<Expense>()
                    .ToTable("Expenses")
                    .HasData(
                        new Expense
                        {
                            ExpenseID = Guid.Parse("6104E09A-2E69-4CC8-8B27-3A9529780F38"),
                            ExpenseValue = 200,
                            ExpenseDate = DateTime.Parse("2024-08-09"),
                            CategoryID = Guid.Parse("48109215-A887-4FEA-A7D2-76C43271530F"),
                            ExpenseDescription = "Petrol cost."
                        },
                        new Expense
                        {
                            ExpenseID = Guid.Parse("02EFE69C-D59F-4A2C-9BAF-11930304F906"),
                            ExpenseValue = 700,
                            ExpenseDate = DateTime.Parse("2024-08-09"),
                            CategoryID = Guid.Parse("24336D36-D18A-4181-9067-F7CF4E952D93"),
                            ExpenseDescription = "Party with friends"
                        },
                        new Expense
                        {
                            ExpenseID = Guid.Parse("124352B7-97BF-4AA7-8018-9FB8970D4700"),
                            ExpenseValue = 1300,
                            ExpenseDate = DateTime.Parse("2024-08-11"),
                            CategoryID = Guid.Parse("61CBA2C6-F9DC-4BB0-A61C-456C447F3AFC"),
                            ExpenseDescription = "Bought some trousers and t-shirts."
                        },
                        new Expense
                        {
                            ExpenseID = Guid.Parse("E1381A0E-6CC9-4906-B87C-FFB003A35E85"),
                            ExpenseValue = 320,
                            ExpenseDate = DateTime.Parse("2024-08-18"),
                            CategoryID = Guid.Parse("A32AE27A-EA00-4CF0-AB4A-4A3BB74B9147"),
                            ExpenseDescription = "Some groceries"
                        },
                        new Expense
                        {
                            ExpenseID = Guid.Parse("CE15162B-04C2-4C0F-8535-F4B4FF54A7A6"),
                            ExpenseValue = 210,
                            ExpenseDate = DateTime.Parse("2024-08-16"),
                            CategoryID = Guid.Parse("48109215-A887-4FEA-A7D2-76C43271530F"),
                            ExpenseDescription = "Petrol cost"
                        }
                    );
            
        }
    }
}
