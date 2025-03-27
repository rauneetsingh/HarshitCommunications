using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HarshitCommunications.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class PaymentAndSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "OrderHeaders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "OrderHeaders");
        }
    }
}
