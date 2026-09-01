using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenSysCom.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    ID_Usuario = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NoDocumento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.ID_Usuario);
                });

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

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "ID_Usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "ID_Usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "ID_Usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "ID_Usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Gastos",
                columns: table => new
                {
                    ID_Gasto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    Descripcion = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ID_Usuario = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ID_CategoriaGasto = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gastos", x => x.ID_Gasto);
                    table.ForeignKey(
                        name: "FK_Gastos_AspNetUsers_ID_Usuario",
                        column: x => x.ID_Usuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "ID_Usuario");
                    table.ForeignKey(
                        name: "FK_Gastos_CategoriasGastos",
                        column: x => x.ID_CategoriaGasto,
                        principalTable: "CategoriasGastos",
                        principalColumn: "ID_CategoriasGasto");
                });

            migrationBuilder.CreateTable(
                name: "Ingresos",
                columns: table => new
                {
                    ID_Ingreso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    Descripcion = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ID_Usuario = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ID_CategoriaIngreso = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingresos", x => x.ID_Ingreso);
                    table.ForeignKey(
                        name: "FK_Ingresos_AspNetUsers_ID_Usuario",
                        column: x => x.ID_Usuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "ID_Usuario");
                    table.ForeignKey(
                        name: "FK_Ingresos_CategoriasIngresos",
                        column: x => x.ID_CategoriaIngreso,
                        principalTable: "CategoriasIngresos",
                        principalColumn: "ID_CategoriasIngreso");
                });

            migrationBuilder.CreateTable(
                name: "Ahorros",
                columns: table => new
                {
                    ID_Ahorro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    Descripcion = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    Porcentaje = table.Column<int>(type: "int", nullable: false),
                    ID_Usuario = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ID_Estado = table.Column<int>(type: "int", nullable: false),
                    ID_Intensidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ahorro", x => x.ID_Ahorro);
                    table.ForeignKey(
                        name: "FK_Ahorro_Estado",
                        column: x => x.ID_Estado,
                        principalTable: "Estados",
                        principalColumn: "ID_Estado");
                    table.ForeignKey(
                        name: "FK_Ahorro_Intensidad",
                        column: x => x.ID_Intensidad,
                        principalTable: "Intensidades",
                        principalColumn: "ID_Intensidad");
                    table.ForeignKey(
                        name: "FK_Ahorros_AspNetUsers_ID_Usuario",
                        column: x => x.ID_Usuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "ID_Usuario");
                });

            migrationBuilder.CreateTable(
                name: "AportesAhorros",
                columns: table => new
                {
                    ID_AporteAhorro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ID_Ahorro = table.Column<int>(type: "int", nullable: false),
                    ID_Ingreso = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AportesAhorros", x => x.ID_AporteAhorro);
                    table.ForeignKey(
                        name: "FK_AportesAhorros_Ahorros",
                        column: x => x.ID_Ahorro,
                        principalTable: "Ahorros",
                        principalColumn: "ID_Ahorro",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AportesAhorros_Ingresos",
                        column: x => x.ID_Ingreso,
                        principalTable: "Ingresos",
                        principalColumn: "ID_Ingreso",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GastosAhorros",
                columns: table => new
                {
                    ID_GastoAhorro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ID_Ahorro = table.Column<int>(type: "int", nullable: false),
                    ID_Gasto = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GastosAhorro", x => x.ID_GastoAhorro);
                    table.ForeignKey(
                        name: "FK_GastosAhorro_Ahorro",
                        column: x => x.ID_Ahorro,
                        principalTable: "Ahorros",
                        principalColumn: "ID_Ahorro",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GastosAhorro_Gasto",
                        column: x => x.ID_Gasto,
                        principalTable: "Gastos",
                        principalColumn: "ID_Gasto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Metas",
                columns: table => new
                {
                    ID_Meta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    Descripcion = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaFin = table.Column<DateOnly>(type: "date", nullable: false),
                    ID_Ahorro = table.Column<int>(type: "int", nullable: false),
                    ID_Usuario = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ID_Estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meta", x => x.ID_Meta);
                    table.ForeignKey(
                        name: "FK_Meta_Ahorro",
                        column: x => x.ID_Ahorro,
                        principalTable: "Ahorros",
                        principalColumn: "ID_Ahorro");
                    table.ForeignKey(
                        name: "FK_Meta_Estado",
                        column: x => x.ID_Estado,
                        principalTable: "Estados",
                        principalColumn: "ID_Estado");
                    table.ForeignKey(
                        name: "FK_Metas_AspNetUsers_ID_Usuario",
                        column: x => x.ID_Usuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "ID_Usuario");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ahorros_ID_Estado",
                table: "Ahorros",
                column: "ID_Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Ahorros_ID_Intensidad",
                table: "Ahorros",
                column: "ID_Intensidad");

            migrationBuilder.CreateIndex(
                name: "IX_Ahorros_ID_Usuario",
                table: "Ahorros",
                column: "ID_Usuario");

            migrationBuilder.CreateIndex(
                name: "IX_AportesAhorros_ID_Ahorro",
                table: "AportesAhorros",
                column: "ID_Ahorro");

            migrationBuilder.CreateIndex(
                name: "IX_AportesAhorros_ID_Ingreso",
                table: "AportesAhorros",
                column: "ID_Ingreso");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_ID_CategoriaGasto",
                table: "Gastos",
                column: "ID_CategoriaGasto");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_ID_Usuario",
                table: "Gastos",
                column: "ID_Usuario");

            migrationBuilder.CreateIndex(
                name: "IX_GastosAhorros_ID_Ahorro",
                table: "GastosAhorros",
                column: "ID_Ahorro");

            migrationBuilder.CreateIndex(
                name: "IX_GastosAhorros_ID_Gasto",
                table: "GastosAhorros",
                column: "ID_Gasto");

            migrationBuilder.CreateIndex(
                name: "IX_Ingresos_ID_CategoriaIngreso",
                table: "Ingresos",
                column: "ID_CategoriaIngreso");

            migrationBuilder.CreateIndex(
                name: "IX_Ingresos_ID_Usuario",
                table: "Ingresos",
                column: "ID_Usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Metas_ID_Ahorro",
                table: "Metas",
                column: "ID_Ahorro");

            migrationBuilder.CreateIndex(
                name: "IX_Metas_ID_Estado",
                table: "Metas",
                column: "ID_Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Metas_ID_Usuario",
                table: "Metas",
                column: "ID_Usuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AportesAhorros");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "GastosAhorros");

            migrationBuilder.DropTable(
                name: "Metas");

            migrationBuilder.DropTable(
                name: "Ingresos");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Gastos");

            migrationBuilder.DropTable(
                name: "Ahorros");

            migrationBuilder.DropTable(
                name: "CategoriasIngresos");

            migrationBuilder.DropTable(
                name: "CategoriasGastos");

            migrationBuilder.DropTable(
                name: "Estados");

            migrationBuilder.DropTable(
                name: "Intensidades");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
