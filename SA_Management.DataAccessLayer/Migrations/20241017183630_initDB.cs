using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SA_Management.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class initDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BrokerTransactions",
                columns: table => new
                {
                    BrokerTransactionID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FixedPriceCut = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrokerTransactions", x => x.BrokerTransactionID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ShareCompanies",
                columns: table => new
                {
                    CompanyID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CompanyName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareCompanies", x => x.CompanyID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CompanyPortfolios",
                columns: table => new
                {
                    CompanyPortfolioID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CompanyID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    QuantityWithoutBroker = table.Column<int>(type: "int", nullable: false),
                    AveragePriceWithoutBroker = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuantityWithBroker = table.Column<int>(type: "int", nullable: false),
                    AveragePriceWithBroker = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalInvestnment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SoldQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyPortfolios", x => x.CompanyPortfolioID);
                    table.ForeignKey(
                        name: "FK_CompanyPortfolios_ShareCompanies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "ShareCompanies",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Trades",
                columns: table => new
                {
                    TradeID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CompanyID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TradeDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TradePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TradeType = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TradeNature = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TotalAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    IsBrokerEngaged = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trades", x => x.TradeID);
                    table.ForeignKey(
                        name: "FK_Trades_ShareCompanies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "ShareCompanies",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TradeDetails",
                columns: table => new
                {
                    TradeDetailsID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TradeID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    QuantitySold = table.Column<int>(type: "int", nullable: false),
                    SalePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsBroker = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DateOfSale = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeDetails", x => x.TradeDetailsID);
                    table.ForeignKey(
                        name: "FK_TradeDetails_Trades_TradeID",
                        column: x => x.TradeID,
                        principalTable: "Trades",
                        principalColumn: "TradeID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyPortfolios_CompanyID",
                table: "CompanyPortfolios",
                column: "CompanyID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TradeDetails_TradeID",
                table: "TradeDetails",
                column: "TradeID");

            migrationBuilder.CreateIndex(
                name: "IX_Trades_CompanyID",
                table: "Trades",
                column: "CompanyID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrokerTransactions");

            migrationBuilder.DropTable(
                name: "CompanyPortfolios");

            migrationBuilder.DropTable(
                name: "TradeDetails");

            migrationBuilder.DropTable(
                name: "Trades");

            migrationBuilder.DropTable(
                name: "ShareCompanies");
        }
    }
}
