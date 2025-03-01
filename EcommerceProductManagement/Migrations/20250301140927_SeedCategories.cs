using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EcommerceProductManagement.Migrations
{
    /// <inheritdoc />
    public partial class SeedCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "All electronic devices, including gadgets, appliances, and accessories.", "Electronics" },
                    { 2, "Trendy clothing, shoes, and accessories for men, women, and kids.", "Fashion" },
                    { 3, "Furniture, appliances, cookware, and home decor items.", "Home & Kitchen" },
                    { 4, "Skincare, haircare, cosmetics, and grooming products.", "Beauty & Personal Care" },
                    { 5, "Sports gear, fitness equipment, and outdoor adventure essentials.", "Sports & Outdoors" },
                    { 6, "Car accessories, parts, and maintenance products.", "Automotive" },
                    { 7, "Novels, textbooks, office supplies, and study materials.", "Books & Stationery" },
                    { 8, "Kids' toys, board games, and video gaming accessories.", "Toys & Games" },
                    { 9, "Supplements, fitness gear, and medical essentials.", "Health & Wellness" },
                    { 10, "Food, beverages, and daily household necessities.", "Groceries & Essentials" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
