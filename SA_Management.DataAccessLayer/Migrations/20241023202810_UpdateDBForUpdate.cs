using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SA_Management.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDBForUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfSale",
                table: "TradeDetails");

            migrationBuilder.DropColumn(
                name: "IsBroker",
                table: "TradeDetails");

            migrationBuilder.DropColumn(
                name: "QuantitySold",
                table: "TradeDetails");

            migrationBuilder.RenameColumn(
                name: "SalePrice",
                table: "TradeDetails",
                newName: "TotalAmount");

            migrationBuilder.AddColumn<decimal>(
                name: "AvgPrice",
                table: "TradeDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                table: "TradeDetails",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ShareCompanies",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CompanyPortfolios",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "BrokerDetails",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvgPrice",
                table: "TradeDetails");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "TradeDetails");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ShareCompanies");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CompanyPortfolios");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "BrokerDetails");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "TradeDetails",
                newName: "SalePrice");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfSale",
                table: "TradeDetails",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsBroker",
                table: "TradeDetails",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "QuantitySold",
                table: "TradeDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
