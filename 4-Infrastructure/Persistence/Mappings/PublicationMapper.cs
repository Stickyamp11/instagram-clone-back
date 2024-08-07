using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Mappings
{
    public class PublicationConfiguration : IEntityTypeConfiguration<Publication>
    {
        public void Configure(EntityTypeBuilder<Publication> builder) 
        {
            builder.ToTable("PUBLICATIONS");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasConversion(publicationId => publicationId.Value,
                               value => new PublicationId(value))
                               .ValueGeneratedOnAdd();

            builder.Property(p => p.Title);
            builder.Property(p => p.Description);
            builder.Property(p => p.userId)
                .HasConversion(userId => userId.Value,
                               value => new UserId(value)); ;

            builder.HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.userId)
                .IsRequired();
        }
    }
}
