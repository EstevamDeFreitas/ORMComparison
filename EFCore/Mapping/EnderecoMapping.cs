using Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.Mapping
{
    internal class EnderecoMapping : IEntityTypeConfiguration<Endereco>
    {
        public void Configure(EntityTypeBuilder<Endereco> builder)
        {
            builder.ToTable("Endereco");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .IsRequired();

            builder.Property(x => x.Pais)
                .HasColumnName("Pais")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Estado)
                .HasColumnName("Estado")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Cidade)
                .HasColumnName("Cidade")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Rua)
                .HasColumnName("Rua")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Numero)
                .HasColumnName("Numero")
                .HasMaxLength(10)
                .IsRequired();
        }
    }
}
