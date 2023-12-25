using KanbanBoard.Core.Enums;
using KanbanBoard.Database;
using KanbanBoard.Database.Entities;

namespace KanbanBoard.WebApi.Seeder;

public class Seed : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public Seed(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SeedDataToDb();
        return Task.CompletedTask;
    }
    
    private void SeedDataToDb()
    {
        // We instantiate our db context
        using var scope = _serviceProvider.CreateScope();
        var databaseContext = scope.ServiceProvider.GetService<DatabaseContext>();
        if (databaseContext == null) return;
        
        SeedLists(databaseContext);
        SeedCards(databaseContext);
    }

    private void SeedLists(DatabaseContext databaseContext)
    {
        // We add some products if the database is empty
        bool isEmpty = databaseContext.Lists.Any();
        if (!isEmpty)
        {
            databaseContext.Lists.AddRange(
                new ListEntity() { Title = "Todo", CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now },
                new ListEntity() { Title = "Active", CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now },
                new ListEntity() { Title = "Complete", CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now }
            );

            databaseContext.SaveChanges();
        }

        // We check the content of our database
        var entities = databaseContext.Lists.ToArray();
        Array.ForEach(entities, p =>
        {
            Console.WriteLine("Id: " + p.Id);
            Console.WriteLine("Title: " + p.Title);
            Console.WriteLine("===============================");
        });
    }

    private void SeedCards(DatabaseContext databaseContext)
    {
        // We add some products if the database is empty
        bool isEmpty = databaseContext.Cards.Any();
        if (!isEmpty)
        {
            foreach (var listEntity in databaseContext.Lists)
            {
                var cardEntity = new CardEntity()
                {
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    Title = $"Task of {listEntity.Title}",
                    Description = "Its description",
                    Priority = CardPriority.Low,
                    ActiveListId = listEntity.Id,
                };

                databaseContext.Set<CardEntity>().Add(cardEntity);
                databaseContext.Cards.Add(cardEntity);

                // record history
                var cardHistoryEntity = new CardHistoryEntity()
                {
                    CreatedOn = DateTime.Now,
                    Type = CardHistoryType.Created,
                    Card = cardEntity
                };

                databaseContext.Set<CardHistoryEntity>().Add(cardHistoryEntity);
                databaseContext.CardHistories.Add(cardHistoryEntity);
            }

            databaseContext.SaveChanges();
        }
    }

}