using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LendingServices.Migrations
{
    /// <inheritdoc />
    public partial class AddDailySummaryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailySummaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SummaryDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TotalCollection = table.Column<decimal>(type: "TEXT", nullable: false),
                    Expenses = table.Column<decimal>(type: "TEXT", nullable: false),
                    AdditionalRelease = table.Column<decimal>(type: "TEXT", nullable: false),
                    NetCollection = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailySummaries", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailySummaries");
        }
    }
}
