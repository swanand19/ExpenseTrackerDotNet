using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class InitialSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    ExpenseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpenseValue = table.Column<int>(type: "int", nullable: false),
                    ExpenseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpenseDescription = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CategoryID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.ExpenseID);
                    table.ForeignKey(
                        name: "FK_Expenses_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryID", "CategoryName" },
                values: new object[,]
                {
                    { new Guid("24336d36-d18a-4181-9067-f7cf4e952d93"), "Food" },
                    { new Guid("48109215-a887-4fea-a7d2-76c43271530f"), "Transportation" },
                    { new Guid("61cba2c6-f9dc-4bb0-a61c-456c447f3afc"), "Clothing" },
                    { new Guid("a32ae27a-ea00-4cf0-ab4a-4a3bb74b9147"), "Shopping" }
                });

            migrationBuilder.InsertData(
                table: "Expenses",
                columns: new[] { "ExpenseID", "CategoryID", "ExpenseDate", "ExpenseDescription", "ExpenseValue" },
                values: new object[,]
                {
                    { new Guid("02efe69c-d59f-4a2c-9baf-11930304f906"), new Guid("24336d36-d18a-4181-9067-f7cf4e952d93"), new DateTime(2024, 8, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Party with friends", 700 },
                    { new Guid("124352b7-97bf-4aa7-8018-9fb8970d4700"), new Guid("61cba2c6-f9dc-4bb0-a61c-456c447f3afc"), new DateTime(2024, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bought some trousers and t-shirts.", 1300 },
                    { new Guid("6104e09a-2e69-4cc8-8b27-3a9529780f38"), new Guid("48109215-a887-4fea-a7d2-76c43271530f"), new DateTime(2024, 8, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Petrol cost.", 200 },
                    { new Guid("ce15162b-04c2-4c0f-8535-f4b4ff54a7a6"), new Guid("48109215-a887-4fea-a7d2-76c43271530f"), new DateTime(2024, 8, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Petrol cost", 210 },
                    { new Guid("e1381a0e-6cc9-4906-b87c-ffb003a35e85"), new Guid("a32ae27a-ea00-4cf0-ab4a-4a3bb74b9147"), new DateTime(2024, 8, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Some groceries", 320 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_CategoryID",
                table: "Expenses",
                column: "CategoryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
