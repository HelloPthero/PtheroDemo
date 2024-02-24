using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PtheroDemo.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class softDeleteUserInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DeleteUserId",
                table: "T_Department",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedTime",
                table: "T_Department",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteUserId",
                table: "T_Department");

            migrationBuilder.DropColumn(
                name: "DeletedTime",
                table: "T_Department");
        }
    }
}
