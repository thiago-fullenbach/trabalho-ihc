using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PontoFacil.Api.Migrations
{
    public partial class DocGeralV1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TiposAjuste",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    descricao = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    eh_inclusao = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    eh_alteracao = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    eh_exclusao = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposAjuste", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TiposLogradouro",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    descricao = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposLogradouro", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UFs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UFs", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cpf = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    data_nascimento = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    horas_diarias = table.Column<int>(type: "int", nullable: false),
                    login = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    senha = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    datahora_criacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    datahora_modificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    datahora_modificacao_login = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    datahora_modificacao_senha = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    eh_senha_temporaria = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Cidades",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ufs_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cidades", x => x.id);
                    table.ForeignKey(
                        name: "FK_Cidades_UFs_ufs_id",
                        column: x => x.ufs_id,
                        principalTable: "UFs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Recursos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    usuarios_id = table.Column<int>(type: "int", nullable: false),
                    pode_vis_usuario = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    pode_cad_usuario = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    pode_vis_demais_usuarios = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    pode_cad_demais_usuarios = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    pode_cad_administrador_acessos = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    pode_vis_ponto = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    pode_cad_ponto = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    pode_vis_ponto_demais_usuarios = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    pode_vis_ajuste = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    pode_vis_ajuste_demais_usuarios = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    pode_cad_ajuste_demais_usuarios = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    datahora_criacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    datahora_modificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recursos", x => x.id);
                    table.ForeignKey(
                        name: "FK_Recursos_Usuarios_usuarios_id",
                        column: x => x.usuarios_id,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Sessoes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    hex_verificacao = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    usuarios_id = table.Column<int>(type: "int", nullable: false),
                    datahora_ultima_autenticacao = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessoes", x => x.id);
                    table.ForeignKey(
                        name: "FK_Sessoes_Usuarios_usuarios_id",
                        column: x => x.usuarios_id,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Locais",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    tiposLogradouro_id = table.Column<int>(type: "int", nullable: false),
                    logradouro = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    numero = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    complemento = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    bairro = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cidades_id = table.Column<int>(type: "int", nullable: false),
                    datahora_criacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    datahora_modificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locais", x => x.id);
                    table.ForeignKey(
                        name: "FK_Locais_Cidades_cidades_id",
                        column: x => x.cidades_id,
                        principalTable: "Cidades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Locais_TiposLogradouro_tiposLogradouro_id",
                        column: x => x.tiposLogradouro_id,
                        principalTable: "TiposLogradouro",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Apelidos",
                columns: table => new
                {
                    usuarios_id = table.Column<int>(type: "int", nullable: false),
                    locais_id = table.Column<int>(type: "int", nullable: false),
                    apelido = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    datahora_criacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    datahora_modificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apelidos", x => new { x.usuarios_id, x.locais_id });
                    table.ForeignKey(
                        name: "FK_Apelidos_Locais_locais_id",
                        column: x => x.locais_id,
                        principalTable: "Locais",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Apelidos_Usuarios_usuarios_id",
                        column: x => x.usuarios_id,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Presencas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    usuarios_id = table.Column<int>(type: "int", nullable: false),
                    locais_id = table.Column<int>(type: "int", nullable: false),
                    eh_entrada = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    datahora_presenca = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Presencas", x => x.id);
                    table.ForeignKey(
                        name: "FK_Presencas_Locais_locais_id",
                        column: x => x.locais_id,
                        principalTable: "Locais",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Presencas_Usuarios_usuarios_id",
                        column: x => x.usuarios_id,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ajustes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    usuarios_id_modificador = table.Column<int>(type: "int", nullable: false),
                    presencas_id = table.Column<int>(type: "int", nullable: true),
                    locais_id_ajuste = table.Column<int>(type: "int", nullable: true),
                    eh_entrada_ajuste = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    datahora_presenca_ajuste = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    tipo_ajuste = table.Column<int>(type: "int", nullable: false),
                    observacao = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    datahora_criacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    datahora_modificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ajustes", x => x.id);
                    table.ForeignKey(
                        name: "FK_Ajustes_Locais_locais_id_ajuste",
                        column: x => x.locais_id_ajuste,
                        principalTable: "Locais",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Ajustes_Presencas_presencas_id",
                        column: x => x.presencas_id,
                        principalTable: "Presencas",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Ajustes_TiposAjuste_tipo_ajuste",
                        column: x => x.tipo_ajuste,
                        principalTable: "TiposAjuste",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ajustes_Usuarios_usuarios_id_modificador",
                        column: x => x.usuarios_id_modificador,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Ajustes_locais_id_ajuste",
                table: "Ajustes",
                column: "locais_id_ajuste");

            migrationBuilder.CreateIndex(
                name: "IX_Ajustes_presencas_id",
                table: "Ajustes",
                column: "presencas_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ajustes_tipo_ajuste",
                table: "Ajustes",
                column: "tipo_ajuste");

            migrationBuilder.CreateIndex(
                name: "IX_Ajustes_usuarios_id_modificador",
                table: "Ajustes",
                column: "usuarios_id_modificador");

            migrationBuilder.CreateIndex(
                name: "IX_Apelidos_locais_id",
                table: "Apelidos",
                column: "locais_id");

            migrationBuilder.CreateIndex(
                name: "IX_Cidades_ufs_id",
                table: "Cidades",
                column: "ufs_id");

            migrationBuilder.CreateIndex(
                name: "IX_Locais_cidades_id",
                table: "Locais",
                column: "cidades_id");

            migrationBuilder.CreateIndex(
                name: "IX_Locais_tiposLogradouro_id",
                table: "Locais",
                column: "tiposLogradouro_id");

            migrationBuilder.CreateIndex(
                name: "IX_Presencas_locais_id",
                table: "Presencas",
                column: "locais_id");

            migrationBuilder.CreateIndex(
                name: "IX_Presencas_usuarios_id",
                table: "Presencas",
                column: "usuarios_id");

            migrationBuilder.CreateIndex(
                name: "IX_Recursos_usuarios_id",
                table: "Recursos",
                column: "usuarios_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sessoes_usuarios_id",
                table: "Sessoes",
                column: "usuarios_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ajustes");

            migrationBuilder.DropTable(
                name: "Apelidos");

            migrationBuilder.DropTable(
                name: "Recursos");

            migrationBuilder.DropTable(
                name: "Sessoes");

            migrationBuilder.DropTable(
                name: "Presencas");

            migrationBuilder.DropTable(
                name: "TiposAjuste");

            migrationBuilder.DropTable(
                name: "Locais");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Cidades");

            migrationBuilder.DropTable(
                name: "TiposLogradouro");

            migrationBuilder.DropTable(
                name: "UFs");
        }
    }
}
