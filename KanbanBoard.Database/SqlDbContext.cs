using KanbanBoard.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Database;

public class DatabaseContext : DbContext
{
    public DbSet<ListEntity> Lists { get; set; } = null!;
    public DbSet<CardEntity> Cards { get; set; } = null!;
    public DbSet<CardHistoryEntity> CardHistories { get; set; } = null!;

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
           optionsBuilder.UseSqlite("Data Source=kanban_board.db");
        }
    }
}