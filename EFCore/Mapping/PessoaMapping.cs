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
    internal class PessoaMapping : IEntityTypeConfiguration<Pessoa>
    {
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            builder.ToTable("Pessoa");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.PrimeiroNome)
                .HasColumnType("VARCHAR(255)")
                .IsRequired();

            builder.Property(p => p.UltimoNome)
                .HasColumnType("VARCHAR(255)")
                .IsRequired();

            builder.Property(p => p.NumeroTelefone)
                .HasColumnType("VARCHAR(255)")
                .IsRequired();

            builder.Property(p => p.DataNascimento)
                .HasColumnType("DATE")
                .IsRequired();

            builder.Property(p => p.EnderecoId)
                .IsRequired();

            builder.HasOne(p => p.Endereco)
                .WithMany()
                .HasForeignKey(p => p.EnderecoId);

        }
    }

}