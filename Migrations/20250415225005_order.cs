using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApplication6.Migrations
{
    /// <inheritdoc />
    public partial class order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c1d4a448-33e3-4262-8083-200daa428ab0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c53838ee-0973-414c-97ac-b458cd2b4ba9");

            migrationBuilder.AddColumn<decimal>(
                name: "PriceAtPurchase",
                table: "OrderDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3bdf87b5-8161-4910-abad-922bdb62234b", "db8f50ce-d15d-47e8-a08c-ca199b727165", "Customer", "customer" },
                    { "c290f0de-78c7-466b-8f99-38db3170f6df", "0709906f-10e7-449d-88d9-60491b8671fd", "Admin", "admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3bdf87b5-8161-4910-abad-922bdb62234b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c290f0de-78c7-466b-8f99-38db3170f6df");

            migrationBuilder.DropColumn(
                name: "PriceAtPurchase",
                table: "OrderDetails");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "c1d4a448-33e3-4262-8083-200daa428ab0", "51964848-46f8-4657-87f9-2a5be256caa2", "Customer", "customer" },
                    { "c53838ee-0973-414c-97ac-b458cd2b4ba9", "c915e30c-631c-4fb8-8f06-27ea5a75fbed", "Admin", "admin" }
                });
        }
    }
}
