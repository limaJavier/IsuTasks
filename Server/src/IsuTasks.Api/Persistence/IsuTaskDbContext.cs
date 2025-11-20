using IsuTasks.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IsuTasks.Api.Persistence;

public class IsuTasksDbContext(DbContextOptions<IsuTasksDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<IsuTask> Tasks { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var userEntity = modelBuilder.Entity<User>();
        userEntity.Property(user => user.Email).HasMaxLength(100);
        userEntity.Property(user => user.Password).HasMaxLength(100);
        userEntity.HasIndex(user => user.Email).IsUnique();

        var taskIsuEntity = modelBuilder.Entity<IsuTask>();
        taskIsuEntity.Property(task => task.Title).HasMaxLength(200);
        taskIsuEntity.Property(task => task.Description).HasMaxLength(500);
    }
}
