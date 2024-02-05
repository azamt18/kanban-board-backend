using System.Linq.Expressions;
using KanbanBoard.Core.Enums;
using KanbanBoard.Database;
using KanbanBoard.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Service.List;

public class ListService
{
    private readonly DatabaseContext _databaseContext;

    public ListService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<IEnumerable<ListEntity>> GetAllLists()
    {
        return await _databaseContext.Lists
            .Where(x => x.IsClosed == false)
            .ToArrayAsync();
    }

    public async Task<ListEntity?> GetListById(int id)
    {
        return await GetOne(x => x.Id == id);
    }

    private async Task<ListEntity?> GetOne(Expression<Func<ListEntity?, bool>> expression, CancellationToken cancellationToken = default)
    {
        return await _databaseContext.Lists
            .Include(x => x.Cards)
            .ThenInclude(x => x.CardHistories)
            .FirstOrDefaultAsync(expression, cancellationToken: cancellationToken);
    }

    public async Task<RegisterListResult> RegisterList(RegisterListModel model)
    {
        var result = new RegisterListResult();

        try
        {
            var listEntity = new ListEntity()
            {
                Title = model.Title,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
            };

            _databaseContext.Set<ListEntity>().Add(listEntity);
            _databaseContext.Lists.Add(listEntity);
            await _databaseContext.SaveChangesAsync();

            result.Success = true;
            result.ListEntity = listEntity;
        }
        catch (Exception e)
        {
            result.Error = e.Message;
        }

        return result;
    }

    public async Task<UpdateListResult> UpdateList(int id, UpdateListModel model)
    {
        var result = new UpdateListResult();

        try
        {
            var existingList = await GetOne(x => x.Id == id);
            if (existingList == null)
            {
                result.ListNotExists = true;
                return result;
            }

            existingList.Title = model.Title;
            existingList.UpdatedOn = DateTime.Now;

            _databaseContext.Set<ListEntity>().Update(existingList);
            await _databaseContext.SaveChangesAsync();

            result.Success = true;
            result.ListEntity = existingList;
        }
        catch (Exception e)
        {
            result.Error = e.Message;
        }

        return result;
    }

    public async Task<DeleteListResult> CloseList(int id)
    {
        var result = new DeleteListResult();
        var now = DateTime.Now;

        try
        {
            var listEntity = await GetOne(x => x.Id == id);
            if (listEntity == null)
            {
                result.ListNotExists = true;
                return result;
            }

            // update list itself
            listEntity.IsClosed = true;
            listEntity.ClosedOn = now;
            _databaseContext.Set<ListEntity>().Update(listEntity);

            // update list items
            foreach (var cardEntity in listEntity.Cards)
            {
                cardEntity.IsDeleted = true;
                cardEntity.DeletedOn = now;
                _databaseContext.Set<CardEntity>().Update(cardEntity);

                // record history
                var cardHistoryEntity = new CardHistoryEntity()
                {
                    CreatedOn = DateTime.Now,
                    Type = CardHistoryType.Deleted,
                    Card = cardEntity
                };

                _databaseContext.Set<CardHistoryEntity>().Add(cardHistoryEntity);
                _databaseContext.CardHistories.Add(cardHistoryEntity);
            }

            await _databaseContext.SaveChangesAsync();

            result.Success = true;
        }
        catch (Exception e)
        {
            result.Error = e.Message;
        }

        return result;
    }
}