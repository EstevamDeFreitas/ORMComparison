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
    internal class CursoAlunoMapping : IEntityTypeConfiguration<CursoAluno>
    {
        public void Configure(EntityTypeBuilder<CursoAluno> builder)
        {
            builder.ToTable("CursoAluno");

            builder.HasKey(x => new { x.AlunoId, x.CursoId });

            //builder.Property(x => x.AlunoId)
            //    .HasColumnName("AlunoId")
            //    .IsRequired();

            //builder.Property(x => x.CursoId)
            //    .HasColumnName("CursoId")
            //    .IsRequired();

            builder.Property(x => x.Nota)
                .HasColumnName("Nota")
                .IsRequired();
        }
    }
}