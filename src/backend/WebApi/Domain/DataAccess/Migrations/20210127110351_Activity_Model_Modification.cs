using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ActivityScheduler.Domain.DataAccess.Migrations
{
    public partial class Activity_Model_Modification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_ActivityEntities_ActivityEntityId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_ActivityEntityId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "ActivityEntityId",
                table: "Activities");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Activities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Activities",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemQuantity",
                table: "Activities",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Activities",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxUserCount",
                table: "Activities",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinUserCount",
                table: "Activities",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Activities",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<Guid>(
                name: "ActivityEntityId",
                table: "Activities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "06ea2e08-bd95-4121-960a-650bd14fc326",
                column: "ConcurrencyStamp",
                value: "a1d85613-6aba-4a49-92ed-77acf06d9f20");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "489184c8-447c-4d0b-9d82-1b4bd5a16b5a",
                column: "ConcurrencyStamp",
                value: "8e1ec1a5-0ae5-4c5c-86d5-269af0a46c98");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eaed2ed5-57ac-44b1-8c1d-0aab8388b1b9",
                column: "ConcurrencyStamp",
                value: "e4ed0e14-b57a-4889-b8c4-848ecd2840bf");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b90f5900-6d5a-4f71-b4b5-aa2424a60a5d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "58255700-e113-4941-a026-fbe6ea7e9cd1", "AQAAAAEAACcQAAAAEMnMXEMIkrXD5E9waeskuF70mqxGKliP79YwvGEpAkiEhlFR2Mt9cziV4q7E4wpbhg==", "495d42fd-8064-40be-b5dd-4d6464aacc8f" });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ActivityEntityId",
                table: "Activities",
                column: "ActivityEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_ActivityEntities_ActivityEntityId",
                table: "Activities",
                column: "ActivityEntityId",
                principalTable: "ActivityEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
