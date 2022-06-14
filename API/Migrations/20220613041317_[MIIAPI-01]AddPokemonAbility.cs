using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class MIIAPI01AddPokemonAbility : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PokemonAbilities",
                columns: table => new
                {
                    PokeId = table.Column<int>(type: "int", nullable: false),
                    PokeType = table.Column<int>(type: "int", nullable: false),
                    AbilityName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PokemonAbilities", x => new { x.PokeId, x.PokeType });
                    table.ForeignKey(
                        name: "FK_PokemonAbilities_Pokemons_PokeId",
                        column: x => x.PokeId,
                        principalTable: "Pokemons",
                        principalColumn: "PokeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PokemonAbilities_PokemonTypes_PokeType",
                        column: x => x.PokeType,
                        principalTable: "PokemonTypes",
                        principalColumn: "PokeType",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PokemonAbilities_PokeType",
                table: "PokemonAbilities",
                column: "PokeType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PokemonAbilities");
        }
    }
}
