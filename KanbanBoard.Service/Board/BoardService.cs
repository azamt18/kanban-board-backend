using System.Linq.Expressions;
using KanbanBoard.Database;
using KanbanBoard.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Service.Board;

public interface IBoardService
{
    // Task<IEnumerable<BoardEntity?>> GetAllBoards();
    // BoardEntity? GetBoardById(int id);
    // Task<BoardEntity?> GetOne(Expression<Func<BoardEntity?, bool>> expression, CancellationToken cancellationToken = default);
    // void CreateBoard(BoardEntity? boardEntity);
    // Task UpdateBoard(BoardEntity boardEntity);
    // void DeleteBoard(int id);
}

public class BoardService : IBoardService
{
    private readonly DatabaseContext _databaseContext;

    public BoardService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<IEnumerable<BoardEntity?>> GetAllBoards()
    {
        return await _databaseContext.Boards.ToArrayAsync();
    }

    public BoardEntity? GetBoardById(int id)
    {
        return _databaseContext.Boards.FirstOrDefault(b => b.Id == id);
    }

    public async Task<BoardEntity?> GetOne(Expression<Func<BoardEntity?, bool>> expression, CancellationToken cancellationToken = default)
    {
        return await _databaseContext.Boards.FirstOrDefaultAsync(expression, cancellationToken: cancellationToken);
    }
    
    public async Task<RegisterBoardResult> RegisterBoard(RegisterBoardModel model)
    {
        var result = new RegisterBoardResult();

        try
        {
            var boardEntity = new BoardEntity()
            {
                Name = model.Name,
            };
        
            _databaseContext.Set<BoardEntity>().Add(boardEntity);
            _databaseContext.Boards.Add(boardEntity);
            await _databaseContext.SaveChangesAsync();

            result.Success = true;
            result.BoardEntity = boardEntity;
        }
        catch (Exception e)
        {
            result.Error = e.Message;
        }

        return result;
    }

    public async Task<UpdateBoardResult> UpdateBoard(int id, UpdateBoardModel model)
    {
        var result = new UpdateBoardResult();

        try
        {
            var existingBoard = await _databaseContext.Boards.FirstOrDefaultAsync(b => b.Id == id);
            if (existingBoard == null)
            {
                result.BoardExists = false;
                return result;
            }

            existingBoard.Name = model.Name;

            _databaseContext.Set<BoardEntity>().Update(existingBoard);
            await _databaseContext.SaveChangesAsync();

            result.Success = true;
            result.BoardEntity = existingBoard;
        }
        catch (Exception e)
        {
            result.Error = e.Message;
        }

        return result;
    }

    public async Task<DeleteBoardResult> DeleteBoard(int id)
    {
        var result = new DeleteBoardResult();

        try
        {
            var boardEntity = _databaseContext.Boards.FirstOrDefault(b => b.Id == id);
            if (boardEntity == null)
            {
                result.BoardExists = false;
                return result;
            }
            
            // We can also use some flag in order to track a deletion, e.g.: bool IsDeleted, DateTime? DeletedOn

            _databaseContext.Boards.Remove(boardEntity); 
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