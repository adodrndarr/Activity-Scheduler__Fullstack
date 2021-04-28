using Microsoft.EntityFrameworkCore.Migrations;

namespace ActivityScheduler.Domain.DataAccess.Migrations
{
    public partial class Activity_User_ForeignKey_Change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "06ea2e08-bd95-4121-960a-650bd14fc326",
                column: "ConcurrencyStamp",
                value: "7d0db373-5f5f-4912-bf61-d6fbd23a1e5f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "489184c8-447c-4d0b-9d82-1b4bd5a16b5a",
                column: "ConcurrencyStamp",
                value: "ecaf811e-6a26-4754-adef-88a2c8a7b7fa");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eaed2ed5-57ac-44b1-8c1d-0aab8388b1b9",
                column: "ConcurrencyStamp",
                value: "7f3eaa1e-5037-4750-9075-e1419c1ae150");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b90f5900-6d5a-4f71-b4b5-aa2424a60a5d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1a6ae271-db45-4bbd-931e-912907039e29", "AQAAAAEAACcQAAAAEM4IBlbgBS9I9s+uLt1gSRP9eIYDLC+ZqHf46p+j4QgYL2gLSKfRh0W3C1kyfgmZgQ==", "4982de2a-77e7-46ac-93b2-9492bfecb0b7" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "06ea2e08-bd95-4121-960a-650bd14fc326",
                column: "ConcurrencyStamp",
                value: "0948916a-9edd-46fd-bf1c-3bcd42dffcff");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "489184c8-447c-4d0b-9d82-1b4bd5a16b5a",
                column: "ConcurrencyStamp",
                value: "9c7ba2d8-a0dd-44f6-900a-095bd92ce71f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eaed2ed5-57ac-44b1-8c1d-0aab8388b1b9",
                column: "ConcurrencyStamp",
                value: "79cbc29c-657a-493c-8f33-ee2e90760e5f");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b90f5900-6d5a-4f71-b4b5-aa2424a60a5d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "723bedf8-3afd-4a56-b7b6-002423f72ca8", "AQAAAAEAACcQAAAAEA9Vlv6FNzhAal2aGXSFTQxOTb0oMxuzpkKYCF5z/zRP715RKQ/GstKkM4R+gHCYPA==", "473c6270-2770-40a2-a444-c6ae0c803aae" });
        }
    }
}
