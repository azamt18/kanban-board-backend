 using System.Text.Json.Serialization;
 using KanbanBoard.Core;
 using KanbanBoard.Core.Enums;
 using KanbanBoard.Database.Entities;
 using Newtonsoft.Json;

 namespace KanbanBoard.WebApi.ResponseContracts;

public class CardContract
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("createdOn")]
    public string CreatedOn { get; set; }

    [JsonProperty("updatedOn")]
    public string UpdatedOn { get; set; }

    [JsonProperty("isDeleted")]
    public bool IsDeleted { get; set; }

    [JsonProperty("deletedOn")]
    public string? DeletedOn { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("priority")]
    public CardPriority Priority { get; set; }

    [JsonProperty("listId")]
    public int ListId { get; set; }

    [JsonProperty("history")]
    public CardHistoryContract[] HistoryContract { get; set; }
    
    public static CardContract ConvertToContract(CardEntity cardEntity)
    {
        return new CardContract()
        {
            Id = cardEntity.Id,
            CreatedOn = cardEntity.CreatedOn.ConvertToDateTime(),
            UpdatedOn = cardEntity.UpdatedOn.ConvertToDateTime(),
            IsDeleted = cardEntity.IsDeleted,
            DeletedOn = cardEntity.DeletedOn?.ConvertToDateTime(),
            Title = cardEntity.Title,
            Description = cardEntity.Description,
            Priority = cardEntity.Priority,
            ListId = cardEntity.ActiveListId,
            HistoryContract = cardEntity.CardHistories.Select(CardHistoryContract.ConvertToContract).ToArray()
        };
    }
    
}