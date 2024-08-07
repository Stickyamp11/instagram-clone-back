using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Mappings
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("USERS");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
               .HasConversion(userId => userId.Value,
                              value => new UserId(value))
               .ValueGeneratedOnAdd();

            builder.Property(p => p.Guid)
                .HasConversion(userGuid => userGuid.Value,
                               value => new UserGuid(value));
            builder.Property(p => p.Email);
            builder.Property(p => p.Password);
            builder.Property(p => p.FullName);

            builder.HasMany(p => p.Publications)
                .WithOne(p => p.User);

        }
    }
}
