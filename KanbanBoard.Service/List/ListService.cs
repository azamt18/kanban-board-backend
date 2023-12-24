using System.Linq.Expressions;
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
        return _databaseContext.Lists.Any() 
            ? await _databaseContext.Lists.ToArrayAsync()
            : Array.Empty<ListEntity>();
    }

    public ListEntity? GetListById(int id)
    {
        return _databaseContext.Lists.FirstOrDefault(b => b.Id == id);
    }

    public async Task<ListEntity?> GetOne(Expression<Func<ListEntity?, bool>> expression, CancellationToken cancellationToken = default)
    {
        return await _databaseContext.Lists.FirstOrDefaultAsync(expression, cancellationToken: cancellationToken);
    }
    
    public async Task<RegisterListResult> RegisterList(RegisterListModel model)
    {
        var result = new RegisterListResult();

        try
        {
            var listEntity = new ListEntity()
            {
                Title = model.Title,
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
            var existingList = await _databaseContext.Lists.FirstOrDefaultAsync(b => b.Id == id);
            if (existingList == null)
            {
                result.ListNotExists = true;
                return result;
            }

            existingList.Title = model.Title;

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

        try
        {
            var listEntity = _databaseContext.Lists.FirstOrDefault(b => b.Id == id);
            if (listEntity == null)
            {
                result.ListNotExists = true;
                return result;
            }

            listEntity.IsClosed = true;
            listEntity.ClosedOn = DateTime.Now;

            _databaseContext.Set<ListEntity>().Update(listEntity);
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