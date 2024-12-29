using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SA_Management.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDBDForBrokerDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrokerTransactions");

            migrationBuilder.CreateTable(
                name: "BrokerDetails",
                columns: table => new
                {
                    BrokerID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FixedPriceCut = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrokerDetails", x => x.BrokerID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrokerDetails");

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
        }
    }
}
