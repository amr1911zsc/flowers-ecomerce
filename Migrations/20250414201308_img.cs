using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApplication6.Migrations
{
    /// <inheritdoc />
    public partial class img : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "67389fbf-a2d2-4647-8c82-afd2b2cc500a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "814a25f0-6c63-40d4-9b72-ea9e3f6b0556");

            migrationBuilder.AddColumn<string>(
                name: "imglink",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "c1d4a448-33e3-4262-8083-200daa428ab0", "51964848-46f8-4657-87f9-2a5be256caa2", "Customer", "customer" },
                    { "c53838ee-0973-414c-97ac-b458cd2b4ba9", "c915e30c-631c-4fb8-8f06-27ea5a75fbed", "Admin", "admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c1d4a448-33e3-4262-8083-200daa428ab0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c53838ee-0973-414c-97ac-b458cd2b4ba9");

            migrationBuilder.DropColumn(
                name: "imglink",
                table: "Products");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "67389fbf-a2d2-4647-8c82-afd2b2cc500a", "2d1e5313-ec85-4c0a-9c12-9ac3198cce42", "Admin", "admin" },
                    { "814a25f0-6c63-40d4-9b72-ea9e3f6b0556", "19ef8ae3-1a1f-4642-9569-6a3952bf4cc8", "Customer", "customer" }
                });
        }
    }
}
