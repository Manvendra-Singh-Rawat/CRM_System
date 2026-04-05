using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class V048Migration_ForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Works",
                newName: "IsPaid");

            migrationBuilder.RenameColumn(
                name: "PaymentStatus",
                table: "Works",
                newName: "IsCompleted");

            migrationBuilder.CreateIndex(
                name: "IX_Works_ClientId",
                table: "Works",
                column: "ClientId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Works_Users_ClientId",
                table: "Works",
                column: "ClientId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Works_Users_ClientId",
                table: "Works");

            migrationBuilder.DropIndex(
                name: "IX_Works_ClientId",
                table: "Works");

            migrationBuilder.RenameColumn(
                name: "IsPaid",
                table: "Works",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "IsCompleted",
                table: "Works",
                newName: "PaymentStatus");
        }
    }
}
