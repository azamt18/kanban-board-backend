using System.Text.Json.Serialization;
using KanbanBoard.Core.Enums;
using KanbanBoard.Database.Entities;

namespace KanbanBoard.WebApi.ResponseContracts;

public class CardHistoryContract
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("cardId")]
    public int CardId { get; set; }

    [JsonPropertyName("createdOn")]
    public DateTime CreatedOn { get; set; }

    [JsonPropertyName("type")]
    public CardHistoryType Type { get; set; }

    [JsonPropertyName("movedSourceListId")]
    public int? MovedSourceListId { get; set; }

    [JsonPropertyName("movedTargetListId")]
    public int? MovedTargetListId { get; set; }
    

    public static CardHistoryContract ConvertToContract(CardHistoryEntity cardHistoryEntity)
    {
        return new CardHistoryContract()
        {
            Id = cardHistoryEntity.Id,
            CardId = cardHistoryEntity.CardId,
            CreatedOn = cardHistoryEntity.CreatedOn,
            Type = cardHistoryEntity.Type,
            MovedSourceListId = cardHistoryEntity.MovedSourceListId,
            MovedTargetListId = cardHistoryEntity.MovedTargetListId
        };
    }
}