using System.Globalization;
using KanbanBoard.Core.Enums;
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
                .Include(c => c.ActiveList)
                .Include(c => c.CardHistories)
                .AsQueryable();

            // created date from
            if (model.DateStart != null)
            {
                var dateTime = DateTime.ParseExact(model.DateStart, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None);
                query = query.Where(m => m.CreatedOn >= dateTime);
            }

            // created date to
            if (!string.IsNullOrEmpty(model.DateEnd))
            {
                var dateTime = DateTime.ParseExact(model.DateEnd, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                        DateTimeStyles.None)
                    .Date
                    .AddDays(1)
                    .AddMilliseconds(-1);

                query = query.Where(m => m.CreatedOn <= dateTime);
            }

            // status
            if (model.CardPriority != null)
                query = query.Where(c => c.Priority == model.CardPriority);

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
                .Include(c => c.ActiveList)
                .Include(c => c.CardHistories)
                .AsQueryable();

            // created date from
            if (model.DateStart != null)
            {
                var dateTime = DateTime.ParseExact(model.DateStart, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None);
                query = query.Where(m => m.CreatedOn >= dateTime);
            }

            // created date to
            if (!string.IsNullOrEmpty(model.DateEnd))
            {
                var dateTime = DateTime.ParseExact(model.DateEnd, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                        DateTimeStyles.None)
                    .Date
                    .AddDays(1)
                    .AddMilliseconds(-1);

                query = query.Where(m => m.CreatedOn <= dateTime);
            }

            // status
            if (model.CardPriority != null)
                query = query.Where(c => c.Priority == model.CardPriority);

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
        return await _databaseContext.Cards
            .Include(c => c.ActiveList)
            .Include(c => c.CardHistories)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<RegisterCardResult> RegisterCard(RegisterCardModel model)
    {
        var result = new RegisterCardResult();

        try
        {
            // validate board
            var listEntity = await _databaseContext.Lists.FirstOrDefaultAsync(b => b.Id == model.ListId);
            if (listEntity == null)
            {
                result.ListNotFound = true;
                return result;
            }

            // create card
            var cardEntity = new CardEntity()
            {
                CreatedOn = DateTime.Now,
                Title = model.Title,
                Description = model.Description,
                Priority = model.CardPriority,
                ActiveList = listEntity,
            };

            _databaseContext.Set<CardEntity>().Add(cardEntity);
            _databaseContext.Cards.Add(cardEntity);

            // record history
            var cardHistoryEntity = new CardHistoryEntity()
            {
                CreatedOn = DateTime.Now,
                Type = CardHistoryType.Created,
                Card = cardEntity
            };

            _databaseContext.Set<CardHistoryEntity>().Add(cardHistoryEntity);
            _databaseContext.CardHistories.Add(cardHistoryEntity);

            // bind history to card
            //todo check without this part
            cardEntity.CardHistories.Add(cardHistoryEntity);
            _databaseContext.Set<CardEntity>().Update(cardEntity);

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

    public async Task<UpdateCardResult> UpdateTitle(int id, string title)
    {
        var result = new UpdateCardResult();

        try
        {
            var existingCard = await _databaseContext.Cards.FirstOrDefaultAsync(c => c.Id == id);
            if (existingCard == null)
            {
                result.CardNotExists = true;
                return result;
            }

            existingCard.Title = title;
            existingCard.UpdatedOn = DateTime.Now;
            _databaseContext.Set<CardEntity>().Update(existingCard);

            // record history
            var cardHistoryEntity = new CardHistoryEntity()
            {
                CreatedOn = DateTime.Now,
                Type = CardHistoryType.UpdatedTitle,
                Card = existingCard
            };

            existingCard.CardHistories.Add(cardHistoryEntity);
            _databaseContext.CardHistories.Add(cardHistoryEntity);
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

    public async Task<UpdateCardResult> UpdateDescription(int id, string description)
    {
        var result = new UpdateCardResult();

        try
        {
            var existingCard = await _databaseContext.Cards.FirstOrDefaultAsync(c => c.Id == id);
            if (existingCard == null)
            {
                result.CardNotExists = true;
                return result;
            }

            existingCard.Description = description;
            existingCard.UpdatedOn = DateTime.Now;
            _databaseContext.Set<CardEntity>().Update(existingCard);

            // record history
            var cardHistoryEntity = new CardHistoryEntity()
            {
                CreatedOn = DateTime.Now,
                Type = CardHistoryType.UpdatedDescription,
                Card = existingCard
            };

            existingCard.CardHistories.Add(cardHistoryEntity);
            _databaseContext.CardHistories.Add(cardHistoryEntity);
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

    public async Task<UpdateCardResult> UpdatePriority(int id, CardPriority priority)
    {
        var result = new UpdateCardResult();

        try
        {
            var existingCard = await _databaseContext.Cards.FirstOrDefaultAsync(c => c.Id == id);
            if (existingCard == null)
            {
                result.CardNotExists = true;
                return result;
            }

            existingCard.Priority = priority;
            existingCard.UpdatedOn = DateTime.Now;
            _databaseContext.Set<CardEntity>().Update(existingCard);

            // record history
            var cardHistoryEntity = new CardHistoryEntity()
            {
                CreatedOn = DateTime.Now,
                Type = CardHistoryType.UpdatedDescription,
                Card = existingCard
            };

            existingCard.CardHistories.Add(cardHistoryEntity);
            _databaseContext.CardHistories.Add(cardHistoryEntity);
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

    public async Task<MoveCardToListResult> MoveToList(int id, int sourceListId, int targetListId)
    {
        var result = new MoveCardToListResult();

        try
        {
            var existingCard = await _databaseContext.Cards.FirstOrDefaultAsync(c => c.Id == id);
            if (existingCard == null)
            {
                result.CardNotExists = true;
                return result;
            }

            var sourceList = await _databaseContext.Lists.FirstOrDefaultAsync(l => l.Id == sourceListId);
            if (sourceList == null)
            {
                result.SourceListNotExists = true;
                return result;
            }

            var targetList = await _databaseContext.Lists.FirstOrDefaultAsync(l => l.Id == targetListId);
            if (targetList == null)
            {
                result.TargetListNotExists = true;
                return result;
            }

            existingCard.ActiveList = targetList;
            existingCard.UpdatedOn = DateTime.Now;
            _databaseContext.Set<CardEntity>().Update(existingCard);

            // record history
            var cardHistoryEntity = new CardHistoryEntity()
            {
                CreatedOn = DateTime.Now,
                Type = CardHistoryType.MovedToList,
                Card = existingCard,
                MovedSourceListId = sourceListId,
                MovedTargetListId = targetListId
            };

            existingCard.CardHistories.Add(cardHistoryEntity);
            _databaseContext.CardHistories.Add(cardHistoryEntity);
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
            var existingCard = await _databaseContext.Cards.FirstOrDefaultAsync(c => c.Id == id);
            if (existingCard == null)
            {
                result.CardNotExists = true;
                return result;
            }

            existingCard.IsDeleted = true;
            existingCard.DeletedOn = DateTime.Now;
            _databaseContext.Set<CardEntity>().Update(existingCard);

            // record history
            var cardHistoryEntity = new CardHistoryEntity()
            {
                CreatedOn = DateTime.Now,
                Type = CardHistoryType.Deleted,
                Card = existingCard,
            };

            existingCard.CardHistories.Add(cardHistoryEntity);
            _databaseContext.CardHistories.Add(cardHistoryEntity);
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