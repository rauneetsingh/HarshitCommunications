using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HarshitCommunications.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class OrderandRazorPayId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RazorpayOrderId",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RazorpayOrderId",
                table: "OrderHeaders");
        }
    }
}
