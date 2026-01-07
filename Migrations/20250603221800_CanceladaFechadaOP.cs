using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCPMetalurgicaInter.Migrations
{
    /// <inheritdoc />
    public partial class CanceladaFechadaOP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "Cancelada",
                table: "OPs",
                type: "bit",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "Fechada",
                table: "OPs",
                type: "bit",
                nullable: false,
                defaultValue: 0ul);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cancelada",
                table: "OPs");

            migrationBuilder.DropColumn(
                name: "Fechada",
                table: "OPs");
        }
    }
}
