 using System.Text.Json.Serialization;
 using KanbanBoard.Core.Enums;
 using KanbanBoard.Database.Entities;

 namespace KanbanBoard.WebApi.ResponseContracts;

public class CardContract
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("createdOn")]
    public DateTime CreatedOn { get; set; }

    [JsonPropertyName("updatedOn")]
    public DateTime UpdatedOn { get; set; }

    [JsonPropertyName("isDeleted")]
    public bool IsDeleted { get; set; }

    [JsonPropertyName("deletedOn")]
    public DateTime? DeletedOn { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("priority")]
    public CardPriority Priority { get; set; }

    [JsonPropertyName("listId")]
    public int ListId { get; set; }

    [JsonPropertyName("history")]
    public CardHistoryContract[] HistoryContract { get; set; }
    public static CardContract ConvertToContract(CardEntity cardEntity)
    {
        return new CardContract()
        {
            Id = cardEntity.Id,
            CreatedOn = cardEntity.CreatedOn,
            UpdatedOn = cardEntity.UpdatedOn,
            IsDeleted = cardEntity.IsDeleted,
            DeletedOn = cardEntity.DeletedOn,
            Title = cardEntity.Title,
            Description = cardEntity.Description,
            Priority = cardEntity.Priority,
            ListId = cardEntity.ActiveListId,
            HistoryContract = cardEntity.CardHistories.Select(CardHistoryContract.ConvertToContract).ToArray()
        };
    }
    
}