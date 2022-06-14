using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class MIIAPI01RemovePokemonImgColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Img",
                table: "Pokemons");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Img",
                table: "Pokemons",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
