using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PCPMetalurgicaInter.Models;

namespace PCPMetalurgicaInter.Dados
{
    public class BancoContext :DbContext
    {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options){ }
        public DbSet<Apontamento> Apontamentos{ get; set; }
        public DbSet<Funcionario> Funcionarios{ get; set; }
        public DbSet<Operacao> Operacoes {get; set; } 
        public DbSet<Operador> Operadores {get; set; }
        public DbSet<OrdemDeProducao> OPs {get; set; }
        public DbSet<PCP> PCPs {get; set; }
        public DbSet<Peca> Pecas {get; set; }
        public DbSet<Subproduto> Subprodutos {get; set; }
        public DbSet<PecaOperacao> PecaOperacoes {get; set; }
        public DbSet<TipoDeOperacao> TipoDeOperacoes {get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Apontamento>(t => {
                t.ToTable("Apontamentos");
                t.HasKey(t => t.Id);
                t.Property(t => t.Id).HasColumnType("int").IsRequired().ValueGeneratedOnAdd();
                t.Property(t => t.Quantidade).HasColumnType("int");
                t.Property(t => t.Inicio).HasColumnType("Datetime").IsRequired();
                t.Property(t => t.Fim).HasColumnType("Datetime");
                t.HasOne(t => t.Operacao).WithMany(t => t.Apontamentos).HasForeignKey(t => t.OperacaoId).OnDelete(DeleteBehavior.NoAction).IsRequired();
                t.HasOne(t => t.Operador).WithMany(t => t.Apontamentos).HasForeignKey(t => t.OperadorId).IsRequired();
                t.HasOne(t => t.OrdemDeProducao).WithMany(t => t.Apontamentos).HasForeignKey(t => t.OrdemDeProducaoId).OnDelete(DeleteBehavior.NoAction).IsRequired();
            });

            modelBuilder.Entity<Funcionario>(t => {
                t.ToTable("Funcionario");
                t.HasKey(t => t.Id);
                t.Property(t => t.Id).HasColumnType("int").IsRequired().ValueGeneratedOnAdd();
                t.Property(t => t.Nome).HasColumnType("varchar(128)").IsRequired();
                t.Property(t => t.Login).HasColumnType("varchar(64)").IsRequired();
                t.Property(t => t.Senha).HasColumnType("varchar(128)").IsRequired();
                t.HasOne(t => t.PCP).WithOne(t => t.Funcionario).HasForeignKey<PCP>(t => t.FuncionarioId).OnDelete(DeleteBehavior.NoAction);
                t.HasOne(t => t.Operador).WithOne(t => t.Funcionario).HasForeignKey<Operador>(t => t.FuncionarioId).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Operacao>(t => {
                t.ToTable("Operacoes");
                t.HasKey(t => t.Id);
                t.Property(t => t.Id).HasColumnType("int").IsRequired().ValueGeneratedOnAdd();
                t.Property(t => t.Descricao).HasColumnType("varchar(256)").IsRequired();
                t.HasOne(t => t.TipoDeOperacao).WithMany(t => t.Operacoes).HasForeignKey(t => t.TipoDeOperacaoId).OnDelete(DeleteBehavior.NoAction).IsRequired();
                t.HasMany(t => t.Apontamentos).WithOne(t => t.Operacao).HasForeignKey(t => t.OperacaoId).OnDelete(DeleteBehavior.NoAction);
                t.HasMany(t => t.PecaOperacaoes).WithOne(t => t.Operacao).HasForeignKey(t => t.OperacaoId).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Operador>(t => {
                t.ToTable("Operadores");
                t.HasKey(t => t.Id);
                t.Property(t => t.Id).HasColumnType("int").IsRequired().ValueGeneratedOnAdd();
                t.HasOne(t => t.Funcionario).WithOne(t => t.Operador).HasForeignKey<Operador>(t => t.FuncionarioId).OnDelete(DeleteBehavior.NoAction).IsRequired();
                t.HasMany(t => t.Apontamentos).WithOne(t => t.Operador).HasForeignKey(t => t.OperadorId);
            });

            modelBuilder.Entity<OrdemDeProducao>(t => {
                t.ToTable("OPs");
                t.HasKey(t => t.Id);
                t.Property(t => t.Id).HasColumnType("int").IsRequired().ValueGeneratedOnAdd();
                t.Property(t => t.Quantidade).HasColumnType("int").IsRequired();
                t.Property(t => t.DataEmissao).HasColumnType("Datetime").IsRequired();
                t.Property(t=> t.Cancelada).HasColumnType("bit").IsRequired();
                t.Property(t=> t.Fechada).HasColumnType("bit").IsRequired();
                t.HasOne(t => t.Peca).WithMany(t => t.Ops).HasForeignKey(t => t.PecaId).OnDelete(DeleteBehavior.NoAction).IsRequired();
                t.HasOne(t => t.PCPEmissor).WithMany(t => t.Ops).HasForeignKey(t => t.PCPEmissorId).IsRequired();
                t.HasMany(t => t.Apontamentos).WithOne(t => t.OrdemDeProducao).HasForeignKey(t => t.OrdemDeProducaoId).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<PCP>(t => {
                t.ToTable("PCPs");
                t.HasKey(t => t.Id);
                t.Property(t => t.Id).HasColumnType("int").IsRequired().ValueGeneratedOnAdd();
                t.HasOne(t => t.Funcionario).WithOne(t => t.PCP).HasForeignKey<PCP>(t => t.FuncionarioId).IsRequired().OnDelete(DeleteBehavior.NoAction);
                t.HasMany(t => t.Ops).WithOne(t => t.PCPEmissor).HasForeignKey(t => t.PCPEmissorId);
            });

            modelBuilder.Entity<Peca>(t => {
                t.ToTable("Pecas");
                t.HasKey(t => t.Id);
                t.Property(t => t.Id).HasColumnType("int").IsRequired().ValueGeneratedOnAdd();
                t.Property(t => t.Descricao).HasColumnType("varchar(256)").IsRequired();
                t.Property(t => t.Data_Cadastro).HasColumnType("Datetime").IsRequired();
                t.Property(t => t.Valor).HasColumnType("decimal(18,2)");
                t.Property(t => t.Situacao).HasColumnType("bit").IsRequired();
                t.Property(t => t.Imagem).HasColumnType("varchar(256)");
                t.Property(t => t.DadosImagem).HasColumnType("varchar(2)");
                t.Property(t => t.CodigoUniversal).HasColumnType("varchar(256)");
                t.HasMany(t => t.Ops).WithOne(t => t.Peca).HasForeignKey(t => t.PecaId);
                t.HasMany(t => t.PecaOperacoes).WithOne(t => t.Peca).HasForeignKey(t => t.PecaId);
                t.HasMany(t => t.Subprodutos).WithOne(t => t.Peca).HasForeignKey(t => t.PecaId);
            });

            modelBuilder.Entity<Subproduto>(t => {
                t.ToTable("Subprodutos");
                t.HasKey(t => new { t.PecaId, t.PecaSubId });
                t.Property(t => t.Quantidade).HasColumnType("float").IsRequired();
                t.HasOne(t => t.Peca).WithMany(t => t.Subprodutos).HasForeignKey(t => t.PecaId).OnDelete(DeleteBehavior.NoAction).IsRequired();
            });

            modelBuilder.Entity<PecaOperacao>(t => {
                t.ToTable("PecaOperacoes");
                t.HasKey(t => new { t.PecaId, t.OperacaoId });
                t.HasOne(t => t.Peca).WithMany(t => t.PecaOperacoes).HasForeignKey(t => t.PecaId).IsRequired();
                t.HasOne(t => t.Operacao).WithMany(t => t.PecaOperacaoes).HasForeignKey(t => t.OperacaoId).IsRequired();
                t.Property(t => t.etapa).HasColumnType("int").IsRequired();
            });

            modelBuilder.Entity<TipoDeOperacao>(t => {
                t.ToTable("TipoOperacoes");
                t.HasKey(t => t.Id);
                t.Property(t => t.Id).HasColumnType("int").IsRequired().ValueGeneratedOnAdd();
                t.Property(t => t.Descricao).HasColumnType("varchar(256)").IsRequired();
                t.HasMany(t => t.Operacoes).WithOne(t => t.TipoDeOperacao).HasForeignKey(t => t.TipoDeOperacaoId);
            });

        }
    }
}