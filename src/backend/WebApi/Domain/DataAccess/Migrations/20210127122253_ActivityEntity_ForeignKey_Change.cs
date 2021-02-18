using Microsoft.EntityFrameworkCore.Migrations;

namespace ActivityScheduler.Domain.DataAccess.Migrations
{
    public partial class ActivityEntity_ForeignKey_Change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "ItemQuantity",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "MaxUserCount",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "MinUserCount",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Activities");

            migrationBuilder.AddColumn<string>(
                name: "ActivityEntityId",
                table: "Activities",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "06ea2e08-bd95-4121-960a-650bd14fc326",
                column: "ConcurrencyStamp",
                value: "9190d7f0-5f85-41d5-9446-8f78f17538d9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "489184c8-447c-4d0b-9d82-1b4bd5a16b5a",
                column: "ConcurrencyStamp",
                value: "37c302d6-d98f-451d-a27b-739f33e53c9f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eaed2ed5-57ac-44b1-8c1d-0aab8388b1b9",
                column: "ConcurrencyStamp",
                value: "67f4cba3-12d1-4c88-8b5c-d0ef3d8a4c0e");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b90f5900-6d5a-4f71-b4b5-aa2424a60a5d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a719ca20-b08c-433e-a3d8-bbe3a34f2d95", "AQAAAAEAACcQAAAAECCikYLFPnsZ23FL/MhVi0kNU51JV+opYK2WypMTm03ebBK/Nfl/eKL0IoLRhJYH3Q==", "ec3e8630-3878-4b27-94c4-f17a757c3673" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityEntityId",
                table: "Activities");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemQuantity",
                table: "Activities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxUserCount",
                table: "Activities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinUserCount",
                table: "Activities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: true);

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
    }
}
