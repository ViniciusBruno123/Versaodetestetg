using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCPMetalurgicaInter.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Funcionario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(128)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Login = table.Column<string>(type: "varchar(64)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Senha = table.Column<string>(type: "varchar(128)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcionario", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Pecas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descricao = table.Column<string>(type: "varchar(256)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Data_Cadastro = table.Column<DateTime>(type: "Datetime", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Situacao = table.Column<ulong>(type: "bit", nullable: false),
                    Imagem = table.Column<string>(type: "varchar(256)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DadosImagem = table.Column<string>(type: "varchar(2)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CodigoUniversal = table.Column<string>(type: "varchar(256)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pecas", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TipoOperacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descricao = table.Column<string>(type: "varchar(256)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoOperacoes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Operadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FuncionarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operadores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operadores_Funcionario_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "Funcionario",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PCPs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FuncionarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PCPs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PCPs_Funcionario_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "Funcionario",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Subprodutos",
                columns: table => new
                {
                    PecaId = table.Column<int>(type: "int", nullable: false),
                    PecaSubId = table.Column<int>(type: "int", nullable: false),
                    Quantidade = table.Column<float>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subprodutos", x => new { x.PecaId, x.PecaSubId });
                    table.ForeignKey(
                        name: "FK_Subprodutos_Pecas_PecaId",
                        column: x => x.PecaId,
                        principalTable: "Pecas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Subprodutos_Pecas_PecaSubId",
                        column: x => x.PecaSubId,
                        principalTable: "Pecas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Operacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descricao = table.Column<string>(type: "varchar(256)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoDeOperacaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operacoes_TipoOperacoes_TipoDeOperacaoId",
                        column: x => x.TipoDeOperacaoId,
                        principalTable: "TipoOperacoes",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OPs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    DataEmissao = table.Column<DateTime>(type: "Datetime", nullable: false),
                    PecaId = table.Column<int>(type: "int", nullable: false),
                    PCPEmissorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OPs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OPs_PCPs_PCPEmissorId",
                        column: x => x.PCPEmissorId,
                        principalTable: "PCPs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OPs_Pecas_PecaId",
                        column: x => x.PecaId,
                        principalTable: "Pecas",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PecaOperacoes",
                columns: table => new
                {
                    PecaId = table.Column<int>(type: "int", nullable: false),
                    OperacaoId = table.Column<int>(type: "int", nullable: false),
                    etapa = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PecaOperacoes", x => new { x.PecaId, x.OperacaoId });
                    table.ForeignKey(
                        name: "FK_PecaOperacoes_Operacoes_OperacaoId",
                        column: x => x.OperacaoId,
                        principalTable: "Operacoes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PecaOperacoes_Pecas_PecaId",
                        column: x => x.PecaId,
                        principalTable: "Pecas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Apontamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrdemDeProducaoId = table.Column<int>(type: "int", nullable: false),
                    OperadorId = table.Column<int>(type: "int", nullable: false),
                    OperacaoId = table.Column<int>(type: "int", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: true),
                    Inicio = table.Column<DateTime>(type: "Datetime", nullable: false),
                    Fim = table.Column<DateTime>(type: "Datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apontamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Apontamentos_OPs_OrdemDeProducaoId",
                        column: x => x.OrdemDeProducaoId,
                        principalTable: "OPs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Apontamentos_Operacoes_OperacaoId",
                        column: x => x.OperacaoId,
                        principalTable: "Operacoes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Apontamentos_Operadores_OperadorId",
                        column: x => x.OperadorId,
                        principalTable: "Operadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Apontamentos_OperacaoId",
                table: "Apontamentos",
                column: "OperacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Apontamentos_OperadorId",
                table: "Apontamentos",
                column: "OperadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Apontamentos_OrdemDeProducaoId",
                table: "Apontamentos",
                column: "OrdemDeProducaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Operacoes_TipoDeOperacaoId",
                table: "Operacoes",
                column: "TipoDeOperacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Operadores_FuncionarioId",
                table: "Operadores",
                column: "FuncionarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OPs_PCPEmissorId",
                table: "OPs",
                column: "PCPEmissorId");

            migrationBuilder.CreateIndex(
                name: "IX_OPs_PecaId",
                table: "OPs",
                column: "PecaId");

            migrationBuilder.CreateIndex(
                name: "IX_PCPs_FuncionarioId",
                table: "PCPs",
                column: "FuncionarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PecaOperacoes_OperacaoId",
                table: "PecaOperacoes",
                column: "OperacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Subprodutos_PecaSubId",
                table: "Subprodutos",
                column: "PecaSubId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Apontamentos");

            migrationBuilder.DropTable(
                name: "PecaOperacoes");

            migrationBuilder.DropTable(
                name: "Subprodutos");

            migrationBuilder.DropTable(
                name: "OPs");

            migrationBuilder.DropTable(
                name: "Operadores");

            migrationBuilder.DropTable(
                name: "Operacoes");

            migrationBuilder.DropTable(
                name: "PCPs");

            migrationBuilder.DropTable(
                name: "Pecas");

            migrationBuilder.DropTable(
                name: "TipoOperacoes");

            migrationBuilder.DropTable(
                name: "Funcionario");
        }
    }
}
