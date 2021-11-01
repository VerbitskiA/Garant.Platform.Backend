//using Garant.Platform.Models.Entities.User;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace Garant.Platform.Models.Mappings.User
//{
//    public partial class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
//    {
//        public void Configure(EntityTypeBuilder<UserEntity> entity)
//        {
//            entity.ToTable("Users", "dbo");

//            entity.HasKey(u => u.Id);

//            entity.HasIndex(u => u.NormalizedUserName).HasName("UserNameIndex").IsUnique();
//            entity.HasIndex(u => u.NormalizedEmail).HasName("EmailIndex");

//            entity.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

//            // Limit the size of columns to use efficient database types
//            entity.Property(u => u.UserName).HasMaxLength(256);
//            entity.Property(u => u.NormalizedUserName).HasMaxLength(256);
//            entity.Property(u => u.Email).HasMaxLength(256);
//            entity.Property(u => u.NormalizedEmail).HasMaxLength(256);

//            // The relationships between User and other entity types
//            // Note that these relationships are configured with no navigation properties

//            // Each User can have many UserClaims
//            //entity.HasMany<TUserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();

//            //// Each User can have many UserLogins
//            //entity.HasMany<TUserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();

//            //// Each User can have many UserTokens
//            //entity.HasMany<TUserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();

//            //// Each User can have many entries in the UserRole join table
//            //entity.HasMany<TUserRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();

//            OnConfigurePartial(entity);
//        }

//        partial void OnConfigurePartial(EntityTypeBuilder<UserEntity> entity);
//    }
//}
