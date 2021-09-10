﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TecWi_Web.Data.Context;

namespace TecWi_Web.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TecWi_Web.Domain.Entities.Cliente", b =>
                {
                    b.Property<int>("Cdclifor")
                        .HasColumnType("int");

                    b.Property<string>("Cidade")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DDD")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fantasia")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fone1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fone2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("IdUsuario")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Inscrifed")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Razao")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Cdclifor");

                    b.HasIndex("IdUsuario");

                    b.ToTable("Cliente");
                });

            modelBuilder.Entity("TecWi_Web.Domain.Entities.ContatoCobranca", b =>
                {
                    b.Property<int>("IdContato")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Anotacao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DtAgenda")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DtContato")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdCliente")
                        .HasColumnType("int");

                    b.Property<Guid>("IdUsuario")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("TipoContato")
                        .HasColumnType("int");

                    b.HasKey("IdContato");

                    b.ToTable("ContatoCobranca");
                });

            modelBuilder.Entity("TecWi_Web.Domain.Entities.ContatoCobrancaLancamento", b =>
                {
                    b.Property<int>("IdContato")
                        .HasColumnType("int");

                    b.Property<string>("Numlancto")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Sq")
                        .HasColumnType("int");

                    b.Property<string>("Cdfilial")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("IdContato", "Numlancto", "Sq", "Cdfilial");

                    b.HasIndex("Numlancto", "Sq", "Cdfilial");

                    b.ToTable("ContatoCobrancaLancamento");
                });

            modelBuilder.Entity("TecWi_Web.Domain.Entities.PagarReceber", b =>
                {
                    b.Property<string>("Numlancto")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Sq")
                        .HasColumnType("int");

                    b.Property<string>("Cdfilial")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Cdclifor")
                        .HasColumnType("int");

                    b.Property<DateTime>("Dtemissao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Dtpagto")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Dtvencto")
                        .HasColumnType("datetime2");

                    b.Property<string>("NotasFiscais")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SeqID")
                        .HasColumnType("int");

                    b.Property<bool>("Stcobranca")
                        .HasColumnType("bit");

                    b.Property<decimal>("Valorr")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Numlancto", "Sq", "Cdfilial");

                    b.HasIndex("Cdclifor");

                    b.ToTable("PagarReceber");
                });

            modelBuilder.Entity("TecWi_Web.Domain.Entities.Usuario", b =>
                {
                    b.Property<Guid>("IdUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("SenhaHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("SenhaSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("IdUsuario");

                    b.ToTable("Usuario");
                });

            modelBuilder.Entity("TecWi_Web.Domain.Entities.UsuarioAplicacao", b =>
                {
                    b.Property<Guid>("IdUsuario")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("IdAplicacao")
                        .HasColumnType("int");

                    b.Property<int>("IdPerfil")
                        .HasColumnType("int");

                    b.HasKey("IdUsuario", "IdAplicacao", "IdPerfil");

                    b.ToTable("UsuarioAplicacao");
                });

            modelBuilder.Entity("TecWi_Web.Domain.Entities.Cliente", b =>
                {
                    b.HasOne("TecWi_Web.Domain.Entities.Usuario", "Usuario")
                        .WithMany("Cliente")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("TecWi_Web.Domain.Entities.ContatoCobrancaLancamento", b =>
                {
                    b.HasOne("TecWi_Web.Domain.Entities.ContatoCobranca", "ContatoCobranca")
                        .WithMany("ContatoCobrancaLancamento")
                        .HasForeignKey("IdContato")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("TecWi_Web.Domain.Entities.PagarReceber", "PagarReceber")
                        .WithMany("ContatoCobrancaLancamento")
                        .HasForeignKey("Numlancto", "Sq", "Cdfilial")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("ContatoCobranca");

                    b.Navigation("PagarReceber");
                });

            modelBuilder.Entity("TecWi_Web.Domain.Entities.PagarReceber", b =>
                {
                    b.HasOne("TecWi_Web.Domain.Entities.Cliente", "Cliente")
                        .WithMany("PagarReceber")
                        .HasForeignKey("Cdclifor")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("TecWi_Web.Domain.Entities.UsuarioAplicacao", b =>
                {
                    b.HasOne("TecWi_Web.Domain.Entities.Usuario", "Usuario")
                        .WithMany("UsuarioAplicacao")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("TecWi_Web.Domain.Entities.Cliente", b =>
                {
                    b.Navigation("PagarReceber");
                });

            modelBuilder.Entity("TecWi_Web.Domain.Entities.ContatoCobranca", b =>
                {
                    b.Navigation("ContatoCobrancaLancamento");
                });

            modelBuilder.Entity("TecWi_Web.Domain.Entities.PagarReceber", b =>
                {
                    b.Navigation("ContatoCobrancaLancamento");
                });

            modelBuilder.Entity("TecWi_Web.Domain.Entities.Usuario", b =>
                {
                    b.Navigation("Cliente");

                    b.Navigation("UsuarioAplicacao");
                });
#pragma warning restore 612, 618
        }
    }
}
