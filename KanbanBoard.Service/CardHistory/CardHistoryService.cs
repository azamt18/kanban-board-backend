using KanbanBoard.Database;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Service.CardHistory;

public class CardHistoryService
{
    private readonly DatabaseContext _databaseContext;

    public CardHistoryService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<CardHistoryResult> GetCardHistory(int cardId)
    {
        var result = new CardHistoryResult();

        try
        {
            var existingCard = await _databaseContext.Cards.FirstOrDefaultAsync(c => c.Id == cardId);
            if (existingCard == null)
            {
                result.CardNotExists = true;
                return result;
            }

            result.Success = true;
            result.CardHistories = existingCard.CardHistories;
        }
        catch (Exception e)
        {
            result.Error = e.Message;
        }

        return result;
    }
}