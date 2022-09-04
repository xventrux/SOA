using API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.DataAccess.Configurations
{
    public class MyFileConfiguration : IEntityTypeConfiguration<MyFile>
    {
        public void Configure(EntityTypeBuilder<MyFile> builder)
        {
            builder.ToTable(nameof(MyFile));

            builder.HasKey(mf => mf.Id);
            builder.Property(mf => mf.Id).ValueGeneratedOnAdd();

        }
    }
}
