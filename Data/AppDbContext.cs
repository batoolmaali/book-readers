using BookReaders.Areas.AccountUser.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BookReaders.Areas.AccountUser.ViewModels;
using BookReaders.Models;
using System.Reflection.Emit;
using BookReaders.Areas.Dashboard.Models;
using BookReaders.ViewModels;
using BookAPI.ViewModels;

namespace BookReaders.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Borrow> Borrows { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "sec"); 
            builder.Entity<IdentityRole>().ToTable("Roles", "sec");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "sec"); builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "sec"); builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "sec");
            builder.Entity<ApplicationUser>().ToTable("Users", "sec");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserToken", "sec");


           builder.Entity<Favorite>()
         .HasOne(f => f.User)
         .WithMany(u => u.Favorites)
         .HasForeignKey(f => f.UserId);

           builder.Entity<Favorite>()
                .HasOne(f => f.Book)
                .WithMany(b => b.Favorites)
                .HasForeignKey(f => f.BookId);


            // Relationship Borrow 
            builder.Entity<Borrow>()
                .HasOne(b => b.User)
                .WithMany(u => u.Borrows)
                .HasForeignKey(b => b.UserId);

            builder.Entity<Borrow>()
                .HasOne(b => b.Book)
                .WithMany(u => u.Borrows)
                .HasForeignKey(b => b.BookId);



            // Relationship  for Review 
            builder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId);

            builder.Entity<Review>()
                .HasOne(r => r.Book)
                .WithMany(b => b.Reviews)
                .HasForeignKey(r => r.BookId);

            builder.Entity<Answer>()
      .HasOne(a => a.User)
      .WithMany(u => u.Answers)
      .HasForeignKey(a => a.UserId)
      .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    
       
   
      
     
    }
}