using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenSysCom.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTablesToEnums_CategoriasIntensidadEstado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ahorro_Estado",
                table: "Ahorros");

            migrationBuilder.DropForeignKey(
                name: "FK_Ahorro_Intensidad",
                table: "Ahorros");

            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_CategoriasGastos",
                table: "Gastos");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingresos_CategoriasIngresos",
                table: "Ingresos");

            migrationBuilder.DropForeignKey(
                name: "FK_Meta_Estado",
                table: "Metas");

            migrationBuilder.DropTable(
                name: "CategoriasGastos");

            migrationBuilder.DropTable(
                name: "CategoriasIngresos");

            migrationBuilder.DropTable(
                name: "Estados");

            migrationBuilder.DropTable(
                name: "Intensidades");

            migrationBuilder.DropIndex(
                name: "IX_Metas_ID_Estado",
                table: "Metas");

            migrationBuilder.DropIndex(
                name: "IX_Ingresos_ID_CategoriaIngreso",
                table: "Ingresos");

            migrationBuilder.DropIndex(
                name: "IX_Gastos_ID_CategoriaGasto",
                table: "Gastos");

            migrationBuilder.DropIndex(
                name: "IX_Ahorros_ID_Estado",
                table: "Ahorros");

            migrationBuilder.DropIndex(
                name: "IX_Ahorros_ID_Intensidad",
                table: "Ahorros");

            migrationBuilder.AddColumn<int>(
                name: "Estado",
                table: "Metas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CategoriaGasto",
                table: "Gastos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Estado",
                table: "Ahorros",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Intensidad",
                table: "Ahorros",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Metas");

            migrationBuilder.DropColumn(
                name: "CategoriaGasto",
                table: "Gastos");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Ahorros");

            migrationBuilder.DropColumn(
                name: "Intensidad",
                table: "Ahorros");

            migrationBuilder.CreateTable(
                name: "CategoriasGastos",
                columns: table => new
                {
                    ID_CategoriasGasto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasGasto", x => x.ID_CategoriasGasto);
                });

            migrationBuilder.CreateTable(
                name: "CategoriasIngresos",
                columns: table => new
                {
                    ID_CategoriasIngreso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasIngreso", x => x.ID_CategoriasIngreso);
                });

            migrationBuilder.CreateTable(
                name: "Estados",
                columns: table => new
                {
                    ID_Estado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estados", x => x.ID_Estado);
                });

            migrationBuilder.CreateTable(
                name: "Intensidades",
                columns: table => new
                {
                    ID_Intensidad = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Intensidades", x => x.ID_Intensidad);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Metas_ID_Estado",
                table: "Metas",
                column: "ID_Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Ingresos_ID_CategoriaIngreso",
                table: "Ingresos",
                column: "ID_CategoriaIngreso");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_ID_CategoriaGasto",
                table: "Gastos",
                column: "ID_CategoriaGasto");

            migrationBuilder.CreateIndex(
                name: "IX_Ahorros_ID_Estado",
                table: "Ahorros",
                column: "ID_Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Ahorros_ID_Intensidad",
                table: "Ahorros",
                column: "ID_Intensidad");

            migrationBuilder.AddForeignKey(
                name: "FK_Ahorro_Estado",
                table: "Ahorros",
                column: "ID_Estado",
                principalTable: "Estados",
                principalColumn: "ID_Estado");

            migrationBuilder.AddForeignKey(
                name: "FK_Ahorro_Intensidad",
                table: "Ahorros",
                column: "ID_Intensidad",
                principalTable: "Intensidades",
                principalColumn: "ID_Intensidad");

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_CategoriasGastos",
                table: "Gastos",
                column: "ID_CategoriaGasto",
                principalTable: "CategoriasGastos",
                principalColumn: "ID_CategoriasGasto");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingresos_CategoriasIngresos",
                table: "Ingresos",
                column: "ID_CategoriaIngreso",
                principalTable: "CategoriasIngresos",
                principalColumn: "ID_CategoriasIngreso");

            migrationBuilder.AddForeignKey(
                name: "FK_Meta_Estado",
                table: "Metas",
                column: "ID_Estado",
                principalTable: "Estados",
                principalColumn: "ID_Estado");
        }
    }
}
