using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole
            {
                Id = "de709651-d7b2-48f0-b420-c39ff16fec07",
                ConcurrencyStamp = "2bcd98dd-7d24-45b9-ba4e-5672cab298b0",
                Name = "Manager",
                NormalizedName = "MANAGER"
            },
            new IdentityRole
            {
                Id = "95da4f40-f5b2-49f9-bdbb-8111099c28e5",
                ConcurrencyStamp = "68231484-0f44-469d-b23a-872af8364f3d",
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            }
        );
    }
}