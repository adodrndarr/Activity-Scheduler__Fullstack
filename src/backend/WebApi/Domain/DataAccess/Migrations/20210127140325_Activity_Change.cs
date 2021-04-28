using Microsoft.EntityFrameworkCore.Migrations;

namespace ActivityScheduler.Domain.DataAccess.Migrations
{
    public partial class Activity_Change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Activities",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "06ea2e08-bd95-4121-960a-650bd14fc326",
                column: "ConcurrencyStamp",
                value: "b74b8fbf-4652-4de8-b7b7-8c6b4498bda5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "489184c8-447c-4d0b-9d82-1b4bd5a16b5a",
                column: "ConcurrencyStamp",
                value: "84ac9470-0515-48c5-9983-acbe256a3b6e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eaed2ed5-57ac-44b1-8c1d-0aab8388b1b9",
                column: "ConcurrencyStamp",
                value: "2d5aa01c-3176-4e1d-a892-60617dfd7cad");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b90f5900-6d5a-4f71-b4b5-aa2424a60a5d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8dbd8abc-b4c4-47ca-a619-7b8c7c56ccca", "AQAAAAEAACcQAAAAEBXuGZYj59SCFzq4AifdIhnBchrAk2fwKxyYjKrX1jefnwsjgf+MlY1Aymtp8JXc/g==", "7da1e6ce-99b3-427f-a5ba-fa2384a8ddf5" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Activities");

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
    }
}
