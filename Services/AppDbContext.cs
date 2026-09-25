using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Models;

namespace TaskManager.Api.Services;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Convert the enum Ints to Strings
        modelBuilder.Entity<TaskItem>()
            .Property(t => t.Priority)
            .HasConversion<string>();
        
        // 2. Seed the entity data
        modelBuilder.Entity<TaskItem>()
            .HasData(
                new TaskItem
                {
                    Id = 1,
                    Title = "Set up development environment",
                    Description = "Install .NET SDK, VS Code, and configure local database tools.",
                    isCompleted = true,
                    Priority = TaskPriority.HIGH,
                    DueDate = new DateTime(2026, 10, 1, 0, 0, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2026, 9, 20, 0, 0, 0, DateTimeKind.Utc)
                },
                new TaskItem
                {
                    Id = 2,
                    Title = "Design database schema",
                    Description = "Draft entities for tasks, user assignments, and categories.",
                    isCompleted = true,
                    Priority = TaskPriority.HIGH,
                    DueDate = new DateTime(2026, 10, 5, 0, 0, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2026, 9, 21, 0, 0, 0, DateTimeKind.Utc)
                },
                new TaskItem
                {
                    Id = 3,
                    Title = "Implement CRUD API endpoints",
                    Description = "Build controllers and repository layer for task management.",
                    isCompleted = false,
                    Priority = TaskPriority.MEDIUM,
                    DueDate = new DateTime(2026, 10, 10, 0, 0, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2026, 9, 22, 0, 0, 0, DateTimeKind.Utc)
                },
                new TaskItem
                {
                    Id = 4,
                    Title = "Write unit and integration tests",
                    Description = "Cover the service logic and API routes with xUnit test suites.",
                    isCompleted = false,
                    Priority = TaskPriority.MEDIUM,
                    DueDate = new DateTime(2026, 10, 15, 0, 0, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2026, 9, 23, 0, 0, 0, DateTimeKind.Utc)
                },
                new TaskItem
                {
                    Id = 5,
                    Title = "Configure CI/CD deployment pipeline",
                    Description = "Set up GitHub Actions to build, test, and package container images.",
                    isCompleted = false,
                    Priority = TaskPriority.LOW,
                    DueDate = new DateTime(2026, 10, 20, 0, 0, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2026, 9, 24, 0, 0, 0, DateTimeKind.Utc)
                }
            );
    }
}