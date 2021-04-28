using Microsoft.EntityFrameworkCore.Migrations;

namespace ActivityScheduler.Domain.DataAccess.Migrations
{
    public partial class ActivityEntity_Modification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "ActivityEntities");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "ActivityEntities",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "06ea2e08-bd95-4121-960a-650bd14fc326",
                column: "ConcurrencyStamp",
                value: "a086c01b-2918-40b7-9423-711feadd070c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "489184c8-447c-4d0b-9d82-1b4bd5a16b5a",
                column: "ConcurrencyStamp",
                value: "d1735f6c-1b79-40ec-a5d1-42d3ff8a1406");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eaed2ed5-57ac-44b1-8c1d-0aab8388b1b9",
                column: "ConcurrencyStamp",
                value: "db306834-cd76-4940-be28-bfd5c4875dc0");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b90f5900-6d5a-4f71-b4b5-aa2424a60a5d",
                columns: new[] { "ConcurrencyStamp", "IsAdmin", "PasswordHash", "SecurityStamp" },
                values: new object[] { "350bd93d-ad51-45df-898d-2bdeedba6726", true, "AQAAAAEAACcQAAAAEEyFjkaqEUOV5InvCEW7oQJSfrzpd8zQbHTL0P94NYOJlcZV3OGz8RlkAYkU3WMcVQ==", "9b0a806a-8262-494f-b78a-e600e764d007" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "ActivityEntities");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "ActivityEntities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "06ea2e08-bd95-4121-960a-650bd14fc326",
                column: "ConcurrencyStamp",
                value: "93e88219-0b92-4507-b6d9-a01a17d31c7a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "489184c8-447c-4d0b-9d82-1b4bd5a16b5a",
                column: "ConcurrencyStamp",
                value: "a2b6221f-6e98-4d82-8707-d11e6e7a3b26");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eaed2ed5-57ac-44b1-8c1d-0aab8388b1b9",
                column: "ConcurrencyStamp",
                value: "ff73219c-d5ec-4e23-8c43-7b48227921a1");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b90f5900-6d5a-4f71-b4b5-aa2424a60a5d",
                columns: new[] { "ConcurrencyStamp", "IsAdmin", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b8ab74ac-200c-4508-96da-dce32077df75", false, "AQAAAAEAACcQAAAAEKj+nvDxxiVvg7r120hPYBMeIME4lmIASovSon2dzTmqzkkEPMZ9n3yW4f36bwqLAw==", "bdf6abc6-490a-411f-95ac-582043987e91" });
        }
    }
}
