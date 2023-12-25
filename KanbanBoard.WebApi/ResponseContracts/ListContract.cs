using System.Text.Json.Serialization;
using KanbanBoard.Core;
using KanbanBoard.Database.Entities;
using Newtonsoft.Json;

namespace KanbanBoard.WebApi.ResponseContracts;

public class ListContract
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("createdOn")]
    public string CreatedOn { get; set; }

    [JsonProperty("updatedOn")]
    public string UpdatedOn { get; set; }

    [JsonProperty("isClosed")]
    public bool IsClosed { get; set; }

    [JsonProperty("closedOn")]
    public string? ClosedOn { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    public CardContract[] Cards { get; set; }

    public static ListContract ConvertToContract(ListEntity listEntity)
    {
        return new ListContract()
        {
            Id = listEntity.Id,
            CreatedOn = listEntity.CreatedOn.ConvertToDateTime(),
            UpdatedOn = listEntity.UpdatedOn.ConvertToDateTime(),
            IsClosed = listEntity.IsClosed,
            ClosedOn = listEntity.ClosedOn?.ConvertToDateTime(),
            Title = listEntity.Title,
            Cards = listEntity.Cards.Select(CardContract.ConvertToContract).ToArray()
        };
    }
}