using System.Text.Json.Serialization;
using KanbanBoard.Database.Entities;

namespace KanbanBoard.WebApi.ResponseContracts;

public class ListContract
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("createdOn")]
    public DateTime CreatedOn { get; set; }

    [JsonPropertyName("updatedOn")]
    public DateTime UpdatedOn { get; set; }

    [JsonPropertyName("isClosed")]
    public bool IsClosed { get; set; }

    [JsonPropertyName("closedOn")]
    public DateTime? ClosedOn { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    public CardContract[] Cards { get; set; }

    public static ListContract ConvertToContract(ListEntity listEntity)
    {
        return new ListContract()
        {
            Id = listEntity.Id,
            CreatedOn = listEntity.CreatedOn,
            UpdatedOn = listEntity.UpdatedOn,
            IsClosed = listEntity.IsClosed,
            ClosedOn = listEntity.ClosedOn,
            Title = listEntity.Title,
            Cards = listEntity.Cards.Select(CardContract.ConvertToContract).ToArray()
        };
    }
}