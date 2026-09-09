namespace DevLogix.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public partial class ApplicationDbContext
{
    private void SeedRoles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole 
            { 
                Id = "b4a3c6d2-1e5f-4a8b-9c7d-2e3f5a6b8c9d", 
                Name = "Admin", 
                NormalizedName = "ADMIN",
                ConcurrencyStamp = "b4a3c6d2-1e5f-4a8b-9c7d-2e3f5a6b8c9d"
            },
            new IdentityRole 
            { 
                Id = "d5b4c7e3-2f6a-5b9c-ad8e-3f4a6b7c9d0e", 
                Name = "PM", 
                NormalizedName = "PM",
                ConcurrencyStamp = "d5b4c7e3-2f6a-5b9c-ad8e-3f4a6b7c9d0e"
            },
            new IdentityRole 
            { 
                Id = "e6c5d8f4-3a7b-6cad-be9f-4a5b7c8d0e1f", 
                Name = "Developer", 
                NormalizedName = "DEVELOPER",
                ConcurrencyStamp = "e6c5d8f4-3a7b-6cad-be9f-4a5b7c8d0e1f"
            }
        );
    }
}
