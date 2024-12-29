using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SA_Management.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "FUTAveragePriceWithBroker",
                table: "CompanyPortfolios",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FUTAveragePriceWithoutBroker",
                table: "CompanyPortfolios",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "FUTQuantityWithBroker",
                table: "CompanyPortfolios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FUTQuantityWithoutBroker",
                table: "CompanyPortfolios",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FUTAveragePriceWithBroker",
                table: "CompanyPortfolios");

            migrationBuilder.DropColumn(
                name: "FUTAveragePriceWithoutBroker",
                table: "CompanyPortfolios");

            migrationBuilder.DropColumn(
                name: "FUTQuantityWithBroker",
                table: "CompanyPortfolios");

            migrationBuilder.DropColumn(
                name: "FUTQuantityWithoutBroker",
                table: "CompanyPortfolios");
        }
    }
}
