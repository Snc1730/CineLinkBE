using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CineLinkBE.Migrations
{
    public partial class updatePostModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Genre",
                table: "Posts");

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "DatePosted",
                value: new DateTime(2023, 11, 27, 16, 42, 57, 402, DateTimeKind.Local).AddTicks(2045));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                column: "DatePosted",
                value: new DateTime(2023, 11, 27, 16, 42, 57, 402, DateTimeKind.Local).AddTicks(2102));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "DatePosted",
                value: new DateTime(2023, 11, 27, 16, 42, 57, 402, DateTimeKind.Local).AddTicks(2119));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "DatePosted",
                value: new DateTime(2023, 11, 27, 16, 42, 57, 402, DateTimeKind.Local).AddTicks(2121));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Genre",
                table: "Posts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DatePosted", "Genre" },
                values: new object[] { new DateTime(2023, 11, 18, 2, 25, 58, 435, DateTimeKind.Local).AddTicks(2589), "Genre 1" });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DatePosted", "Genre" },
                values: new object[] { new DateTime(2023, 11, 18, 2, 25, 58, 435, DateTimeKind.Local).AddTicks(2627), "Genre 2" });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "DatePosted",
                value: new DateTime(2023, 11, 18, 2, 25, 58, 435, DateTimeKind.Local).AddTicks(2642));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "DatePosted",
                value: new DateTime(2023, 11, 18, 2, 25, 58, 435, DateTimeKind.Local).AddTicks(2644));
        }
    }
}
