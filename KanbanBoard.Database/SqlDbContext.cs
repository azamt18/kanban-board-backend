using KanbanBoard.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Database;

public class DatabaseContext : DbContext
{
    public DbSet<ListEntity> Lists { get; set; }
    public DbSet<CardEntity> Cards { get; set; }

    public DbSet<CardHistoryEntity> CardHistories { get; set; }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }
    
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseSqlite("Data Source=kanban.db");
    // }
}