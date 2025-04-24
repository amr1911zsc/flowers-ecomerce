using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApplication6.Migrations
{
    /// <inheritdoc />
    public partial class payment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "PaymentWay",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "OrderStatus",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "008fabc0-175d-4fe2-915f-691e37daf523", "7a572ad5-fcd3-4352-9675-f8915864332e", "Customer", "CUSTOMER" },
                    { "8ee86160-0b66-463f-a893-cf38ff35ddfb", "0651748d-f4cc-4c4f-8627-222a8e3ef9f6", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "008fabc0-175d-4fe2-915f-691e37daf523");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8ee86160-0b66-463f-a893-cf38ff35ddfb");

            migrationBuilder.AlterColumn<string>(
                name: "OrderStatus",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "PaymentWay",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3bdf87b5-8161-4910-abad-922bdb62234b", "db8f50ce-d15d-47e8-a08c-ca199b727165", "Customer", "customer" },
                    { "c290f0de-78c7-466b-8f99-38db3170f6df", "0709906f-10e7-449d-88d9-60491b8671fd", "Admin", "admin" }
                });
        }
    }
}
