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
    internal class EstudanteMapping : IEntityTypeConfiguration<Estudante>
    {
        public void Configure(EntityTypeBuilder<Estudante> builder)
        {
            builder.ToTable("Estudante");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                   .HasColumnName("Id")
                   .ValueGeneratedOnAdd();

            builder.Property(e => e.Descricao)
                   .HasColumnName("Descricao")
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(e => e.PessoaId)
                   .HasColumnName("PessoaId")
                   .IsRequired();

            builder.HasOne(e => e.Pessoa)
                   .WithMany()
                   .HasForeignKey(e => e.PessoaId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}