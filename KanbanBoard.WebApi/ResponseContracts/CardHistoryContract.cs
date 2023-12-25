using System.Text.Json.Serialization;
using KanbanBoard.Core;
using KanbanBoard.Core.Enums;
using KanbanBoard.Database.Entities;
using Newtonsoft.Json;

namespace KanbanBoard.WebApi.ResponseContracts;

public class CardHistoryContract
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("cardId")]
    public int CardId { get; set; }

    [JsonProperty("createdOn")]
    public string CreatedOn { get; set; }

    [JsonProperty("type")]
    public CardHistoryType CardHistoryType { get; set; }

    [JsonProperty("movedSourceListId")]
    public int? MovedSourceListId { get; set; }

    [JsonProperty("movedTargetListId")]
    public int? MovedTargetListId { get; set; }
    

    public static CardHistoryContract ConvertToContract(CardHistoryEntity cardHistoryEntity)
    {
        return new CardHistoryContract()
        {
            Id = cardHistoryEntity.Id,
            CardId = cardHistoryEntity.CardId,
            CreatedOn = cardHistoryEntity.CreatedOn.ConvertToDateTime(),
            CardHistoryType = cardHistoryEntity.Type,
            MovedSourceListId = cardHistoryEntity.MovedSourceListId,
            MovedTargetListId = cardHistoryEntity.MovedTargetListId
        };
    }
}