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
    internal class ProfessorMapping : IEntityTypeConfiguration<Professor>
    {
        public void Configure(EntityTypeBuilder<Professor> builder)
        {
            builder.ToTable("Professor");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                   .HasColumnName("Id")
                   .ValueGeneratedOnAdd();

            builder.Property(p => p.Especializacao)
                   .HasColumnName("Especializacao")
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(p => p.PessoaId)
                   .HasColumnName("PessoaId")
                   .IsRequired();

            builder.HasOne(p => p.Pessoa)
                   .WithMany()
                   .HasForeignKey(p => p.PessoaId);
        }
    }
}