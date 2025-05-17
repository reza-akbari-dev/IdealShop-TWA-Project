using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdealShop.Migrations
{
    /// <inheritdoc />
    public partial class adminPassProblem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "Address", "Email", "FirstName", "LastName", "Password", "PhoneNumber", "Salt" },
                values: new object[] { 1, "Admin Address", "admin@idealshop.com", "Admin", "User", "bCT7At2w6xf6ORSVVctqBhNM/pCZmLWohD0Klgc1awQ=", "1234567890", "Ribvc2GsTaAEGU2aLs+k5g==" });
        }
    }
}
