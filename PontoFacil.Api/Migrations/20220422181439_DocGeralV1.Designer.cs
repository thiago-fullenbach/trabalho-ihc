﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PontoFacil.Api.Modelo.Contexto;

#nullable disable

namespace PontoFacil.Api.Migrations
{
    [DbContext(typeof(PontoFacilContexto))]
    [Migration("20220422181439_DocGeralV1")]
    partial class DocGeralV1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("PontoFacil.Api.Modelo.Ajustes", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("datahora_criacao")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("datahora_modificacao")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("datahora_presenca_ajuste")
                        .HasColumnType("datetime(6)");

                    b.Property<bool?>("eh_entrada_ajuste")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("locais_id_ajuste")
                        .HasColumnType("int");

                    b.Property<string>("observacao")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("presencas_id")
                        .HasColumnType("int");

                    b.Property<int>("tipo_ajuste")
                        .HasColumnType("int");

                    b.Property<int>("usuarios_id_modificador")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("locais_id_ajuste");

                    b.HasIndex("presencas_id")
                        .IsUnique();

                    b.HasIndex("tipo_ajuste");

                    b.HasIndex("usuarios_id_modificador");

                    b.ToTable("Ajustes");
                });

            modelBuilder.Entity("PontoFacil.Api.Modelo.Apelidos", b =>
                {
                    b.Property<int>("usuarios_id")
                        .HasColumnType("int");

                    b.Property<int>("locais_id")
                        .HasColumnType("int");

                    b.Property<string>("apelido")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("datahora_criacao")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("datahora_modificacao")
                        .HasColumnType("datetime(6)");

                    b.HasKey("usuarios_id", "locais_id");

                    b.HasIndex("locais_id");

                    b.ToTable("Apelidos");
                });

            modelBuilder.Entity("PontoFacil.Api.Modelo.Cidades", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("nome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("ufs_id")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("ufs_id");

                    b.ToTable("Cidades");
                });

            modelBuilder.Entity("PontoFacil.Api.Modelo.Locais", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("bairro")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("cidades_id")
                        .HasColumnType("int");

                    b.Property<string>("complemento")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("datahora_criacao")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("datahora_modificacao")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("logradouro")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("numero")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("tiposLogradouro_id")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("cidades_id");

                    b.HasIndex("tiposLogradouro_id");

                    b.ToTable("Locais");
                });

            modelBuilder.Entity("PontoFacil.Api.Modelo.Presencas", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("datahora_presenca")
                        .HasColumnType("datetime(6)");

                    b.Property<bool?>("eh_entrada")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("locais_id")
                        .HasColumnType("int");

                    b.Property<int>("usuarios_id")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("locais_id");

                    b.HasIndex("usuarios_id");

                    b.ToTable("Presencas");
                });

            modelBuilder.Entity("PontoFacil.Api.Modelo.Recursos", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("datahora_criacao")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("datahora_modificacao")
                        .HasColumnType("datetime(6)");

                    b.Property<bool?>("pode_cad_administrador_acessos")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool?>("pode_cad_ajuste_demais_usuarios")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool?>("pode_cad_demais_usuarios")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool?>("pode_cad_ponto")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool?>("pode_cad_usuario")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool?>("pode_vis_ajuste")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool?>("pode_vis_ajuste_demais_usuarios")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool?>("pode_vis_demais_usuarios")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool?>("pode_vis_ponto")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool?>("pode_vis_ponto_demais_usuarios")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool?>("pode_vis_usuario")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("usuarios_id")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("usuarios_id")
                        .IsUnique();

                    b.ToTable("Recursos");
                });

            modelBuilder.Entity("PontoFacil.Api.Modelo.Sessoes", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("datahora_ultima_autenticacao")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("hexVerificacao")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("usuarios_id")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("usuarios_id");

                    b.ToTable("Sessoes");
                });

            modelBuilder.Entity("PontoFacil.Api.Modelo.TiposAjuste", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("descricao")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool?>("eh_alteracao")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool?>("eh_exclusao")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool?>("eh_inclusao")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("id");

                    b.ToTable("TiposAjuste");
                });

            modelBuilder.Entity("PontoFacil.Api.Modelo.TiposLogradouro", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("descricao")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("id");

                    b.ToTable("TiposLogradouro");
                });

            modelBuilder.Entity("PontoFacil.Api.Modelo.UFs", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("nome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("id");

                    b.ToTable("UFs");
                });

            modelBuilder.Entity("PontoFacil.Api.Modelo.Usuarios", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("cpf")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("data_nascimento")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("datahora_criacao")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("datahora_modificacao")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("datahora_modificacao_login")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("datahora_modificacao_senha")
                        .HasColumnType("datetime(6)");

                    b.Property<bool?>("eh_senha_temporaria")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("horas_diarias")
                        .HasColumnType("int");

                    b.Property<string>("login")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("nome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("senha")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("id");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("PontoFacil.Api.Modelo.Ajustes", b =>
                {
                    b.HasOne("PontoFacil.Api.Modelo.Locais", "NavegacaoLocaisAjuste")
                        .WithMany("NavegacaoMuitosAjustes")
                        .HasForeignKey("locais_id_ajuste");

                    b.HasOne("PontoFacil.Api.Modelo.Presencas", "NavegacaoPresencas")
                        .WithOne("NavegacaoAjustes")
                        .HasForeignKey("PontoFacil.Api.Modelo.Ajustes", "presencas_id");

                    b.HasOne("PontoFacil.Api.Modelo.TiposAjuste", "NavegacaoTiposAjuste")
                        .WithMany("NavegacaoMuitosAjustes")
                        .HasForeignKey("tipo_ajuste")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PontoFacil.Api.Modelo.Usuarios", "NavegacaoUsuariosModificador")
                        .WithMany("NavegacaoMuitosAjustes")
                        .HasForeignKey("usuarios_id_modificador")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NavegacaoLocaisAjuste");

                    b.Navigation("NavegacaoPresencas");

                    b.Navigation("NavegacaoTiposAjuste");

                    b.Navigation("NavegacaoUsuariosModificador");
                });

            modelBuilder.Entity("PontoFacil.Api.Modelo.Apelidos", b =>
                {
                    b.HasOne("PontoFacil.Api.Modelo.Locais", "NavegacaoLocais")
                        .WithMany("NavegacaoMuitosApelidos")
                        .HasForeignKey("locais_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PontoFacil.Api.Modelo.Usuarios", "NavegacaoUsuarios")
                        .WithMany("NavegacaoMuitosApelidos")
                        .HasForeignKey("usuarios_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NavegacaoLocais");

                    b.Navigation("NavegacaoUsuarios");
                });

            modelBuilder.Entity("PontoFacil.Api.Modelo.Cidades", b =>
                {
                    b.HasOne("PontoFacil.Api.Modelo.UFs", "NavegacaoUFs")
                        .WithMany("NavegacaoMuitosCidades")
                        .HasForeignKey("ufs_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NavegacaoUFs");
                });

            modelBuilder.Entity("PontoFacil.Api.Modelo.Locais", b =>
                {
                    b.HasOne("PontoFacil.Api.Modelo.Cidades", "NavegacaoCidades")
                        .WithMany("NavegacaoMuitosLocais")
                        .HasForeignKey("cidades_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PontoFacil.Api.Modelo.TiposLogradouro", "NavegacaoTiposLogradouro")
                        .WithMany("NavegacaoMuitosLocais")
                        .HasForeignKey("tiposLogradouro_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NavegacaoCidades");

                    b.Navigation("NavegacaoTiposLogradouro");
                });

            modelBuilder.Entity("PontoFacil.Api.Modelo.Presencas", b =>
                {
                    b.HasOne("PontoFacil.Api.Modelo.Locais", "NavegacaoLocais")
                        .WithMany("NavegacaoMuitosPresencas")
                        .HasForeignKey("locais_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PontoFacil.Api.Modelo.Usuarios", "NavegacaoUsuarios")
                        .WithMany("NavegacaoMuitosPresencas")
                        .HasForeignKey("usuarios_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NavegacaoLocais");

                    b.Navigation("NavegacaoUsuarios");
                });

            modelBuilder.Entity("PontoFacil.Api.Modelo.Recursos", b =>
                {
                    b.HasOne("PontoFacil.Api.Modelo.Usuarios", "NavegacaoUsuarios")
                        .WithOne("NavegacaoRecursos")
                        .HasForeignKey("PontoFacil.Api.Modelo.Recursos", "usuarios_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NavegacaoUsuarios");
                });

            modelBuilder.Entity("PontoFacil.Api.Modelo.Sessoes", b =>
                {
                    b.HasOne("PontoFacil.Api.Modelo.Usuarios", "NavegacaoUsuarios")
                        .WithMany("NavegacaoMuitosSessoes")
                        .HasForeignKey("usuarios_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NavegacaoUsuarios");
                });

            modelBuilder.Entity("PontoFacil.Api.Modelo.Cidades", b =>
                {
                    b.Navigation("NavegacaoMuitosLocais");
                });

            modelBuilder.Entity("PontoFacil.Api.Modelo.Locais", b =>
                {
                    b.Navigation("NavegacaoMuitosAjustes");

                    b.Navigation("NavegacaoMuitosApelidos");

                    b.Navigation("NavegacaoMuitosPresencas");
                });

            modelBuilder.Entity("PontoFacil.Api.Modelo.Presencas", b =>
                {
                    b.Navigation("NavegacaoAjustes");
                });

            modelBuilder.Entity("PontoFacil.Api.Modelo.TiposAjuste", b =>
                {
                    b.Navigation("NavegacaoMuitosAjustes");
                });

            modelBuilder.Entity("PontoFacil.Api.Modelo.TiposLogradouro", b =>
                {
                    b.Navigation("NavegacaoMuitosLocais");
                });

            modelBuilder.Entity("PontoFacil.Api.Modelo.UFs", b =>
                {
                    b.Navigation("NavegacaoMuitosCidades");
                });

            modelBuilder.Entity("PontoFacil.Api.Modelo.Usuarios", b =>
                {
                    b.Navigation("NavegacaoMuitosAjustes");

                    b.Navigation("NavegacaoMuitosApelidos");

                    b.Navigation("NavegacaoMuitosPresencas");

                    b.Navigation("NavegacaoMuitosSessoes");

                    b.Navigation("NavegacaoRecursos");
                });
#pragma warning restore 612, 618
        }
    }
}
