using System.Globalization;
using KanbanBoard.Database;
using KanbanBoard.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Service.Card;

public class CardService
{
    private readonly DatabaseContext _databaseContext;

    public CardService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<GetAllCardsResult> GetAllCards(GetAllCardsModel model)
    {
        var result = new GetAllCardsResult();

        try
        {
            var query = _databaseContext.Cards
                .Include(c => c.Board)
                .Include(c => c.History)
                .AsQueryable();

            // created date from
            if (model.DateStart != null)
            {
                var dateTime = DateTime.ParseExact(model.DateStart, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                query = query.Where(m => m.CreatedAt >= dateTime);
            }

            // created date to
            if (!string.IsNullOrEmpty(model.DateEnd))
            {
                var dateTime = DateTime.ParseExact(model.DateEnd, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None)
                    .Date
                    .AddDays(1)
                    .AddMilliseconds(-1);

                query = query.Where(m => m.CreatedAt <= dateTime);
            }
            
            // status
            if (model.Status != null)
                query = query.Where(c => c.ActiveStatus == model.Status);

            query = query.OrderByDescending(b => b.Id);

            if (model.Skip.HasValue)
                query = query.Skip(Math.Max(0, model.Skip.Value));

            if (model.Limit.HasValue)
                query = query.Take(Math.Max(1, model.Limit.Value));

            result.Success = true;
            result.Cards = await query.ToArrayAsync();
        }
        catch (Exception e)
        {
            result.Error = e.Message;
        }

        return result;
    }

    public async Task<GetAllCardsCountResult> GetAllCardsCount(GetAllCardsCountModel model)
    {
        var result = new GetAllCardsCountResult();

        try
        {
            var query = _databaseContext.Cards
                .Include(c => c.Board)
                .Include(c => c.History)
                .AsQueryable();

            // created date from
            if (model.DateStart != null)
            {
                var dateTime = DateTime.ParseExact(model.DateStart, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                query = query.Where(m => m.CreatedAt >= dateTime);
            }

            // created date to
            if (!string.IsNullOrEmpty(model.DateEnd))
            {
                var dateTime = DateTime.ParseExact(model.DateEnd, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None)
                    .Date
                    .AddDays(1)
                    .AddMilliseconds(-1);

                query = query.Where(m => m.CreatedAt <= dateTime);
            }
            
            // status
            if (model.Status != null)
                query = query.Where(c => c.ActiveStatus == model.Status);

            query = query.OrderByDescending(b => b.Id);
            
            result.Success = true;
            result.Count = await query.CountAsync();
        }
        catch (Exception e)
        {
            result.Error = e.Message;
        }

        return result;
    }

    public async Task<CardEntity?> GetCardById(int id)
    {
        return await _databaseContext.Cards.Include(c => c.Board).Include(c => c.History).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<RegisterCardResult> RegisterCard(RegisterCardModel model)
    {
        var result = new RegisterCardResult();

        try
        {
            // validate board
            var boardEntity = await _databaseContext.Boards.FirstOrDefaultAsync(b => b.Id == model.BoardId);
            if (boardEntity == null)
            {
                result.BoardNotFound = true;
                return result;
            }
            
            // create card
            var cardEntity = new CardEntity()
            {
                CreatedAt = DateTime.Now,
                Title = model.Title,
                Description = model.Description,
                ActiveStatus = model.ActiveStatus,
                Board = boardEntity
            };

            _databaseContext.Set<CardEntity>().Add(cardEntity);
            _databaseContext.Cards.Add(cardEntity);
            
            // bind history
            var cardHistoryEntity = new CardHistoryEntity()
            {
                CreatedAt = DateTime.Now,
                Card = cardEntity,
                Status = cardEntity.ActiveStatus,
            };

            _databaseContext.Set<CardHistoryEntity>().Add(cardHistoryEntity);
            _databaseContext.CardHistories.Add(cardHistoryEntity);

            await _databaseContext.SaveChangesAsync();

            result.Success = true;
            result.CardEntity = cardEntity;
        }
        catch (Exception e)
        {
            result.Error = e.Message;
        }

        return result;
    }

    public async Task<UpdateCardResult> UpdateCard(int id, UpdateCardModel model)
    {
        var result = new UpdateCardResult();

        try
        {
            var existingCard = await _databaseContext.Cards.FirstOrDefaultAsync(c => c.Id == id);
            if (existingCard == null)
            {
                result.CardExists = false;
                return result;
            }

            existingCard.Title = model.Title;
            existingCard.Description = model.Description;
            existingCard.ActiveStatus = model.ActiveStatus;
            existingCard.BoardId = model.BoardId;

            _databaseContext.Set<CardEntity>().Update(existingCard);
            await _databaseContext.SaveChangesAsync();

            result.Success = true;
            result.CardEntity = existingCard;
        }
        catch (Exception e)
        {
            result.Error = e.Message;
        }

        return result;
    }

    public async Task<DeleteCardResult> DeleteCard(int id)
    {
        var result = new DeleteCardResult();

        try
        {
            var cardEntity = await _databaseContext.Cards.FirstOrDefaultAsync(c => c.Id == id);
            if (cardEntity == null)
            {
                result.CardExists = false;
                return result;
            }

            _databaseContext.Cards.Remove(cardEntity);
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
