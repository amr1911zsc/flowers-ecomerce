using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApplication6.Migrations
{
    /// <inheritdoc />
    public partial class status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "008fabc0-175d-4fe2-915f-691e37daf523");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8ee86160-0b66-463f-a893-cf38ff35ddfb");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "38cc95c4-ffe7-403b-b495-a1eadb4dc9f4", "bf64252a-3c19-4d25-8f53-fd5bfc71a018", "Admin", "ADMIN" },
                    { "935da969-1d29-4a4b-9b63-bf500552e04b", "d43ffb27-8d24-4a05-8747-46ddcf573b1e", "Customer", "CUSTOMER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "38cc95c4-ffe7-403b-b495-a1eadb4dc9f4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "935da969-1d29-4a4b-9b63-bf500552e04b");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "008fabc0-175d-4fe2-915f-691e37daf523", "7a572ad5-fcd3-4352-9675-f8915864332e", "Customer", "CUSTOMER" },
                    { "8ee86160-0b66-463f-a893-cf38ff35ddfb", "0651748d-f4cc-4c4f-8627-222a8e3ef9f6", "Admin", "ADMIN" }
                });
        }
    }
}
