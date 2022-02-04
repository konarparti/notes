using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductID);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductID", "Category", "Description", "Name", "Price" },
                values: new object[] { new Guid("4aa76a4c-c59d-409a-84c1-06e6487a137a"), "Soccer", "A ball for paly soccer", "Soccer ball", 120.50m });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductID", "Category", "Description", "Name", "Price" },
                values: new object[] { new Guid("63dc8fa6-07ae-4391-8916-e057f71239ce"), "Watersport", "A boat for one person", "Kayak", 5000m });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductID", "Category", "Description", "Name", "Price" },
                values: new object[] { new Guid("70bf165a-700a-4156-91c0-e83fce0a277f"), "Football", "A ball for paly footbal", "Football", 100m });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
