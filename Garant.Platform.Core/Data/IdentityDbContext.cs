using Garant.Platform.Models.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Garant.Platform.Core.Data
{
    public sealed class IdentityDbContext : IdentityDbContext<UserEntity>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }
    }
}
